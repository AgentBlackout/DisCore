using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DisCore.Core;
using DisCore.Core.Commands;
using DisCore.Core.Module;
using DisCore.Factories;
using DisCore.Helpers;

namespace DisCore
{
    class Program
    {
        public const string ModulePath = @".\modules\";

        public static List<IModule> Modules;
        static void Main(string[] args)
        {
            DisCoreRoot core = new DisCoreRoot();


            Task.WhenAll(new Task[] { core.Load(), core.Run()});
        }
    }
}
