using System;
using System.Collections.Generic;
using System.Text;

namespace DisCore.Shared.Events
{
    public enum DiscordEvent
    {
        MessageCreated,
        MessageDeleted,
        ReactionAdded,
        ReactionDeleted,
        GuildMemberJoined,
        GuildMemberLeft
    }
}
