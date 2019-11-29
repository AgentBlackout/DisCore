using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DisCore.Shared.Config;
using DSharpPlus.Entities;

namespace DisCore.Runner.Config
{
    class MongoConfigManager : IConfigManager
    {
        //TODO

        private readonly string _conStr;

        public MongoConfigManager(string conStr)
        {
            _conStr = conStr;
        }

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
