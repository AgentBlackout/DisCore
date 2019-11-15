using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DisCore.Api.Commands;
using DisCore.Api.Commands.Timeout;
using DisCore.Api.Logging;
using DisCore.Api.Permissions;
using DisCore.Core.Config.Json;
using DisCore.Core.Entities.Modules;
using DisCore.Core.Loaders;
using DisCore.Core.Loaders.Library;
using DisCore.Core.Loaders.Module;
using DisCore.Core.Logging;
using DisCore.Helpers;
using DSharpPlus;

namespace DisCore.Core
{
    public sealed class DisCoreRunner
    {
        public JsonConfig Config;

        public List<DiscordShardedClient> Shards;

        public IPermissionHandler PermManager;
        public ITimeoutHandler TimeoutHandler;

        public ICommandParser Parser;

        public ILogHandler LogHandler;

        public readonly ModuleLoader ModuleLoader;
        public readonly LibraryLoader LibraryLoader;

        public List<DllModule> Modules => ModuleLoader.GetModules().ToList();

        public DisCoreRunner()
        {

            PermManager = null;
            TimeoutHandler = null;

            LogHandler = new ConsoleLogHandler();

            ModuleLoader = new ModuleLoader("./modules", Config, LogHandler);
            LibraryLoader = new LibraryLoader("./library", LogHandler);
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
            foreach (var fileLoc in dllLocations)
            {
                var result = await LibraryLoader.LoadLibrary(fileLoc);
                if (result == LoadResult.Loaded)
                    await LogHandler.LogInfo($"Loaded {Path.GetFileName(fileLoc)} successfully.");
                else
                    await LogHandler.LogInfo($"Could not load {Path.GetFileName(fileLoc)}.");
            }
        }

        private async Task LoadModules()
        {
            IEnumerable<string> dllLocations = FileHelper.GetDLLs("./modules");
            foreach (var fileLoc in dllLocations)
            {
                var result = await ModuleLoader.LoadModule(fileLoc);
                if (result == LoadResult.Loaded)
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
