using MSDNFinder.Model.DataJson;
using MSDNFinder.ViewModel;
using Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using WinSFA.Common.Network;
using WinSFA.Common.Serializer;
using WinSFA.Common.Utility;

namespace MSDNFinder.Manager
{
    public class DataManager
    {
        const string BaseUrlKey = "BaseUrl";
        const string BaseUrl0 = "https://msdn.microsoft.com/zh-cn/library/windows/apps/hh703192.aspx";
        private static string _baseUrl;

        public static string BaseUrl
        {
            get
            {
                _baseUrl = StorageHelper.GetSetting<string>(BaseUrlKey, BaseUrl0);
                return _baseUrl;
            }
        }

        public static ServerModel GetCurrData()
        {
            var conn = LocalDatabaseService.GetMainConnection();
            var data = conn.Table<ServerModel>().Where(c => c.Href.Equals(BaseUrl)).ToList();
            if (data.Count == 0)
            {
                ServerModel s = new ServerModel()
                {
                    _id = 0,
                    Title = "Windows",
                    Href = BaseUrl,
                    ToolTip = "Windows",
                    hassubtree = true,
                    parentid = 0,
                    level = 0
                };
                conn.Table<ServerModel>().Delete(c => 1 == 1);
                conn.Insert(s);
                conn.Dispose();
                return s;
            }
            else
            {
                return data[0];
            }

        }

        const string Accept = "application/json, text/javascript, */*; q=0.01";
        const string Accept_Encoding = "gzip, deflate, sdch";
        const string Accept_Language = "zh-CN,zh;q=0.8";
        const string User_Agent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.154 Safari/537.36 LBBROWSER";
        const string X_Requested_With = "XMLHttpRequest";

        public static async Task<List<ServerModel>> GetData(ClientModel value)
        {
            if (value == null) return null;
            List<ServerModel> data = new List<ServerModel>();

            var conn = LocalDatabaseService.GetMainConnection();
            data = conn.Table<ServerModel>().Where(c => c.parentid == value.serverModel._id).ToList();
            conn.Dispose();
            if (data.Count != 0)
                return data;

            if (value.serverModel.hassubtree)
            {
                data = await DownloadData(value.serverModel);
                value.ChildrenSaved = 1;
            }
            return data;
        }

        public static List<ServerModel> GetData(string value)
        {
            if (value == null) new List<ServerModel>();
            List<ServerModel> data = new List<ServerModel>();

            var conn = LocalDatabaseService.GetMainConnection();
            data = conn.Table<ServerModel>().Where(c => c.Title.Contains(value)).ToList();
            conn.Dispose();
            return data;
        }

        static object DBSynchObj = new object();

        public static async Task<List<ServerModel>> DownloadData(ServerModel value)
        {
            List<ServerModel> data = new List<ServerModel>();

            string url = string.Format("{0}{1}", value.Href, "?toc=1");

            WebRequestAgent request = new WebRequestAgent();
            var res = await request.GetAsync(url);
            if (res.Success)
            {
                try
                {
                    string hasSub;
                    data = JsonNetSerializer.DeserializeList<ServerModel>(res.Result);
                    //Debug.Write(res.Result);

                    foreach (var item in data)
                    {
                        hasSub = item.ExtendedAttributes.data_tochassubtree;
                        if (!string.IsNullOrEmpty(hasSub) && hasSub.Equals("true"))
                            item.hassubtree = true;
                        else
                            item.hassubtree = false;
                        item.parentid = value._id;
                        item.level = value.level + 1;
                    }
                    lock (DBSynchObj)
                    {
                        var conn = LocalDatabaseService.GetMainConnection();
                        conn.InsertAll(data);
                        value.subtreesaved = true;
                        conn.Update(value);
                        conn.Dispose();

                    }
                }
                catch (Exception ex)
                {
                    if (data.Count == 0)
                        Debug.Write("Error: " + "数据反序列话错误。");
                    else
                    {
                        Debug.Write("Error: " + "数据库插入数据错误。");
                    }
                }
            }

            return data;
        }


        public static async Task<bool> DownloadAllJson(Progress progress)
        {
            Dictionary<ServerModel, List<ServerModel>> loadedData = new Dictionary<ServerModel, List<ServerModel>>();
            Queue<ServerModel> dlQueue = new Queue<ServerModel>();
            List<ServerModel> data = null;

            var conn = LocalDatabaseService.GetMainConnection();
            data = conn.Table<ServerModel>().Where(c => c.hassubtree && c.subtreesaved == false && c.skepdownload == false).ToList();
            data.ForEach(c => dlQueue.Enqueue(c));
            progress.Total = dlQueue.Count();

            FastDownload h5TaskState = new FastDownload() { FileQueue = dlQueue, ValueCollector = loadedData, Prog = progress};

            while (dlQueue.Count > 0 && !FastDownload.isStop)
            {
                loadedData.Clear();

                if (dlQueue.Count > 10)
                {
                    Task<Task<bool>> t0 = new Task<Task<bool>>(async () => { return await h5TaskState.DownloadFile(0); });
                    Task<Task<bool>> t1 = new Task<Task<bool>>(async () => { return await h5TaskState.DownloadFile(1); });
                    Task<Task<bool>> t2 = new Task<Task<bool>>(async () => { return await h5TaskState.DownloadFile(2); });
                    Task<Task<bool>> t3 = new Task<Task<bool>>(async () => { return await h5TaskState.DownloadFile(3); });
                    Task<Task<bool>> t4 = new Task<Task<bool>>(async () => { return await h5TaskState.DownloadFile(4); });

                    Task<Task<bool>>[] tasks = new Task<Task<bool>>[5];
                    tasks[0] = t0;
                    tasks[1] = t1;
                    tasks[2] = t2;
                    tasks[3] = t3;
                    tasks[4] = t4;
                    t0.Start();
                    t1.Start();
                    t2.Start();
                    t3.Start();
                    t4.Start();
                    Task<bool>[] result = await Task.WhenAll(tasks);
                    for (int i = 0; i < 5; i++)
                    {
                        bool res = await result[i];
                    }
                }
                else
                {
                    bool ok = await h5TaskState.DownloadFile(0);

                }
                conn = LocalDatabaseService.GetMainConnection();

                foreach (var item in loadedData)
                {
                    conn.InsertAll(item.Value);
                    item.Key.subtreesaved = true;
                    conn.Update(item.Key);
                    progress.Saved++;
                    await Task.Delay(20);
                }

                data = conn.Table<ServerModel>().Where(c => c.hassubtree && c.subtreesaved == false && c.skepdownload == false).ToList();
                data.ForEach(c => dlQueue.Enqueue(c));
                conn.Dispose();
                progress.Total += data.Count;
            }
            return true;
        }

