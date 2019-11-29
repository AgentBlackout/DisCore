using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DisCore.Shared.Config;
using DisCore.Shared.Config.Json;
using DisCore.Shared.Logging;
using Newtonsoft.Json.Linq;

namespace DisCore.Runner.Helpers
{
    public class RootConfigHelper
    {
        private const string TOKEN_KEY = "token";
        private const string OWNERS_KEY = "owners";
        private const string SHARD_COUNT_KEY = "shardCount";
        private const string MONGO_CONFIG_KEY = "mongoConStr";
        private const string USE_MONGO_KEY = "shouldUseMongo";

        public static async Task<IConfig> InitOrLoad(string fileLoc, ILogHandler log)
        {
            var exists = File.Exists(fileLoc);
            var conf = new JsonConfig(fileLoc);
            await log.LogDebug($"Loading config file from \"{fileLoc}\"");

            if (!exists)
            {
                await log.LogInfo(
                    "Config file does not exist, creating. You'll want to setup all of the values in there.");
                await InitConfig(conf);
                throw new Exception("Please fill out the config then restart.");
            }

            await conf.Load();

            return conf;

        }

        public static async Task InitConfig(IConfig config)
        {
            await config.Set("token", "abc-123-abc");
            await config.Set("owners", new long[] { 123, 1441, 999 });
            await config.Set("shardCount", 1);
            
        }

        public static async Task InitConfigManager(IConfigManager configManager)
        {
            var global = await configManager.GetGlobalConfig();
            await global.Set("prefix", "!");
            await global.Save();
        }


        public static async Task<string> GetToken(IConfig config)
        {
            return await config.Get<string>(TOKEN_KEY);
        }

        public static async Task<IEnumerable<ulong>> GetCreatorIDs(IConfig config)
        {
            return await config.Get<ulong[]>(OWNERS_KEY);
        }

        public static async Task<int> GetShardCount(IConfig config)
        {
            return await config.Get<int>(SHARD_COUNT_KEY);
        }

        public static async Task<string> GetMongoDetails(IConfig config)
        {
            return await config.Get<string>(MONGO_CONFIG_KEY);
        }

        public static async Task<bool> UseMongo(IConfig config)
        {
            return await config.Get<bool>(USE_MONGO_KEY);
        }
    }
}
