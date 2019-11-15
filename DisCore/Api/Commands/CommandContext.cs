using System;
using System.Threading.Tasks;
using DisCore.Api.Commands.Timeout;
using DisCore.Api.Logging;
using DisCore.Api.Permissions;
using DSharpPlus.Entities;

namespace DisCore.Api.Commands
{
    public struct RootGroup
    {
        public readonly ITimeoutHandler TimeoutHandler;
        public readonly IPermissionHandler PermissionHandler;
        public readonly ILogHandler LogHandler;

        public RootGroup(ITimeoutHandler timeoutHandler, IPermissionHandler permissionHandler, ILogHandler logHandler)
        {
            TimeoutHandler = timeoutHandler ?? throw new ArgumentNullException(nameof(timeoutHandler));
            PermissionHandler = permissionHandler ?? throw new ArgumentNullException(nameof(permissionHandler));
            LogHandler = logHandler ?? throw new ArgumentNullException(nameof(logHandler));
        }
    }

    public class CommandContext
    {
        public readonly DiscordMessage Message;

        public readonly DiscordGuild Guild;
        public readonly DiscordChannel Channel;

        public readonly DiscordMember Member;
        public readonly DiscordUser User;

        public readonly String Command;

        public RootGroup Instance;

        public DiscordUser Author => (DiscordUser)Member ?? User;

        public bool IsDm => Channel.IsPrivate;

        public async Task<DiscordMessage> Reply(string content = null, DiscordEmbed embed = null, bool tts = false) => await Channel.SendMessageAsync(content, tts, embed);

        #region Constructors

        public CommandContext(String command, RootGroup instance, DiscordUser user, DiscordChannel channel, DiscordMessage message)
        {
            Command = command;
            User = user;
            Channel = channel;
            Message = message;

            Instance = instance;
        }

        public CommandContext(String command, RootGroup instance, DiscordMember member, DiscordChannel channel, DiscordGuild guild, DiscordMessage message)
        {
            Command = command;
            Member = member;
            Channel = channel;
            Guild = guild;
            Message = message;

            Instance = instance;
        }

        #endregion
    }
}
