using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DisCore.Commands
{
    class CommandGroup
    {
        private string CommandName;
        private List<MethodInfo> Methods;

        public CommandGroup(string cmdName)
        {
            CommandName = cmdName;
            Methods = new List<MethodInfo>();
        }

        public void AddMethod(MethodInfo info)
        {
            Methods.Add(info);
        }

        public void RemoveMethod(MethodInfo info)
        {
            Methods.Remove(info);
        }

    }
}
