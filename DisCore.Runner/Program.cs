using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DisCore.Shared.Commands;

namespace DisCore.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            Runner core = new Runner();

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
                        PrintSubcommandsRecursive(module.Commands);
                    }
                }

            } while (inp != "exit");

        }


        public static void PrintSubcommandsRecursive(IEnumerable<CommandGroup> commands)
        {
            string indent = "    ";
            foreach (var commandGroup in commands)
            {
                Console.WriteLine(indent + "!" + commandGroup.CommandName);

                foreach (var overload in commandGroup.GetOverloads())
                {
                    var method = overload.GetMethod();
                    Console.WriteLine(indent + indent + "!" + method.Name + " " + String.Join(" ", method.GetParameters().Select(item => item.ParameterType.Name)));
                }

                PrintSubcommandsRecursive(commandGroup.GetSubGroups());
            }
        }
    }
}
