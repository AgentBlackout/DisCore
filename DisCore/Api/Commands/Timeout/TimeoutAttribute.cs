using System;
using DisCore.Api.Permissions;

namespace DisCore.Api.Commands.Timeout
{
    public enum TimeoutScope
    {
        User,
        Channel,
        Guild
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class TimeoutAttribute : Attribute
    {
        public readonly PermissionLevels BypassLevel;
        public readonly TimeoutScope Scope;

        public TimeoutAttribute(PermissionLevels bypassLevel = PermissionLevels.Administrator, TimeoutScope scope = TimeoutScope.User)
        {
            BypassLevel = bypassLevel;
            Scope = scope;
        }
    }
}
