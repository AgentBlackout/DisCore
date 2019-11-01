using System;
using System.Threading.Tasks;
using DisCore.Core;
using DisCore.Core.Module;

namespace ExampleModule
{
    public class ExampleModule : IModule
    {
        public new string Name => "ExampleModule";
        public new String Version => "0.0.1";
        public new String Author => "AgentBlackout";
        public string Summary => "Brief description about the module? I suppose it's a test module.";

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
