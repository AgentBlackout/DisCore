using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using DisCore.Module;

namespace DisCore.Commands
{
    public class CommandManager
    {
        public static void RegisterCommand(MethodInfo method)
        {
            Console.WriteLine($"Registering Command {method.Name} with Params {method.GetParameters().ToString()}");
        }

    }
}
