using System.Threading.Tasks;

namespace DisCore.Shared.Commands
{
    public enum CommandResultType
    {
        Success,
        BadArgs,
        Cooldown,
        PermissionDenied,
        Exception
    }

    public class CommandResult
    {
        private CommandResultType _resultType;

        private object _result;

        private CommandResult(CommandResultType resType, object res = null)
        {
            _resultType = resType;
            _result = res;
        }

        public static async Task<CommandResult> Success() =>
            await Task.Run(() => new CommandResult(CommandResultType.Success));

        public static async Task<CommandResult> BadArgs(object res = null) =>
            await Task.Run(() => new CommandResult(CommandResultType.BadArgs, res));

        public static async Task<CommandResult> Cooldown(object res = null) =>
            await Task.Run(() => new CommandResult(CommandResultType.Cooldown, res));

        public static async Task<CommandResult> PermissionDenied(object res = null) =>
            await Task.Run(() => new CommandResult(CommandResultType.PermissionDenied, res));

        public static async Task<CommandResult> Exception(object res = null) =>
            await Task.Run(() => new CommandResult(CommandResultType.Exception, res));

    }

}
