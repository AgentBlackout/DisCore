using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DisCore.Core;
using DisCore.Core.Commands;
using DisCore.Core.Commands.Timeouts;
using DisCore.Core.Entities.Commands;
using DisCore.Core.Permissions;

namespace ExampleModule
{
    [Timeout(bypassLevel: PermissionLevels.Administrator)]
    [RequiredPermissions(PermissionLevels.Moderator)]
    [Command(typeof(TestCommand), "subtest2")]
    public class SubTestCommand : ICommand
    {
        public async Task<CommandResult> subTest(CommandContext ctx, string details) 
        {
            return await CommandResult.Success();
        }

        public string Usage()
        {
            throw new NotImplementedException();
        }

        public string Summary()
        {
            throw new NotImplementedException();
        }
    }
}
