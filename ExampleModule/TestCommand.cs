using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DisCore.Commands;

namespace ExampleModule
{
    [Command("test")]
    public class TestCommand : ICommand
    {
        public async Task<CommandResult> Test()
        {
            await Task.Delay(1);
            return CommandResult.Success;
        }

        public async Task<CommandResult> Test(string details)
        {
            await Task.Delay(0);
            return CommandResult.Failure;
        }

        public string Summary()
        {
            return "Your mum";
        }
    }
}
