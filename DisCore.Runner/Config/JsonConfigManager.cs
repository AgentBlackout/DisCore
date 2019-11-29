using System;
using System.Threading.Tasks;
using System.Xml;
using DisCore.Shared.Config;
using DisCore.Shared.Config.Json;
using DSharpPlus.Entities;
using Newtonsoft.Json.Linq;

namespace DisCore.Runner.Config
{
    public class JsonConfigManager : IConfigManager, IDisposable
    {
        private const string CONFIGS_USERS = "users";
        private const string CONFIGS_GUILDS = "guilds";
        private const string CONFIGS_CHANNELS = "channels";
        private const string CONFIGS_GLOBALS = "globals";

        private readonly IConfig _rootConfig;

        private JsonSubConfig _users = null;
        private JsonSubConfig _guilds = null;
        private JsonSubConfig _channels = null;
        private JsonSubConfig _globals = null;

        public JsonConfigManager(JsonConfig rootConfig)
        {
            _rootConfig = rootConfig;
        }

        public async Task<IConfig> GetUserConfig(DiscordUser user)
        {
            var users = await GetUsersConfig();

            return await GetSubConfig(user.Id.ToString(), users);
        }

        public async Task<IConfig> GetChannelConfig(DiscordChannel channel)
        {
            var channels = await GetChannelsConfig();
            return await GetSubConfig($"{channel.GuildId}#{channel.Id}", channels);
        }

        public async Task<IConfig> GetGuildConfig(DiscordGuild guild)
        {
            var guilds = await GetGuildsConfig();
            return await GetSubConfig(guild.Id.ToString(), guilds);
        }

        public async Task<IConfig> GetGlobalConfig()
        {
            if (_globals != null) return _globals;

            _globals = (JsonSubConfig)(await GetSubConfig(CONFIGS_GLOBALS, _rootConfig));

            return _globals;
        }

        #region Get groups

        private async Task<IConfig> GetUsersConfig()
        {
            if (_users != null) return _users;

            _users = (JsonSubConfig)(await GetSubConfig(CONFIGS_USERS, _rootConfig));

            return _users;
        }

        private async Task<IConfig> GetChannelsConfig()
        {
            if (_channels != null) return _channels;

            _channels = (JsonSubConfig)(await GetSubConfig(CONFIGS_CHANNELS, _rootConfig));

            return _channels;
        }

        private async Task<IConfig> GetGuildsConfig()
        {
            if (_guilds != null) return _guilds;

            _guilds = (JsonSubConfig)(await GetSubConfig(CONFIGS_GUILDS, _rootConfig));

            return _guilds;
        }

        #endregion

        private async Task<IConfig> GetSubConfig(string key, IConfig parent)
        {
            var jObj = await _rootConfig.Get<JObject>(key);
            if (jObj == null)
            {
                jObj = new JObject();
                await parent.Set(key, jObj);
            }


            return new JsonSubConfig(_rootConfig, key, jObj);
        }

        //Save config on close
        public void Dispose()
        {
            _rootConfig.Save().Wait();
        }

    }
}
