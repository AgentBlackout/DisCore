using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DisCore.Core.Config;
using DisCore.Core.Config.Json;

namespace DisCore.Helpers
{
    public class RootConfigHelper
    {
        public const string Token = "token";
        public const string Owners = "owners";

        private readonly IConfig _conf;

        public static async Task<JsonConfig> LoadOrInit(string fileLoc)
        {
            var conf = new RootConfigHelper(new JsonConfig(fileLoc));
            if (!File.Exists(fileLoc))
                await conf.InitConfig();
            await conf._conf.Load();
            return (JsonConfig)conf._conf;
        }

        public RootConfigHelper(IConfig config)
        {
            _conf = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task<string> GetToken()
        {
            return await _conf.Get<string>(Token);
        }

        public async Task<IEnumerable<ulong>> GetCreatorIDs()
        {
            return await _conf.Get<ulong[]>(Owners);
        }

        public async Task InitConfig()
        {
            await _conf.Set("token", "abc-123-abc");
            await _conf.Set("owners", new long[] { 123, 1441, 999 });

            await _conf.Save();
        }
    }
}
