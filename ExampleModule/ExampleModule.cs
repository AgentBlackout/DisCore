using System;
using System.Threading.Tasks;
using DisCore.Shared.Commands;
using DisCore.Shared.Events;
using DisCore.Shared.Logging;
using DisCore.Shared.Modules;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace ExampleModule
{
    public class ExampleModule : IModule
    {
        private ILogHandler _log;

        public string Name => "ExampleModule";
        public String Version => "0.0.1";
        public String Author => "AgentBlackout";
        public string Summary => "Brief description about the module? I suppose it's a test module.";

        public async Task OnLoad(ILogHandler log)
        {
            _log = log;

            await _log.LogInfo("ExampleModule has loaded and is doing it's own thing!");
        }

        public async Task OnUnload()
        {
            await _log.LogInfo("ExampleModule is unloading, what a shame");
        }

        public async Task OnLoadGuild(DiscordGuild guild)
        {
            await _log.LogInfo("ExampleModule just got a guild");
        }

    }
}
