using System;

namespace DisCore.Core.Commands.Parser
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
