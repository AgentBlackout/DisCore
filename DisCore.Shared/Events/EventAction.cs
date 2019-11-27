using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using DSharpPlus.EventArgs;

namespace DisCore.Shared.Events
{
    public class EventAction
    {
        public readonly Action<DiscordEventArgs> Action;
        public readonly MethodInfo Method;
        public readonly ListenerAttribute Attribute;

        public EventAction(Action<DiscordEventArgs> action, MethodInfo method, ListenerAttribute attribute)
        {
            Action = action;
            Method = method;
            Attribute = attribute;
        }
    }
}
