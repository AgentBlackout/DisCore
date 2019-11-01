using System;
using System.Collections.Generic;
using DisCore.Core.Commands;

namespace DisCore.Core.Module
{
    public class Module
    {
        public readonly ModuleInfo Info;

        private IModule moduleObject;

        private List<CommandGroup> Commands;

        public String Summary => moduleObject.Summary;

        public Module(IModule modObject)
        {
            
        }

    }
}
