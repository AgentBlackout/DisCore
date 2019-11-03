using System;
using System.Collections.Generic;
using System.Text;
using DisCore.Core.Commands;
using DSharpPlus.Entities;

namespace DisCore.Core.Permissions
{
    public class PermissionManager : IPermissionManager
    {
        public PermissionLevels GetPermissionLevel(DiscordMember member, DiscordGuild guild)
        {
            throw new NotImplementedException();
        }

        public PermissionLevels GetPermissionLevel(DiscordUser user)
        {
            throw new NotImplementedException();
        }

        public PermissionLevels GetPermissionOverrides(ICommand cmd, DiscordMember member, DiscordGuild guild)
        {
            throw new NotImplementedException();
        }

        public bool CanDoAction(ICommand cmd, DiscordMember member, DiscordGuild guild)
        {
            throw new NotImplementedException();
        }

        public bool CanDoAction(ICommand cmd, DiscordUser user)
        {
            throw new NotImplementedException();
        }
    }
}
