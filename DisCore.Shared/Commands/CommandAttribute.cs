using System;

namespace DisCore.Shared.Commands
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : Attribute
    {
        public string Name;
        public Type Parent; 

        public CommandAttribute(string name)
        {
            this.Name = name;
        }

        public CommandAttribute(Type t, string name)
        {
            Parent = t;
            Name = name;
        }
    }
}
