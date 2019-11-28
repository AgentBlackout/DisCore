using System;
using System.Collections.Generic;
using System.Text;

namespace DisCore.Shared.Events
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ListenerAttribute : Attribute
    {
        public bool IgnoreCommands;

        public ListenerAttribute(bool ignoreCommands = true)
        {
            IgnoreCommands = ignoreCommands;
        }
    }
}
