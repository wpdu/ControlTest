using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using WinSFA.Common.Utility;
using Windows.Storage;
using Windows.Web.Http;
using System.Net;

namespace WinSFA.Common.Network
{
    public class WebRequestAgent : IWebRequestAgent
    {
        #region Private Fields

        private List<Tuple<string, string>> _headerCollection = null;

        #endregion Private Fields

        #region Public Constructors

        public WebRequestAgent()
        {
            _headerCollection = new List<Tuple<string, string>>();
        }

        #endregion Public Constructors

        #region Public Properties

        public List<Tuple<string, string>> HeaderCollection
        {
            get
            {
                return _headerCollection ?? (_headerCollection = new List<Tuple<string, string>>());
            }
        }

        public bool IsNetworkAvailable
        {
            get
            {
                return Utility.NetworkInfoHelper.Current.IsNetworkAvaliable;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public async Task<HttpResult> GetAsync(string url)
        {
            HttpResult ret = new HttpResult();
            try
            {
                using (HttpClient client = new HttpClient())
                {

                    TryAddHeaders(client);
                    using (HttpResponseMessage response = await client.GetAsync(new Uri(url)))
                    {
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            var responseText = await response.Content.ReadAsStringAsync();
                            ret.Result = responseText;
                            ret.Success = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(Logging.LogType.Exception, "GetAsync:" + url, ex);
            }

            return ret;
        }

        public async Task<HttpResult> PostAsync(string url, string data)
        {
            HttpResult ret = new HttpResult();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    TryAddHeaders(client);
                    HttpResponseMessage response = null;
                    if (string.IsNullOrWhiteSpace(data))
                    {
                        response = await client.PostAsync(new Uri(url), null);
                    }
                    else
                    {
                        using (HttpStringContent content = new HttpStringContent(data))
                        {
                            response = await client.PostAsync(new Uri(url), content);
                        }
                    }
                    if (response?.IsSuccessStatusCode == true)
                    {
                        byte[] resultBytes = null;
                        var buffer = await response.Content.ReadAsBufferAsync();
                        Windows.Storage.Streams.DataReader reader = Windows.Storage.Streams.DataReader.FromBuffer(buffer);
                        resultBytes = new byte[buffer.Length];
                        reader.ReadBytes(resultBytes);

                        //Check if byte[] need to decompress
                        if (resultBytes.Length > 2 && resultBytes[0] == 120 && resultBytes[1] == 156)
                        {
                            //var unZipBytes = ZipUtil.Inflate(resultBytes);
                            //ret.Result = System.Text.Encoding.UTF8.GetString(unZipBytes);
                        }
                        else
                        {
                            ret.Result = System.Text.Encoding.UTF8.GetString(resultBytes);
                        }
                        ret.Success = true;
                    }
                    else
                    {
                        Logging.Logger.Log(Logging.LogType.Error, "PostAsync:" + url + ",Status Code:" + response.StatusCode, null);
                    }
                    response.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(Logging.LogType.Exception, "PostAsync:" + url, ex);
            }

            return ret;
        }

        public async Task<HttpResult> DownloadAsync(string url, string filePath, Action<HttpProgress> progressHandler = null)
        {
            HttpResult ret = new HttpResult();
            try
            {
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response;
                if (progressHandler != null)
                {
                    IProgress<HttpProgress> progress = new Progress<HttpProgress>(progressHandler);
                    response = await httpClient.GetAsync(new Uri(url), HttpCompletionOption.ResponseContentRead).AsTask(progress);
                }
                else
                {
                    response = await httpClient.GetAsync(new Uri(url), HttpCompletionOption.ResponseContentRead);
                }

                if (response?.StatusCode == Windows.Web.Http.HttpStatusCode.Ok)
                {
                    var storageFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(filePath, CreationCollisionOption.ReplaceExisting);
                    using (StorageStreamTransaction transaction = await storageFile.OpenTransactedWriteAsync())
                    {
                        await response.Content.WriteToStreamAsync(transaction.Stream);
                        await transaction.CommitAsync();
                    }
                    ret.Success = true;
                }
                response?.Dispose();
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(Logging.LogType.Exception, "DownloadAsync:" + url, ex);
            }

            return ret;
        }

        public async Task<HttpResult> DownloadAndSaveAsync(string url, string filePath, Action<HttpProgress> progressHandler = null)
        {
            HttpResult ret = new HttpResult();
            try
            {
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response;
                if (progressHandler != null)
                {
                    IProgress<HttpProgress> progress = new Progress<HttpProgress>(progressHandler);
                    response = await httpClient.GetAsync(new Uri(url), HttpCompletionOption.ResponseContentRead).AsTask(progress);
                }
                else
                {
                    response = await httpClient.GetAsync(new Uri(url), HttpCompletionOption.ResponseContentRead);
                }

                if (response?.StatusCode == Windows.Web.Http.HttpStatusCode.Ok)
                {
                    var storageFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(filePath, CreationCollisionOption.ReplaceExisting);
                    using (StorageStreamTransaction transaction = await storageFile.OpenTransactedWriteAsync())
                    {
                        await response.Content.WriteToStreamAsync(transaction.Stream);
                        await transaction.CommitAsync();
                    }
                    ret.Success = true;
                }
                response?.Dispose();
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(Logging.LogType.Exception, "DownloadAsync:" + url, ex);
            }

            return ret;
        }


        #endregion Public Methods

        #region Private Methods

        private void TryAddHeaders(HttpClient client)
        {
            HeaderCollection?.ForEach(header => client.DefaultRequestHeaders.Add(header.Item1, header.Item2));
        }

        #endregion Private Methods
    }
}