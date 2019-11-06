using System;
using System.Reflection;
using DisCore.Core.Commands;
using DisCore.Core.Commands.Timeouts;
using DisCore.Core.Permissions;

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