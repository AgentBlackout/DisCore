using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using DisCore.Core;
using DisCore.Core.Commands;

namespace DisCore.Factories
{
    public static class CommandGroupFactory
    {
        public static CommandGroup GetCommandGroup(DisCoreRoot core, Type t)
        {
            if (t.IsSubclassOf(typeof(Command)) && t != typeof(Command))
            {


                var attrib = (CommandAttribute)t.GetCustomAttribute(typeof(CommandAttribute));
                if (attrib == null)
                    return null;

                Command c = (Command)Activator.CreateInstance(t, core);

                var cmdGroup = new CommandGroup(attrib.Name, c.Usage, c.Summary);

                foreach (var mInfo in t.GetMethods())
                {
                    Console.WriteLine(mInfo);
                }

                return cmdGroup;
            }

            return null;
        }
    }
}
