using System;
using System.Threading.Tasks;
using DisCore.Api.Commands;
using DisCore.Api.Commands.Timeout;
using DisCore.Api.Permissions;
using DisCore.Helpers;

namespace ExampleModule
{
    [Timeout(bypassLevel: PermissionLevels.Administrator)]
    [RequiredPermissions(PermissionLevels.Moderator)]
    [Command("test")]
    public class TestCommand : ICommand
    {
        //!test
        public async Task<CommandResult> Test(CommandContext cmd)
        {

            if (cmd.Instance.PermissionHandler.GetPermissionLevel(cmd.Member, cmd.Guild) == PermissionLevels.Moderator)
            {
                await cmd.Reply("You are a moderator");
            }
            else
            {
                await cmd.Reply("You are an administrator");
            }

            //Command is going to succeed, so set a timeout at the end
            Timeout.SetTimeout(TimeSpan.FromMinutes(1));

            return await CommandResult.Success();
        }
        //!test hello world
        public async Task<CommandResult> Test(CommandContext cmd, string details)
        {
            if (details.Length < 5)
                return await CommandResult.BadArgs("Needs to be longer than 5 chars");

            await cmd.Reply("Hello " + MessageHelper.StipMentions(details));

            //Timeout changes
            Timeout.SetTimeout(TimeSpan.FromSeconds(30));

            return await CommandResult.Success();
        }

        //!test subtest
        [Timeout(PermissionLevels.Creator)]
        [RequiredPermissions(PermissionLevels.Administrator)]
        public async Task<CommandResult> Subtest(CommandContext ctx)
        {
            await ctx.Reply("This is an admin-only command, which has a timeout, for no logical reason.");
            return await CommandResult.Success();
        }

        //This will be triggered by returning CommandResult.BadArgs()
        public async Task<CommandResult> Usage(CommandContext ctx)
        {
            await ctx.Reply("Usage: \n"+
                            "!test\n" +
                            "!test \"string\"");
            return await CommandResult.Success();
        }

        public async Task<CommandResult> Summary(CommandContext ctx)
        {
            await ctx.Reply("Summary of example command");
            await Usage(ctx); //If you want to print the usage with the summary, personal choice. 
            return await CommandResult.Success();
        }
    }
}
