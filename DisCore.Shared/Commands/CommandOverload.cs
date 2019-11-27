using System;
using System.Collections.Generic;
using System.Linq;
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

        public TimeoutAttribute GetTimeout() => _timeout;
        public RequiredPermissions GetRequiredPermissions() => _perms;

        public MethodInfo GetMethod() => _methodInfo;

        public IEnumerable<ParameterInfo> GetParameters() => _methodInfo.GetParameters();

        public IEnumerable<Type> GetParameterTypes() => _methodInfo.GetParameters().Select(pInfo => pInfo.ParameterType);

    }
}
