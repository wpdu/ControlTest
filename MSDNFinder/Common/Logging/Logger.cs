using System;
using System.Collections.Generic;

namespace WinSFA.Common.Logging
{
    public static class Logger
    {
        private static List<ILogger> LoggerRegisterList;

        static Logger()
        {
            LoggerRegisterList = new List<ILogger>();
            LoggerRegisterList.Add(new FileLogger());
            LoggerRegisterList.Add(new ConsoleLogger());
        }

        /// <summary>
        /// Log message that was a record of the issue
        /// </summary>
        /// <param name="type">Severity of issue</param>
        /// <param name="message">Message to log</param>
        /// <param name="exception">Exception that occured</param>
        public static void Log(LogType type, string message)
        {
            LoggerRegisterList.ForEach(async logger =>
            {
                await logger.LogAsync(type, message);
            });
        }

        /// <summary>
        /// Log message that was a record of the issue
        /// </summary>
        /// <param name="type">Severity of issue</param>
        /// <param name="message">Message to log</param>
        /// <param name="exception">Exception that occured</param>
        public static void Log(LogType type, string message, Exception exception)
        {
            LoggerRegisterList.ForEach(async logger =>
            {
                await logger.LogAsync(type, message, exception);
            });
        }

        public static void LogUIEvent(string eventName)
        {
            Log(LogType.Information, "Excute:" + eventName);
        }
    }
}