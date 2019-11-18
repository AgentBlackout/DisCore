using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using DisCore.Shared.Commands;
using DisCore.Shared.Permissions;
using DSharpPlus.Entities;

namespace UtilsModule.Commands
{
    [RequiredPermissions(PermissionLevels.Moderator)]
    [Command("kick")]
    public class CommandKick : ICommand
    {

        public async Task<CommandResult> Kick(CommandContext ctx, ulong id, string reason = "")
        {
            var member = await ctx.Guild.GetMemberAsync(id);
            if (member == null)
            {
                return await CommandResult.BadArgs("Could not find user");
            }

            return await Kick(ctx, member, reason);
        }

        public async Task<CommandResult> Kick(CommandContext ctx, DiscordMember member, string reason = "")
        {
            var author = ctx.Author;
            var fullReason = $"\"{reason}\" - {author.Username}#{author.Discriminator} ({author.Id})";
            await ctx.Guild.RemoveMemberAsync(member, fullReason);
            await ctx.Reply($"Kicked {member.Username}. {reason}");

            return await CommandResult.Success();
        }

        public async Task<CommandResult> Kick(CommandContext ctx, DiscordUser user, string reason = "") =>
            await CommandResult.BadArgs("Cannot kick user not in Guild.");

        public async Task<string> Summary() => await Task.FromResult("Kick a member from the guild");

        public async Task<string> Usage() => await Task.FromResult("!kick [uid / mention]");
    }
}
