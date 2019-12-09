using System;
using System.Threading.Tasks;
using DisCore.Shared.Commands;
using DisCore.Shared.Events;
using DisCore.Shared.Modules;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace ExampleModule
{
    public class ExampleModule : IModule
    {
        public string Name => "ExampleModule";
        public String Version => "0.0.1";
        public String Author => "AgentBlackout";
        public string Summary => "Brief description about the module? I suppose it's a test module.";

        public async Task OnLoad(HandlerGroup handlers)
        {
            await Task.Delay(200);
        }

        public async Task OnUnload(HandlerGroup handlers)
        {
            await Task.Delay(200);
        }

        public async Task OnLoadGuild(DiscordGuild guild)
        {

            //TODO
            throw new NotImplementedException();
        }

    }
}
