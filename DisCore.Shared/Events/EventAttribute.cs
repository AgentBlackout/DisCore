using System;
using System.Collections.Generic;
using System.Text;

namespace DisCore.Shared.Events
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EventAttribute : Attribute
    {
        public bool IgnoreCommands;
        public EventAttribute()
    }
}
