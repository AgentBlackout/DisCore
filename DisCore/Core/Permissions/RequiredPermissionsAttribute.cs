using System;

namespace DisCore.Core.Permissions
{
    [AttributeUsage(AttributeTargets.Class)]
    class RequiredPermissions : Attribute
    {

        public readonly PermissionLevels PermLevel;

        public RequiredPermissions(PermissionLevels level)
        {
            PermLevel = level;
        }
    }
}
