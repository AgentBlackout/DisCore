using System.Threading.Tasks;
using DisCore.Core.Commands;

namespace ExampleModule
{
    [Command("test")]
    public class TestCommand : ICommand
    {
        public async Task<CommandResult> Test()
        {
            await Task.Delay(1);
            return CommandResult.Success();
        }

        public async Task<CommandResult> Test(string details)
        {
            await Task.Delay(0);
            if (details.Length < 5)
                return CommandResult.BadArgs("Needs to be longer than 5 chars");
            return CommandResult.Success();
        }

        public string Summary()
        {
            return "Your mum";
        }
    }
}
