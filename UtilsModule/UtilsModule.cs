using System;
using System.Threading.Tasks;
using DisCore.Shared.Commands;
using DisCore.Shared.Events;
using DisCore.Shared.Modules;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace UtilsModule
{
    public class UtilsModule : IModule
    {
        public string Name => nameof(UtilsModule);
        public string Version => "0.0.1";
        public string Author => "AgentBlackout";
        public string Summary => "General administrative functions for moderators";
        public async Task OnLoad(HandlerGroup handlers)
        {
            //TODO
            throw new NotImplementedException();
        }

        public async Task OnUnload(HandlerGroup handlers)
        {
            //TODO
            throw new NotImplementedException();
        }

        public async Task OnLoadGuild(DiscordGuild guild)
        {
            //TODO
            throw new NotImplementedException();
        }

    }
}
