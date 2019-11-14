using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DisCore.Core;
using DisCore.Core.Commands;
using DisCore.Factories;
using DisCore.Helpers;

namespace DisCore
{
    class Program
    {
        static void Main(string[] args)
        {
            DisCoreRoot core = new DisCoreRoot();

            Task.WaitAll(new Task[] {core.Run()});
        }
    }
}
