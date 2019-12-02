using System;
using System.Threading.Tasks;
using DisCore.Shared.Commands;
using DisCore.Shared.Commands.Timeout;
using DisCore.Shared.Helpers;
using DisCore.Shared.Permissions;

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

            //CommandOverload is going to succeed, so set a timeout at the end
            Timeout.SetTimeout(TimeSpan.FromMinutes(1));

            return CommandResult.Success();
        }
        //!test hello world
        public async Task<CommandResult> Test(CommandContext cmd, string details)
        {
            if (details.Length < 5)
                return CommandResult.BadArgs("Needs to be longer than 5 chars");

            await cmd.Reply("Hello " + MessageHelper.StipMentions(details));

            //Timeout changes
            Timeout.SetTimeout(TimeSpan.FromSeconds(30));

            return CommandResult.Success();
        }

        //This will be triggered by returning CommandResult.BadArgs()
        public async Task<string> Usage()
        {
            return "Usage: \n"+
                            "!test\n" +
                            "!test \"string\"";
        }

        //Triggered on help
        public async Task<string> Summary()
        {
            return "This command does example things.\nGenerally, used for examples.";
        }
    }
}
