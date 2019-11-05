using System;
using System.Threading.Tasks;

namespace DisCore.Core.Logging
{
    public class LogHandler : ILogHandler
    {
        public Task Log(LogSeverity severity, string message, Exception e = null) =>
            Task.Run(() =>
                Console.WriteLine(
                    $"{DateTime.UtcNow} [{Enum.GetName(typeof(LogSeverity), severity).ToUpper()}] {(string.IsNullOrEmpty(message) ? "" : message)}{(e != null ? " -" + e.ToString() : "")}")
                );

        public Task LogDebug(string message, Exception e = null) => Log(LogSeverity.Debug, message, e);

        public Task LogInfo(string message, Exception e = null) => Log(LogSeverity.Info, message, e);

        public Task LogWarning(string message, Exception e = null) => Log(LogSeverity.Warning, message, e);

        public Task LogError(string message, Exception e = null) => Log(LogSeverity.Error, message, e);


        public Task LogFatal(string message, Exception e = null) => Log(LogSeverity.Fatal, message, e);
    }
}