using System;
using System.Threading.Tasks;
using DisCore.Shared.Commands;
using DisCore.Shared.Events;
using DisCore.Shared.Logging;
using DisCore.Shared.Modules;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace UtilsModule
{
    public class UtilsModule : IModule
    {
        private ILogHandler _log;

        public string Name => nameof(UtilsModule);
        public string Version => "0.0.1";
        public string Author => "AgentBlackout";
        public string Summary => "General administrative functions for moderators";
        public async Task OnLoad(ILogHandler log)
        {
            _log = log;


            await _log.LogInfo("Utils starting");
        }

        public async Task OnUnload()
        {
            await _log.LogInfo("Utils stopping");

        }

        public async Task OnLoadGuild(DiscordGuild guild)
        {
            await _log.LogInfo($"Utils got guild {guild.Name}");

        }

    }
}
