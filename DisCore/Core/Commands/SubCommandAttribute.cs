using System;
using System.Collections.Generic;
using System.Text;

namespace DisCore.Core.Commands
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SubCommandAttribute : Attribute
    {

        public readonly string Name;

        public SubCommandAttribute(string name)
        {
            Name = name;
        }

    }
}
