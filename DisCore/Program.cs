using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DisCore.Commands;
using DisCore.Helper;
using DisCore.Module;

namespace DisCore
{
    class Program
    {
        public const string ModulePath = @".\modules\";

        public static List<IModule> Modules;
        static void Main(string[] args)
        {
            Modules = new List<IModule>();
            Console.WriteLine("Hello World! Switching to Task Main");

            List<string> files = FileHelper.GetDLLs(ModulePath).ToList();
            foreach (var mod in files)
            {
                Assembly assembly = Assembly.LoadFile(mod);
                IModule rootModule = (IModule)Activator.CreateInstance(AssemblyHelper.GetIModuleType(assembly));

                MethodInfo[] methods = AssemblyHelper.GetCommandMethods(assembly).ToArray();

            }
            
        }
    }
}
