﻿using System;
using System.Threading.Tasks;
using DisCore.Shared.Modules;
using DSharpPlus.Entities;

namespace UtilsModule
{
    public class UtilsModule : IModule
    {
        public string Name => nameof(UtilsModule);
        public string Version => "0.0.1";
        public string Author => "AgentBlackout";
        public string Summary => "General administrative functions for moderators";
        public async Task OnLoad()
        {
            throw new NotImplementedException();
        }

        public async Task OnUnload()
        {
            throw new NotImplementedException();
        }

        public async Task OnLoadGuild(DiscordGuild guild)
        {
            throw new NotImplementedException();
        }
    }
}