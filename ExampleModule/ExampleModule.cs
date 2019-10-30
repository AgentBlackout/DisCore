using System;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using DisCore;
using DisCore.Commands;
using DisCore.Module;

namespace ExampleModule
{
    public class ExampleModule : IModule
    {
        public new string Name => "ExampleModule";
        public new String Version => "0.0.1";
        public new String Author => "AgentBlackout";

        public async Task OnLoad(CorePlaceholder placeholder)
        {
            await Task.Delay(200);
        }


        public async Task OnUnload()
        {
            await Task.Delay(200);
        }

    }
}
