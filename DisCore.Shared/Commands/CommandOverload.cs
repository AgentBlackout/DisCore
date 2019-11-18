using System;
using System.Reflection;
using DisCore.Shared.Commands.Timeout;
using DisCore.Shared.Permissions;

namespace DisCore.Shared.Commands
{
    public class CommandOverload
    {
        private readonly MethodInfo _methodInfo;
        private readonly TimeoutAttribute _timeout;
        private readonly RequiredPermissions _perms;

        public CommandOverload(MethodInfo methodInfo, RequiredPermissions perms = null, TimeoutAttribute timeout = null)
        {
            _methodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));
            _perms = perms;
            _timeout = timeout;
        }

        public MethodInfo GetMethod() => _methodInfo;

        //TODO: Overloads / params

    }
}
