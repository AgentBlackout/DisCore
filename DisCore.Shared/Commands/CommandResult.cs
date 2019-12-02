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
        public readonly CommandResultType Result;

        public readonly object Object;

        private CommandResult(CommandResultType resType, object res = null)
        {
            Result = resType;
            Object = res;
        }

        public static CommandResult Success() => new CommandResult(CommandResultType.Success);

        public static CommandResult BadArgs(object res = null) => new CommandResult(CommandResultType.BadArgs, res);

        public static CommandResult Cooldown(object res = null) => new CommandResult(CommandResultType.Cooldown, res);

        public static CommandResult PermissionDenied(object res = null) => new CommandResult(CommandResultType.PermissionDenied, res);

        public static CommandResult Exception(object res = null) => new CommandResult(CommandResultType.Exception, res);

    }

}
