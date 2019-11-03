using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DisCore.Core;
using DisCore.Core.Commands;
using DisCore.Core.Module;

namespace DisCore.Helpers
{
    public static class AssemblyHelper
    {
        public static Type GetIModuleType(Assembly assembly)
        {
            return assembly.GetTypes().FirstOrDefault(item => item.GetInterfaces().Contains(typeof(IModule)));
        }

    }
}
