using System;
using System.Reflection;
using DisCore.Api.Commands.Timeout;
using DisCore.Api.Permissions;

namespace DisCore.Core.Commands
{
    public class Command
    {
        private readonly MethodInfo _methodInfo;
        private readonly TimeoutAttribute _timeout;
        private readonly RequiredPermissions _perms;

        public Command(MethodInfo methodInfo, RequiredPermissions perms = null, TimeoutAttribute timeout = null)
        {
            _methodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));
            _perms = perms;
            _timeout = timeout;
        }

        //TODO: Overloads / params

    }
}
