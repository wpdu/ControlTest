using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WinSFA.Common.Network
{
    public struct HttpResult
    {
        public string Result { get; set; }
        public bool Success { get; set; }
    }

    public interface IWebRequestAgent
    {
        bool IsNetworkAvailable { get; }

        Task<HttpResult> GetAsync(string url);

        Task<HttpResult> PostAsync(string url, string data);

        Task<HttpResult> DownloadAndSaveAsync(string url, string filePath, Action<Windows.Web.Http.HttpProgress> progressHandler = null);

        List<Tuple<string, string>> HeaderCollection { get; }
    }
}