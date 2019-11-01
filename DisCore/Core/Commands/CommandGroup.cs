using System;
using System.Collections.Generic;
using System.Reflection;

namespace DisCore.Core.Commands
{
    class CommandGroup
    {
        private string CommandName;
        private List<MethodInfo> Methods;

        private Func<String> usageFunc;
        private Func<String> summaryFunc;

        public String Usage => usageFunc();
        public String Summary => summaryFunc();

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

        public IEnumerable<MethodInfo> GetMethods()
        {
            return Methods;
        }

    }
}
