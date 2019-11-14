using System;
using System.Collections.Generic;
using DisCore.Core.Commands;
using DisCore.Core.Loaders.Module;
using DisCore.Factories;

namespace DisCore.Core.Entities.Modules
{
    public class DllModule
    {
        public readonly ModuleInfo Info;

        private readonly IModule _moduleObject;

        public List<CommandGroup> Commands;

        public String Summary => _moduleObject.Summary;

        public DllModule(IModule modObject)
        {
            _moduleObject = modObject;
            Info = ModuleInfoFactory.GetModuleInfo(_moduleObject);

        }

    }
}
