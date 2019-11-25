﻿using System;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace DisCore.Shared.Config
{
    public class ConfigManager : IConfigManager
    {
        public const string CONFIGS_USERS = "users";
        public const string CONFIGS_GUILDS = "guilds";
        public const string CONFIGS_CHANNELS = "channels";
        public const string CONFIGS_GLOBALS = "globals";

        private IConfig _rootConfig;

        public ConfigManager(IConfig rootConfig)
        {
            _rootConfig = rootConfig;
        }

        public async Task<IConfig> GetUserConfig(DiscordUser user)
        {
            var users = await _rootConfig.Get<IConfig>(CONFIGS_USERS);
            return await users.Get<IConfig>(user.Id.ToString());
        }


        public async Task<IConfig> GetChannelConfig(DiscordChannel channel)
        {
            var channels = await _rootConfig.Get<IConfig>(CONFIGS_CHANNELS);
            return await channels.Get<IConfig>(channel.Id.ToString());
        }

        public async Task<IConfig> GetGuildConfig(DiscordGuild guild)
        {
            var guilds = await _rootConfig.Get<IConfig>(CONFIGS_GUILDS);
            return await guilds.Get<IConfig>(guild.Id.ToString());
        }

        public async Task<IConfig> GetGlobalConfig()
        {
            return await _rootConfig.Get<IConfig>(CONFIGS_GLOBALS);
        }
    }
}
