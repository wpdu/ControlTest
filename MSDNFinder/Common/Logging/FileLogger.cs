using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using WinSFA.Common.Utility;

namespace WinSFA.Common.Logging
{
    public class FileLogger : ILogger
    {
        private static readonly string LOG_FILENAME_FORMAT = @"Logs\Log.App._{0}.log";

        private readonly SemaphoreSlim _writeSmaphore = new SemaphoreSlim(1);

        private static readonly string LogMessageBrief =
@"=============================================
Time: {0}
LogType: {1}
Message: {2}
=============================================

";

        private static readonly string LogMessageException =
    @"=============================================
Time: {0}
LogType: {1}
Message: {2}
Exception: {3}
Inner Exception: {4}
Source: {5}
=============================================

";

        public async Task<bool> LogAsync(LogType type, string message)
        {
            var messageToLog = GetMessage(type, message);

            return await WriteToFileAysnc(messageToLog);
        }

        public async Task<bool> LogAsync(LogType type, string message, Exception exception)
        {
            if (exception == null)
            {
                return LogAsync(type, message).Result;
            }

            var messageToLog = GetMessage(type, message, exception);

            return await WriteToFileAysnc(messageToLog);
        }

        private async Task<bool> WriteToFileAysnc(string textToWrite)
        {
            bool result = false;
            try
            {
                await _writeSmaphore.WaitAsync();
                string logFileName = string.Format(LOG_FILENAME_FORMAT, DateTime.Now.ToString("yyyy_MM_dd"));
                await StorageHelper.AppendFileContent(logFileName, textToWrite);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception:" + ex.ToString() + "while trying to log to file at" + DateTime.Now.ToString());
            }

            return result;
        }

        private string GetTimeStamp()
        {
            return DateTime.Now.ToString(@"yyyy-MM-dd HH:mm:ss:fff");
        }

        private string GetMessage(LogType type, string message, Exception exception = null)
        {
            string messageToLog = string.Empty;
            if (exception != null)
            {
                messageToLog = string.Format(LogMessageException,
                    GetTimeStamp(),
                    type.ToString(),
                    message,
                    exception.GetType(),
                    exception.Message,
                    (exception.InnerException != null && exception.InnerException.Message != null) ? exception.InnerException.Message : "None",
                    exception.Source
                    );
            }
            else
            {
                messageToLog = string.Format(LogMessageBrief,
                    GetTimeStamp(),
                    type.ToString(),
                    message
                    );
            }

            return messageToLog;
        }
    }
}