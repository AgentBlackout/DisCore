
using System;
using System.Linq;
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
        public readonly Type Parameter;

        public EventMethod(MethodInfo method, ListenerAttribute attribute, Func<DiscordEventArgs, Task> func)
        {
            Method = method;

            Parameter = method.GetParameters().First().ParameterType;

            ListenerAttribute = attribute;
            Func = func;
        }

    }
}