        public static async Task<KeyValuePair<ServerModel, List<ServerModel>>> GetDataFromServer(ServerModel sm)
        {
            List<ServerModel> value = new List<ServerModel>();
            KeyValuePair<ServerModel, List<ServerModel>> data = new KeyValuePair<ServerModel, List<ServerModel>>(sm, value);

            string url = string.Format("{0}{1}", sm.Href, "?toc=1");

            WebRequestAgent request = new WebRequestAgent();
            var res = await request.GetAsync(url);
            if (res.Success)
            {
                try
                {
                    string hasSub;
                    
                    value = JsonNetSerializer.DeserializeList<ServerModel>(res.Result);
                    //Debug.Write(res.Result);

                    foreach (var item in value)
                    {
                        hasSub = item.ExtendedAttributes.data_tochassubtree;
                        if (!string.IsNullOrEmpty(hasSub) && hasSub.Equals("true"))
                            item.hassubtree = true;
                        else
                            item.hassubtree = false;
                        item.parentid = sm._id;
                        item.level = sm.level + 1;
                    }
                    value.ForEach(c => data.Value.Add(c));
                }
                catch (Exception ex)
                {
                    if (value.Count == 0)
                        Debug.Write("Error: " + "数据反序列话错误。");
                    else
                    {
                        Debug.Write("Error: " + "数据库插入数据错误。");
                    }
                }
            }

            return data;
        }


        public static async Task<string> DownloadPage(ClientModel value)
        {
            string path = value.GetPath();
            string absolutePath = "ms-appdata:///local/" + path.Replace("\\", "/");
            if (!await StorageHelper.CheckFileExist(path))
            {
                WebRequestAgent request = new WebRequestAgent();
                var res = await request.DownloadAsync(value.serverModel.Href, path);
                if (res.Success)
                {
                    return absolutePath;
                }
            }
            else
            {
                return absolutePath;
            }
            return null;
        }

        internal static int GetTotal()
        {
            var conn = LocalDatabaseService.GetMainConnection();
            int count = conn.Table<ServerModel>().Where(c => c.hassubtree).Count();
            conn.Dispose();
            return count;
        }

        internal static int GetSaved()
        {
            var conn = LocalDatabaseService.GetMainConnection();
            int count = conn.Table<ServerModel>().Where(c => c.hassubtree && c.subtreesaved == true).Count();
            conn.Dispose();
            return count;
        }

        internal static bool UpdateObj(ClientModel tempCM)
        {
            var conn = LocalDatabaseService.GetMainConnection();
            int res = conn.Update(tempCM.serverModel);
            conn.Dispose();
            return res == 1 ? true : false;
        }
    }

    public class FastDownload
    {
        public static bool isStop = false;

        object syncObj = new object();
        public Queue<ServerModel> FileQueue = new Queue<ServerModel>();
        public Dictionary<ServerModel, List<ServerModel>> ValueCollector;
        public Progress Prog;

        public async Task<bool> DownloadFile(int index)
        {
            try
            {
                ServerModel sm = null;

                while (GetNext(FileQueue, out sm) && !isStop)
                {
                    //List<ServerModel> tempdata = await DataManager.DownloadData(sm);
                    KeyValuePair<ServerModel, List<ServerModel>> tempdata = await DataManager.GetDataFromServer(sm);
                    Debug.WriteLine("Index:\t" + index);

                    ValueCollector.Add(tempdata.Key, tempdata.Value);
                    if (Window.Current != null && Window.Current.Content != null)
                    {
                        CommonHelper.InvokeOnUI(() => { Prog.Saved++; });
                    }


                    await Task.Delay(20);

                    //lock (FileQueue)
                    //{
                    //    Debug.WriteLine(index + "\t in");
                    //    progress.Saved++;
                    //    tempdata.ForEach(c => {
                    //        if (c.hassubtree)
                    //        {
                    //            FileQueue.Enqueue(c);
                    //            progress.Total++;
                    //        }
                    //    });
                    //    Debug.WriteLine(index + "\t out");
                    //}
                }

                return true;
            }
            catch (Exception ex)
            {
                
            }
            return false;
        }

        private bool GetNext(Queue<ServerModel> fileQueue, out ServerModel sm)
        {
            lock (syncObj)
            {
                sm = null;
                if (fileQueue.Count > 0)
                {
                    sm = FileQueue.Dequeue();
                    return true;
                }
                
                return false;
            }
        }

    }
}
