using System;
using System.Collections.Generic;
using DisCore.Shared.Commands;
using DisCore.Shared.Factories;

namespace DisCore.Shared.Modules
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
