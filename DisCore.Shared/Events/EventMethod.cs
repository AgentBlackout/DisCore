
using System;
using System.Reflection;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;

namespace DisCore.Shared.Events
{
    public class EventMethod
    {
        public readonly ListenerAttribute ListenerAttribute;
        public readonly MethodInfo Method;
        public readonly Func<DiscordEventArgs, Task> Func;

        public EventMethod(MethodInfo method, ListenerAttribute attribute, Func<DiscordEventArgs, Task> func)
        {
            Method = method;
            ListenerAttribute = attribute;
            Func = func;
        }

    }
}
