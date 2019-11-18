using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DisCore.Shared.Modules;

namespace DisCore.Runner.Helpers
{
    public static class AssemblyHelper
    {
        public static Type GetIModuleType(Assembly assembly)
        {
            return assembly.GetTypes().FirstOrDefault(item => item.GetInterfaces().Contains(typeof(IModule)));
        }

        public static async Task<Assembly> ReadAndLoad(string location)
        {
            var assemblyBytes = await File.ReadAllBytesAsync(location);
            return Assembly.Load(assemblyBytes);
        }

    }
}
