using System;

namespace DisCore.Core.Permissions
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequiredPermissions : Attribute
    {

        public readonly PermissionLevels PermLevel;

        public RequiredPermissions(PermissionLevels level)
        {
            PermLevel = level;
        }
    }
}
