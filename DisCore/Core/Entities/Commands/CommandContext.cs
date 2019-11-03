using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DisCore.Core.Commands;
using DSharpPlus.Entities;

namespace DisCore.Core.Entities.Commands
{
    public class CommandContext
    {
        public readonly DiscordMessage Message;

        public readonly DiscordGuild Guild;
        public readonly DiscordChannel Channel;

        public readonly DiscordMember Member;
        public readonly DiscordUser User;

        public readonly String Command;

        public DiscordUser Author => (DiscordUser)Member ?? User;

        public bool IsDM => Channel.IsPrivate;

        public async Task<DiscordMessage> Reply(string content = null, DiscordEmbed embed = null, bool tts = false) => await Channel.SendMessageAsync(content, tts, embed);

        #region Constructors

        public CommandContext(String command, DiscordUser user, DiscordChannel channel, DiscordMessage message)
        {
            Command = command;
            User = user;
            Channel = channel;
            Message = message;
        }

        public CommandContext(String command, DiscordMember member, DiscordChannel channel, DiscordGuild guild, DiscordMessage message)
        {
            Command = command;
            Member = member;
            Channel = channel;
            Guild = guild;
            Message = message;
        }

        #endregion
    }
}
