using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DisCore.Core;

namespace DisCore
{
    class Program
    {
        static void Main(string[] args)
        {
            DisCoreRunner core = new DisCoreRunner();

            Task.WaitAll(new Task[] { core.Run() });

            string inp;
            do
            {
                Console.Write(">");
                inp = Console.ReadLine();

                if (inp == ".info")
                {
                    string indent = "    ";
                    Console.WriteLine(core.Modules.Count + " loaded modules");
                    foreach (var module in core.Modules)
                    {
                        Console.WriteLine(module.Info.Name + " - " + module.Info.Author);
                        foreach (var command in module.Commands)
                        {
                            Console.WriteLine(indent+"!"+command.CommandName);

                            Console.WriteLine(indent+indent+command.GetCommands().Count()+" overloads");
                        }
                    }
                }

            } while (inp != "exit");

        }
    }
}
