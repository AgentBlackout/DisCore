using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public static IEnumerable<MethodInfo> GetCommandMethods(Assembly assembly)
        {
            List<MethodInfo> methodInfos = new List<MethodInfo>();
            foreach (Type t in assembly.GetTypes())
            {
                var attrib = (Command)t.GetCustomAttribute(typeof(Command));
                if (attrib != null)
                {
                    CommandGroup cg = new CommandGroup(attrib.Name);

                    foreach (var methodInfo in t.GetMethods())
                    {
                        if (methodInfo.Name == "Summary")
                            continue;
                        if (methodInfo.IsFamily)

                        methodInfos.Add(methodInfo);
                    }
                }
            }

            return methodInfos;
        }
    }
}
