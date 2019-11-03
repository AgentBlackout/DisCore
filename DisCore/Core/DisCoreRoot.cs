using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using DisCore.Core.Commands.Timeouts;
using DisCore.Core.Config;
using DisCore.Core.Entities;
using DisCore.Core.Log;
using DisCore.Core.Permissions;
using DisCore.Helpers;
using DSharpPlus;

namespace DisCore.Core
{
    public class DisCoreRoot
    {
        public static DisCoreRoot Singleton { get; private set; }

        public DConfig Config;

        public List<DiscordShardedClient> Shards; 

        public IPermissionManager PermManager;
        public ITimeoutHandler TimeoutHandler;

        public ILogHandler LogHandler;

        private List<Entities.Modules.Module> Modules;

        public DisCoreRoot()
        {
            Singleton = this;

            PermManager = null;
            TimeoutHandler = null;

            Modules = new List<Entities.Modules.Module>();

            Config = new DConfig("./config.json");
            LogHandler = new LogHandler();
        }

        public async Task Load()
        {
            IEnumerable<string> dllLocations = FileHelper.GetDLLs("./modules");
            foreach (var fileLoc in dllLocations)
            {
                Assembly assembly;
                try
                {
                    assembly = Assembly.LoadFile(fileLoc);
                }
                catch (Exception e)
                {
                    await LogHandler.LogWarning($"Failed to load module {Path.GetFileName(fileLoc)} - {e.Message}");
                    continue;
                }

                Type moduleType = AssemblyHelper.GetIModuleType(assembly);

            }
        }

    }
}
