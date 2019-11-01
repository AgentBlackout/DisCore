using System;
using System.Reflection;

namespace DisCore.Core.Commands
{
    public class CommandManager
    {
        public static void RegisterCommand(MethodInfo method)
        {
            Console.WriteLine($"Registering Command {method.Name} with Params {method.GetParameters().ToString()}");
        }

    }
}
