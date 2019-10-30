using System;
using System.Collections.Generic;
using System.Text;

namespace DisCore.Commands
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Command : Attribute
    {
        private string Name;

        public Command(string name)
        {
            this.Name = name;
        }
    }
}
