using System;
using System.Threading.Tasks;

namespace WinSFA.Common.Logging
{
    public class ConsoleLogger : ILogger
    {
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

        public Task<bool> LogAsync(LogType type, string message)
        {
            var messageToLog = GetMessage(type, message);
            System.Diagnostics.Debug.WriteLine(messageToLog);

            return Task.FromResult(true);
        }

        public Task<bool> LogAsync(LogType type, string message, Exception exception)
        {
            if (exception == null)
            {
                return LogAsync(type, message);
            }
            var messageToLog = GetMessage(type, message, exception);
            System.Diagnostics.Debug.WriteLine(messageToLog);

            return Task.FromResult<bool>(true);
        }

        private static string GetTimeStamp()
        {
            return DateTime.Now.ToString(@"yyyy-MM-dd HH:mm:ss:fff");
        }

        private static string GetMessage(LogType type, string message, Exception exception = null)
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