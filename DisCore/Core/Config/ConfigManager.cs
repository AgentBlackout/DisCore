using System;
using System.Threading.Tasks;
using DisCore.Api.Config;
using DSharpPlus.Entities;

namespace DisCore.Core.Config
{
    public class ConfigManager : IConfigManager
    {
        public async Task<IConfig> GetUserConfig(DiscordUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<IConfig> GetChannelConfig(DiscordChannel channel)
        {
            throw new NotImplementedException();
        }

        public async Task<IConfig> GetGuildConfig(DiscordGuild guild)
        {
            throw new NotImplementedException();
        }

        public async Task<IConfig> GetGlobalConfig()
        {
            throw new NotImplementedException();
        }
    }
}
