using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DisCore.Core.Logging
{
    public enum LogSeverity
    {
        Debug, //Debug
        Info, //Information
        Warning, //Minor issue
        Error, // Major issue
        Fatal //Irrecoverable issue
    }

    public interface ILogHandler
    {
        Task Log(LogSeverity severity, String message, Exception e = null);
        Task LogDebug(String message, Exception e = null);
        Task LogInfo(String message, Exception e = null);
        Task LogWarning(String message, Exception e = null);
        Task LogError(String message, Exception e = null);
        Task LogFatal(String message, Exception e = null);

    }
}
