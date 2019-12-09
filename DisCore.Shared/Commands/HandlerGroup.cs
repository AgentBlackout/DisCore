using System;
using DisCore.Shared.Commands.Timeout;
using DisCore.Shared.Logging;
using DisCore.Shared.Permissions;

namespace DisCore.Shared.Commands
{
    public readonly struct HandlerGroup
    {
        public readonly ITimeoutHandler TimeoutHandler;
        public readonly IPermissionHandler PermissionHandler;
        public readonly ILogHandler LogHandler;

        public HandlerGroup(ITimeoutHandler timeoutHandler, IPermissionHandler permissionHandler, ILogHandler logHandler)
        {
            TimeoutHandler = timeoutHandler ?? throw new ArgumentNullException(nameof(timeoutHandler));
            PermissionHandler = permissionHandler ?? throw new ArgumentNullException(nameof(permissionHandler));
            LogHandler = logHandler ?? throw new ArgumentNullException(nameof(logHandler));
        }
    }
}
