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
        public async Task Test()
        {
            await Task.Delay(1);
        }

        public async Task Test(string details)
        {
            await Task.Delay(0);
        }
    }
}
