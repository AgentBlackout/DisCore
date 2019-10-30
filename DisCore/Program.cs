using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DisCore.Commands;
using DisCore.Module;

namespace DisCore
{
    class Program
    {
        public const string modulePath =
            @"E:\Documents\Visual Studio 2019\Projects\DisCore\DisCore\bin\Debug\netcoreapp3.0\modules\";

        public static List<IModule> Modules;
        static void Main(string[] args)
        {
            Modules = new List<IModule>();
            Console.WriteLine("Hello World! Switching to Task Main");

            LoadModules();

            List<Task> initTasks = new List<Task>();
            foreach (var module in Modules)
            {
                initTasks.Add(module.OnLoad(null));
            }

            Task.WaitAll(initTasks.ToArray());

        }

        public static void LoadModules()
        {
            if (!Directory.Exists(modulePath))
            {
                Console.WriteLine("Module path does not exist");
                return;
            }

            var moduleDlls = Directory.GetFiles(modulePath, "*.dll");

            foreach (var modPath in moduleDlls)
            {
                Assembly testAssembly = Assembly.LoadFile(modPath);
                foreach (Type t in testAssembly.GetTypes())
                {
                    Console.WriteLine(t.Name);
                    if (t.GetInterfaces().Contains(typeof(IModule)))
                    {
                        try
                        {
                            IModule module = (IModule) Activator.CreateInstance(t);
                            Console.WriteLine($"Loading Type {t} from {testAssembly.FullName}");
                            Modules.Add(module);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    } else if (t.GetInterfaces().Contains(typeof(ICommand)))
                    {
                        ICommand command = (ICommand) Activator.CreateInstance(t);

                        var methods = t.GetMethods();
                        foreach (var method in methods)
                        {
                            Console.WriteLine(method);
                        }
                    }
                }
            }
        }
    }
}
