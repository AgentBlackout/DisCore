using System;
using System.Collections.Generic;
using System.Reflection;

namespace DisCore.Core.Commands
{
    public class CommandGroup
    {
        private string _commandName;
        private List<MethodInfo> _methods;

        private List<CommandGroup> _subCommandGroups;

        private Func<String> _usageFunc;
        private Func<String> _summaryFunc;

        public String Usage => _usageFunc();
        public String Summary => _summaryFunc();

        public CommandGroup(string cmdName, Func<string> usage, Func<string> summary)
        {
            _commandName = cmdName;
            _methods = new List<MethodInfo>();

            _usageFunc = usage;
            _summaryFunc = summary;
        }

        public void AddMethod(MethodInfo info)
        {
            _methods.Add(info);
        }

        public void RemoveMethod(MethodInfo info)
        {
            _methods.Remove(info);
        }

        public IEnumerable<MethodInfo> GetMethods()
        {
            return _methods;
        }

    }
}
