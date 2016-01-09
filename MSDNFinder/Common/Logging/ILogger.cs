using System;
using System.Threading.Tasks;

namespace WinSFA.Common.Logging
{
    public enum LogType
    {
        Information = 0,
        Warning = 1,
        Error = 2,
        Exception = 3,
        Unknown = 4
    }

    public interface ILogger
    {
        Task<bool> LogAsync(LogType type, string message);

        Task<bool> LogAsync(LogType type, string message, Exception exception = null);
    }
}