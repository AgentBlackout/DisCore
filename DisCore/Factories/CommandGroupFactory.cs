using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using DisCore.Core;
using DisCore.Core.Commands;
using DisCore.Core.Commands.Timeouts;
using DisCore.Core.Permissions;

namespace DisCore.Factories
{
    public static class CommandGroupFactory
    {
        public static CommandGroup GetCommandGroup(Type t)
        {
            if (!t.GetInterfaces().Contains(typeof(ICommand)) || t.IsInterface || t.IsAbstract)
                return null;


            var attrib = (CommandAttribute)t.GetCustomAttribute(typeof(CommandAttribute));
            if (attrib == null)
                return null;

            ICommand c = (ICommand)Activator.CreateInstance(t);

            var timeoutAttrib = (TimeoutAttribute)t.GetCustomAttribute(typeof(TimeoutAttribute));
            var permAttribute = (RequiredPermissions)t.GetCustomAttribute(typeof(RequiredPermissions));

            var cmdGroup = new CommandGroup(attrib.Name, c.Usage, c.Summary);

            foreach (var mInfo in t.GetMethods())
            {
                Console.WriteLine(mInfo);
            }

            return cmdGroup;

        }
    }
}
