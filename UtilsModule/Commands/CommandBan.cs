using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using DisCore.Shared.Commands;
using DisCore.Shared.Permissions;
using DSharpPlus.Entities;

namespace UtilsModule.Commands
{
    [RequiredPermissions(PermissionLevels.Moderator)]
    [Command("ban")]
    public class CommandBan : ICommand
    {

        public async Task<CommandResult> Ban(CommandContext ctx, ulong id, string reason = "")
        {
            var member = await ctx.Guild.GetMemberAsync(id);
            if (member == null)
            {
                return CommandResult.BadArgs("Could not find user");
            }

            return await Ban(ctx, member, reason);
        }

        public async Task<CommandResult> Ban(CommandContext ctx, DiscordMember member, string reason = "")
        {
            var author = ctx.Author;
            var fullReason = $"\"{reason}\" - {author.Username}#{author.Discriminator} ({author.Id})";
            await ctx.Guild.BanMemberAsync(member, 0, fullReason);
            await ctx.Reply($"Banned {member.Username}. {reason}");

            return CommandResult.Success();
        }

        public async Task<CommandResult> Ban(CommandContext ctx, DiscordUser user, string reason = "") =>
            CommandResult.BadArgs("Cannot ban user not in Guild.");

        public async Task<string> Summary() => await Task.FromResult("Ban a member from the guild");

        public async Task<string> Usage() => await Task.FromResult("!ban [uid / mention]");
    }
}
