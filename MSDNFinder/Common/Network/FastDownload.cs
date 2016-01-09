using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSDNFinder.Common.Network
{
    public class FastDownload
    {
        //private int failTimes;
        //public Queue<OfflineInfo> FileQueue { get; set; }

        //public object FailTimes
        //{
        //    get
        //    {
        //        return failTimes;
        //    }
        //    set
        //    {
        //        if (value is int)
        //            failTimes = (int)value;
        //    }
        //}

        //public bool AllSaved { get; set; }
        //public string Url { get; set; }
        
        //public async Task<bool> DownloadFile(int id)
        //{
        //    OfflineInfo info = null;
        //    while (GetH5Info(FileQueue, out info))
        //    {
        //        if (!await V1OfflineJsonClass.DownLoadH5Func(info.Name, info, id.ToString(), Url))
        //        {
        //            AllSaved = false;
        //            lock (FailTimes)
        //            {
        //                FailTimes = (int)FailTimes + 1;
        //                if ((int)FailTimes >= 5)
        //                    break;
        //            }
        //            info.DownloadStatus = 0;
        //            FileQueue.Enqueue(info);
        //        }
        //        else
        //        {
        //            if ((int)FailTimes < 5)
        //                FailTimes = 0;
        //        }
        //    }
        //    if (FileQueue.Count != 0)
        //    {
        //        return AllSaved;
        //    }
        //    else
        //    {
        //        return true;
        //    }

        //}

        //bool GetH5Info(Queue<OfflineInfo> h5Files, out OfflineInfo info)
        //{
        //    lock (h5Files)
        //    {
        //        if (h5Files.Count > 0)
        //        {
        //            if (h5Files.Count == 1)
        //            {

        //            }
        //            info = h5Files.Dequeue();
        //            if (info.DownloadTimes < 2)
        //                return true;
        //        }
        //        info = null;
        //        return false;
        //    }
        //}

        //public static Func<string, OfflineInfo, string, string, Task<bool>> DownLoadH5Func;
        //private static FastDownload h5TaskState;
        //public async Task<bool> DownloadH5File()
        //{
        //    if (H5FileDic == null) return true;

        //    int failTimes = 0;
        //    object failTimesObj = failTimes;
        //    string url = Host.Contains("/") ? Host.Split('/')[2].Split(':')[0] : Host;
        //    Queue<OfflineInfo> h5Files = new Queue<OfflineInfo>(H5FileDic.Values);
        //    CancellationToken cancelToken = new CancellationToken();

        //    int MaxTask = h5Files.Count / 10 + 1;
        //    MaxTask = MaxTask > 5 ? 5 : MaxTask;
        //    Task<Task<bool>>[] tasks = new Task<Task<bool>>[MaxTask];
        //    h5TaskState = new FastDownload() { FileQueue = h5Files, AllSaved = true, FailTimes = 0, Url = url };


        //    Task<Task<bool>> t0 = new Task<Task<bool>>(async () => { return await h5TaskState.DownloadFile(0); });
        //    Task<Task<bool>> t1 = new Task<Task<bool>>(async () => { return await h5TaskState.DownloadFile(1); });
        //    Task<Task<bool>> t2 = new Task<Task<bool>>(async () => { return await h5TaskState.DownloadFile(2); });
        //    Task<Task<bool>> t3 = new Task<Task<bool>>(async () => { return await h5TaskState.DownloadFile(3); });
        //    Task<Task<bool>> t4 = new Task<Task<bool>>(async () => { return await h5TaskState.DownloadFile(4); });
        //    tasks[0] = t0;
        //    tasks[1] = t1;
        //    tasks[2] = t2;
        //    tasks[3] = t3;
        //    tasks[4] = t4;
        //    t0.Start();
        //    t1.Start();
        //    t2.Start();
        //    t3.Start();
        //    t4.Start();
        //    Task<bool>[] result = await Task.WhenAll(tasks);
        //    for (int i = 0; i < MaxTask; i++)
        //    {
        //        bool res = await result[0];
        //    }

        //    if (h5TaskState.AllSaved)
        //    {
        //        DataBaseManager.V1SaveDownloadedFileDesc(H5FileDic, DataBaseManager.H5_DESC, H5Hash);
        //        return true;
        //    }

        //    return h5TaskState.AllSaved;
        //}
    }
}
