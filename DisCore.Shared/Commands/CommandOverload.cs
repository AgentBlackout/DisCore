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
        private readonly CommandGroup _parent;

        public CommandOverload(MethodInfo methodInfo, CommandGroup parent, RequiredPermissions perms = null, TimeoutAttribute timeout = null)
        {
            _methodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));
            _parent = parent ?? throw new ArgumentNullException(nameof(parent));
            _perms = perms;
            _timeout = timeout;
        }

        public TimeoutAttribute GetTimeout() => _timeout ?? _parent.Timeout;
        public RequiredPermissions GetRequiredPermissions() => _perms ?? _parent.RequiredPermissions;

        public MethodInfo GetMethod() => _methodInfo;

        public IEnumerable<ParameterInfo> GetParameters() => _methodInfo.GetParameters();

        public IEnumerable<Type> GetParameterTypes() => _methodInfo.GetParameters().Select(pInfo => pInfo.ParameterType);

        public CommandGroup GetGroup() => _parent;
    }
}
