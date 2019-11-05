using System;
using System.Collections.Generic;
using DisCore.Core.Commands;
using DisCore.Core.Module;

namespace DisCore.Core.Entities.Modules
{
    public class Module
    {
        public readonly ModuleInfo Info;

        private readonly IModule _moduleObject;

        public List<CommandGroup> Commands;

        public String Summary => _moduleObject.Summary;

        public Module(IModule modObject)
        {
            _moduleObject = modObject;
        }

    }
}
