using System;

namespace DisCore.Core.Commands
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Command : Attribute
    {
        public string Name;

        public Command(string name)
        {
            this.Name = name;
        }
    }
}
