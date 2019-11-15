using System;
using System.Threading.Tasks;
using DisCore.Api.Commands;
using DisCore.Api.Commands.Timeout;
using DisCore.Api.Permissions;

namespace ExampleModule
{
    [Timeout(bypassLevel: PermissionLevels.Administrator)]
    [RequiredPermissions(PermissionLevels.Moderator)]
    [Command(typeof(TestCommand), "subtest2")]
    public class SubTestCommand : ICommand
    {
        public async Task<CommandResult> SubTest(CommandContext ctx, string details)
        {
            return await CommandResult.Success();
        }

        public async Task<CommandResult> Usage(CommandContext ctx)
        {
            throw new NotImplementedException();
        }

        public async Task<CommandResult> Summary(CommandContext ctx)
        {
            throw new NotImplementedException();
        }
    }
}
