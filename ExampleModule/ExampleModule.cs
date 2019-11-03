using System;
using System.Threading.Tasks;
using DisCore.Core;
using DisCore.Core.Module;

namespace ExampleModule
{
    public class ExampleModule : IModule
    {
        public string Name => "ExampleModule";
        public String Version => "0.0.1";
        public String Author => "AgentBlackout";
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
