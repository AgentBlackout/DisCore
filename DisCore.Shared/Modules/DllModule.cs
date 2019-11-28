using System;
using System.Collections.Generic;
using System.Reflection;
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
        public readonly Assembly Assembly;

        public readonly IModule ModuleObject;

        public List<CommandGroup> Commands;

        public String Summary => ModuleObject.Summary;

        public IPermissionHandler PermissionHandler;
        public ITimeoutHandler TimeoutHandler;

        public ILogHandler LogHandler;
        public ICommandParser Parser;

        public DllModule(IModule modObject, Assembly assembly)
        {
            ModuleObject = modObject;
            Assembly = assembly;
            Info = ModuleInfoFactory.GetModuleInfo(ModuleObject);

        }

    }
}
