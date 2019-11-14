using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DisCore.Core.Commands;
using DisCore.Core.Commands.Parser;
using DisCore.Core.Commands.Timeouts;
using DisCore.Core.Config;
using DisCore.Core.Config.Json;
using DisCore.Core.Entities;
using DisCore.Core.Entities.Modules;
using DisCore.Core.Loaders.Library;
using DisCore.Core.Loaders.Module;
using DisCore.Core.Logging;
using DisCore.Core.Permissions;
using DisCore.Factories;
using DisCore.Helpers;
using DSharpPlus;

namespace DisCore.Core
{
    public sealed class DisCoreRoot
    {
        public static DisCoreRoot Singleton { get; private set; }

        public JsonConfig Config;

        public List<DiscordShardedClient> Shards;

        public IPermissionManager PermManager;
        public ITimeoutHandler TimeoutHandler;

        public ICommandParser Parser;

        public ILogHandler LogHandler;

        public readonly ModuleLoader ModuleLoader;
        public readonly LibraryLoader LibraryLoader;

        public List<DllModule> Modules => ModuleLoader.GetModules().ToList();

        public DisCoreRoot()
        {
            Singleton = this;

            PermManager = null;
            TimeoutHandler = null;

            LogHandler = new LogHandler();

            ModuleLoader = new ModuleLoader("./modules",Config, LogHandler);
        }

        public async Task CheckConfig()
        {
            var rootHelper = new RootConfigHelper(Config);

            string token = await rootHelper.GetToken();
        }

        public async Task Load()
        {
            Config = await RootConfigHelper.LoadOrInit("./config.json");

            await LoadLibraries();
            await LoadModules();
        }

        private async Task LoadLibraries()
        {
            IEnumerable<string> dllLocations = FileHelper.GetDLLs("./libraries");
            
        }

        private async Task LoadModules()
        {
            IEnumerable<string> dllLocations = FileHelper.GetDLLs("./modules");
            foreach (var fileLoc in dllLocations)
            {
                var (result, reason) = await ModuleLoader.LoadModule(fileLoc);
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
