using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace WinSFA.Common.Utility
{
    public class ImageHelper
    {
        private static string folderName = string.Empty;
        private static string fileName = string.Empty;
        private Action<string> onDownloaded = null;

        public async Task<BitmapImage> Download(string uri, string folder, string file)
        {
            BitmapImage bi = null;
            try
            {
                //download
                HttpWebRequest request = HttpWebRequest.CreateHttp(new Uri(uri));
                request.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();

                //save
                StorageFolder local = ApplicationData.Current.LocalFolder;
                var dataFolder = await local.CreateFolderAsync(folder, CreationCollisionOption.OpenIfExists);

                using (Stream stream = response.GetResponseStream())
                {
                    bi = new BitmapImage();

                    byte[] fileBytes = new byte[response.ContentLength];
                    await stream.ReadAsync(fileBytes, 0, (int)response.ContentLength);

                    var newFile = await dataFolder.CreateFileAsync(file, CreationCollisionOption.ReplaceExisting);

                    // Write the data
                    using (var s = await newFile.OpenStreamForWriteAsync())
                    {
                        await s.WriteAsync(fileBytes, 0, fileBytes.Length);
                    }
                }

                //read
                dataFolder = await local.GetFolderAsync(folder);

                // Get the file.
                StorageFile storageFile = await dataFolder.GetFileAsync(file);
                IRandomAccessStream fileStream = await storageFile.OpenAsync(FileAccessMode.Read);
                bi = new BitmapImage();
                await bi.SetSourceAsync(fileStream);
                return bi;
            }
            catch (WebException e)
            {
            }
            catch (Exception e)
            {
            }
            return bi;
        }

        public void Download(string uri, string folder, string file, Action<string> callback)
        {
            try
            {
                folderName = folder;
                fileName = file;
                onDownloaded = callback;

                HttpWebRequest request = HttpWebRequest.CreateHttp(new Uri(uri));
                request.Method = "GET";
                request.BeginGetResponse(Download_Callback, request);
            }
            catch (WebException e)
            {
            }
            catch (Exception e)
            {
            }
        }

        private async void Download_Callback(IAsyncResult result)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)result.AsyncState;
                WebResponse response = request.EndGetResponse(result);

                using (Stream stream = response.GetResponseStream())
                {
                    byte[] fileBytes = new byte[response.ContentLength];
                    await stream.ReadAsync(fileBytes, 0, (int)response.ContentLength);

                    // Get the local folder.
                    StorageFolder local = ApplicationData.Current.LocalFolder;

                    // Create a new folder
                    var dataFolder = await local.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);

                    // Create a new file
                    var file = await dataFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

                    // Write the data
                    using (var s = await file.OpenStreamForWriteAsync())
                    {
                        await s.WriteAsync(fileBytes, 0, fileBytes.Length);
                    }

                    onDownloaded(file.Path);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
        }

        public async Task<BitmapImage> ReadImage(string folder, string file)
        {
            try
            {
                // Get the local folder.
                StorageFolder local = ApplicationData.Current.LocalFolder;

                if (local != null)
                {
                    // Get the DataFolder folder.
                    var dataFolder = await local.GetFolderAsync(folder);

                    // Get the file.
                    StorageFile storageFile = await dataFolder.GetFileAsync(file);
                    IRandomAccessStream stream = await storageFile.OpenAsync(FileAccessMode.Read);

                    // Read the data.
                    var bi = new BitmapImage();
                    await bi.SetSourceAsync(stream);
                    return bi;
                }
            }
            catch (Exception ex)
            {
            }

            return null;
        }
    }
}