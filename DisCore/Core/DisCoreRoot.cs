using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DisCore.Core.Commands;
using DisCore.Core.Commands.Parser;
using DisCore.Core.Commands.Timeouts;
using DisCore.Core.Config;
using DisCore.Core.Entities;
using DisCore.Core.Logging;
using DisCore.Core.Module;
using DisCore.Core.Permissions;
using DisCore.Factories;
using DisCore.Helpers;
using DSharpPlus;

namespace DisCore.Core
{
    public sealed class DisCoreRoot
    {
        public static DisCoreRoot Singleton { get; private set; }

        public DConfig Config;

        public List<DiscordShardedClient> Shards;

        public IPermissionManager PermManager;
        public ITimeoutHandler TimeoutHandler;

        public ICommandParser Parser;

        public ILogHandler LogHandler;

        public List<Entities.Modules.Module> Modules => ModuleLoader.GetModules;

        public readonly ModuleLoader ModuleLoader;

        public DisCoreRoot()
        {
            Singleton = this;

            PermManager = null;
            TimeoutHandler = null;

            Config = new DConfig("./config.json");
            LogHandler = new LogHandler();

            ModuleLoader = new ModuleLoader(LogHandler);
        }

        public async Task Load()
        {
            var helper = new RootConfigHelper(Config);
            await Config.Load();
            var longs = await helper.GetCreatorIDs();
            await LoadLibraries();
            await LoadModules();
        }

        private async Task LoadLibraries()
        {
            int loaded = 0;
            IEnumerable<string> dllLocations = FileHelper.GetDLLs("./libraries");
            foreach (var fileLoc in dllLocations)
            {
                var fileName = Path.GetFileName(fileLoc);

                await LogHandler.LogDebug($"Loading library {fileName}");

                Assembly assembly;
                try
                {
                    assembly = Assembly.LoadFile(fileLoc);
                }
                catch (Exception e)
                {
                    await LogHandler.LogWarning(
                        $"Failed to load library {fileName} - {e.Message}");
                    return;
                }

                loaded++;
            }

            await LogHandler.LogInfo($"Loaded {loaded} out of {dllLocations.Count()} libraries");
        }

        private async Task LoadModules()
        {
            IEnumerable<string> dllLocations = FileHelper.GetDLLs("./modules");
            foreach (var fileLoc in dllLocations)
            {
                bool result = await ModuleLoader.LoadModule(fileLoc);
                if (result)
                    await LogHandler.LogInfo($"Loaded {Path.GetFileName(fileLoc)} successfully.");
                else
                    await LogHandler.LogInfo($"Could not load {Path.GetFileName(fileLoc)}.");
            }
        }

       

        public async Task Run()
        {
            await Load();
            
        }

    }
}
