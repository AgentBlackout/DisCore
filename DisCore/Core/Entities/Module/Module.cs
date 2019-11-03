using System;
using System.Collections.Generic;
using DisCore.Core.Commands;

namespace DisCore.Core.Module
{
    public class Module
    {
        public readonly ModuleInfo Info;

        private readonly IModule _moduleObject;

        private List<ICommand> _commands;

        public String Summary => _moduleObject.Summary;

        public Module(IModule modObject)
        {
            _moduleObject = modObject;
        }

    }
}
