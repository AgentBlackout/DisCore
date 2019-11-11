using System;
using System.Collections.Generic;
using DisCore.Core.Commands;
using DisCore.Core.Module;
using DisCore.Factories;

namespace DisCore.Core.Entities.Modules
{
    public class Module
    {
        public readonly ModuleInfo _info;

        private readonly IModule _moduleObject;

        public List<CommandGroup> Commands;

        public String Summary => _moduleObject.Summary;

        public Module(IModule modObject)
        {
            _moduleObject = modObject;
            _info = ModuleInfoFactory.GetModuleInfo(_moduleObject);

        }

    }
}
