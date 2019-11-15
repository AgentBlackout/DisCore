using System.Threading.Tasks;

namespace DisCore.Api.Commands
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

        public CommandResult(CommandResultType resType, object res = null)
        {
            _resultType = resType;
            _result = res;
        }

        public static async Task<CommandResult> Success(object res = null)
        {
            return await Task.Run(() => new CommandResult(CommandResultType.Success, res));
        }

        public static async Task<CommandResult> BadArgs(object res = null)
        {
            return await Task.Run(() => new CommandResult(CommandResultType.BadArgs, res));
        }

        public static async Task<CommandResult> Cooldown(object res = null)
        {
            return await Task.Run(() => new CommandResult(CommandResultType.Cooldown, res));
        }

        public static async Task<CommandResult> PermissionDenied(object res = null)
        {
            return await Task.Run(() => new CommandResult(CommandResultType.PermissionDenied, res));
        }

        public static async Task<CommandResult> Exception(object res = null)
        {
            return await Task.Run(() => new CommandResult(CommandResultType.Exception, res));
        }
    }

}
