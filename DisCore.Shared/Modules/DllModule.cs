using System;
using System.Collections.Generic;
using DisCore.Shared.Commands;
using DisCore.Shared.Commands.Parser;
using DisCore.Shared.Commands.Timeout;
using DisCore.Shared.Events;
using DisCore.Shared.Factories;
using DisCore.Shared.Logging;
using DisCore.Shared.Permissions;

namespace DisCore.Shared.Modules
{
    public class DllModule
    {
        public readonly ModuleInfo Info;

        private readonly IModule _moduleObject;

        public List<CommandGroup> Commands;

        public String Summary => _moduleObject.Summary;

        public IPermissionHandler PermissionHandler;
        public ITimeoutHandler TimeoutHandler;
        public IEventHandler EventHandler;

        public ILogHandler LogHandler;
        public ICommandParser Parser;

        public DllModule(IModule modObject)
        {
            _moduleObject = modObject;
            Info = ModuleInfoFactory.GetModuleInfo(_moduleObject);

        }

    }
}
