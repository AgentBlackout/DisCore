using System;
using System.Threading.Tasks;

namespace DisCore.Shared.Logging
{
    public class ConsoleLogHandler : ILogHandler
    {
        public Task Log(LogSeverity severity, string message, Exception e = null)
        {
            Console.WriteLine(
                $"{DateTime.UtcNow} [{EnumAsString(severity).ToUpper()}] {(string.IsNullOrEmpty(message) ? "" : message)}{(e != null ? ($" ( {e.ToString()} )") : "")}");
            return Task.CompletedTask;
        }

        public Task LogDebug(string message, Exception e = null) => Log(LogSeverity.Debug, message, e);

        public Task LogInfo(string message, Exception e = null) => Log(LogSeverity.Info, message, e);

        public Task LogWarning(string message, Exception e = null) => Log(LogSeverity.Warning, message, e);

        public Task LogError(string message, Exception e = null) => Log(LogSeverity.Error, message, e);

        public Task LogFatal(string message, Exception e = null) => Log(LogSeverity.Fatal, message, e);

        private string EnumAsString(Enum e) => Enum.GetName(e.GetType(), e);

    }
}