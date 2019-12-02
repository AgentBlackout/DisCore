using DisCore.Shared.Commands;
using DisCore.Shared.Permissions;
using System.Reflection;
using DisCore.Shared.Commands.Timeout;

namespace DisCore.Runner.Factories
{
    public class CommandFactory
    {
        public static CommandOverload GetCommand(MethodInfo mInfo, CommandGroup parent)
        {
            var require = (RequiredPermissions)mInfo.GetCustomAttribute(typeof(RequiredPermissions));
            var timeout = (TimeoutAttribute)mInfo.GetCustomAttribute(typeof(TimeoutAttribute));

            return new CommandOverload(mInfo, parent, require, timeout);
        }
    }
}