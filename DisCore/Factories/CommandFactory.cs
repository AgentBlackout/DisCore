using System.Reflection;
using DisCore.Api.Commands.Timeout;
using DisCore.Api.Permissions;
using DisCore.Core.Commands;

namespace DisCore.Factories
{
    public class CommandFactory
    {
        public static Command GetCommand(MethodInfo mInfo)
        {
            var require = (RequiredPermissions)mInfo.GetCustomAttribute(typeof(RequiredPermissions));
            var timeout = (TimeoutAttribute)mInfo.GetCustomAttribute(typeof(TimeoutAttribute));

            return new Command(mInfo, require, timeout);
        }
    }
}