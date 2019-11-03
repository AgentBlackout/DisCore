using System;
using System.Collections.Generic;
using System.Text;
using DisCore.Core.Commands;
using DSharpPlus.Entities;

namespace DisCore.Core.Permissions
{
    public interface IPermissionManager
    {

        PermissionLevels GetPermissionLevel(DiscordMember member, DiscordGuild guild);
        PermissionLevels GetPermissionLevel(DiscordUser user);

        PermissionLevels GetPermissionOverrides(ICommand cmd, DiscordMember member, DiscordGuild guild);

        bool CanDoAction(ICommand cmd, DiscordMember member, DiscordGuild guild);
        bool CanDoAction(ICommand cmd, DiscordUser user);
    }
}
