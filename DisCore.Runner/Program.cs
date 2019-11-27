using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DisCore.Shared.Commands;
using DSharpPlus.Entities;

namespace DisCore.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            Runner core = new Runner();

            Task.WaitAll(new Task[] { core.Run() });

            Console.WriteLine("Exited Main Task");
        }

    }
}
