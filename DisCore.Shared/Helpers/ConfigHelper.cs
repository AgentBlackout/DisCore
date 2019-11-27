﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DisCore.Shared.Config;
using DSharpPlus.Entities;

namespace DisCore.Shared.Helpers
{
    public static class ConfigHelper
    {
        public const string PREFIX_KEY = "prefix";
        public static async Task<string> GetGlobalPrefix(IConfigManager manager)
        {
            var globalConfig = await manager.GetGlobalConfig();
            return await globalConfig.Get<string>(PREFIX_KEY);
        }

        public static async Task<string> GetGuildPrefix(IConfigManager manager, DiscordGuild guild)
        {
            var guildConfig = await manager.GetGuildConfig(guild);
            return await guildConfig.Get<string>(PREFIX_KEY);
        }
    }
}
