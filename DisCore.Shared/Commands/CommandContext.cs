using System;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace DisCore.Shared.Commands
{

    public class CommandContext
    {
        public readonly DiscordMessage Message;

        public readonly DiscordGuild Guild;
        public readonly DiscordChannel Channel;

        public readonly DiscordMember Member;
        public readonly DiscordUser User;

        public readonly String Command;

        public HandlerGroup Handlers;

        public DiscordUser Author => (DiscordUser)Member ?? User;

        public bool IsDm => Channel.IsPrivate;

        public async Task<DiscordMessage> Reply(string content = null, DiscordEmbed embed = null, bool tts = false) => await Channel.SendMessageAsync(content, tts, embed);

        #region Constructors

        public CommandContext(String command, HandlerGroup handlers, DiscordUser user, DiscordChannel channel, DiscordMessage message)
        {
            Command = command;
            User = user;
            Channel = channel;
            Message = message;

            Handlers  = handlers;
        }

        public CommandContext(String command, HandlerGroup handlers, DiscordMember member, DiscordChannel channel, DiscordGuild guild, DiscordMessage message)
        {
            Command = command;
            Member = member;
            Channel = channel;
            Guild = guild;
            Message = message;

            Handlers = handlers;
        }

        #endregion
    }
}
