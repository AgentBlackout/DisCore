using System;

namespace DisCore.Shared.Commands.Parser
{
    public struct CommandParameter
    {
        public Type Type;
        public Object Object;

        public CommandParameter(Type t, Object o)
        {
            Type = t;
            Object = o;
        }

    }
}
