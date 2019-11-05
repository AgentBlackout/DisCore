using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using DisCore.Core.Commands.Timeouts;
using DisCore.Core.Permissions;

namespace DisCore.Core.Commands
{
    public class Command
    {
        private MethodInfo _methodInfo;
        private TimeoutAttribute _timeout;
        private RequiredPermissions _perms;

        public Command(MethodInfo methodInfo, RequiredPermissions perms, TimeoutAttribute timeout = null)
        {
            _methodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));
            _perms = perms ?? throw new ArgumentNullException(nameof(perms));
        }
        
    }
}
