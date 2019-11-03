using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DisCore.Core.Commands;
using DisCore.Core.Commands.Timeouts;
using DisCore.Core.Permissions;
using DisCore.Core;
using DisCore.Core.Entities.Commands;

namespace ExampleModule
{
    [Timeout(bypassLevel: PermissionLevels.Administrator)]
    [RequiredPermissions(PermissionLevels.Moderator)]
    [Command("test")]
    public class TestCommand : ICommand
    {
        private readonly DisCoreRoot _disCore = DisCoreRoot.Singleton;

        //!test
        public async Task<CommandResult> Test(CommandContext cmd)
        {

            if (DisCoreRoot.Singleton.PermManager.GetPermissionLevel(cmd.Member, cmd.Guild) == PermissionLevels.Moderator)
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

            //Timeout changes
            Timeout.SetTimeout(TimeSpan.FromSeconds(30));

            return await CommandResult.Success();
        }

        //!test subtest
        [Timeout(PermissionLevels.Creator)]
        [RequiredPermissions(PermissionLevels.Administrator)]
        [SubCommand("subtest1")]
        public async Task<CommandResult> Subtest()
        {
            return await CommandResult.Success();
        }

        public string Usage()
        {
            throw new System.NotImplementedException();
        }

        public string Summary()
        {
            throw new System.NotImplementedException();
        }
    }
}
