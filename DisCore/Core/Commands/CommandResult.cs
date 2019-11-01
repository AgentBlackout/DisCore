namespace DisCore.Core.Commands
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
        private CommandResultType resultType;

        private object result;

        public CommandResult(CommandResultType resType, object res = null)
        {
            resultType = resType;
            result = res;
        }

        public static CommandResult Success(object res = null)
        {
            return new CommandResult(CommandResultType.Success, res);
        }

        public static CommandResult BadArgs(object res = null)
        {
            return new CommandResult(CommandResultType.BadArgs, res);
        }

        public static CommandResult Cooldown(object res = null)
        {
            return new CommandResult(CommandResultType.Cooldown, res);
        }

        public static CommandResult PermissionDenied(object res = null)
        {
            return new CommandResult(CommandResultType.PermissionDenied, res);
        }

        public static CommandResult Exception(object res = null)
        {
            return new CommandResult(CommandResultType.Exception, res);
        }
    }

}
