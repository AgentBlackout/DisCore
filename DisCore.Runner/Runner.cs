using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DisCore.Runner.Helpers;
using DisCore.Runner.Loaders;
using DisCore.Runner.Loaders.Library;
using DisCore.Runner.Loaders.Module;
using DisCore.Shared.Commands.Parser;
using DisCore.Shared.Commands.Timeout;
using DisCore.Shared.Config.Json;
using DisCore.Shared.Helpers;
using DisCore.Shared.Logging;
using DisCore.Shared.Modules;
using DisCore.Shared.Permissions;
using DSharpPlus;

namespace DisCore.Runner
{
    public sealed class Runner
    {
        public JsonConfig Config;

        public DiscordShardedClient ShardClient;

        public IPermissionHandler PermManager;
        public ITimeoutHandler TimeoutHandler;

        public ICommandParser Parser;

        public ILogHandler LogHandler;

        public readonly ModuleLoader ModuleLoader;
        public readonly LibraryLoader LibraryLoader;

        public List<DllModule> Modules => ModuleLoader.GetModules().ToList();

        public Runner()
        {

            PermManager = null;
            TimeoutHandler = null;

            LogHandler = new ConsoleLogHandler();

            ModuleLoader = new ModuleLoader(LogHandler);
            LibraryLoader = new LibraryLoader(LogHandler);

        }

        public async Task Load()
        {
            Config = (JsonConfig)(await RootConfigHelper.InitOrLoad("./config.json", LogHandler));

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

            await StartShards();

            await Task.Delay(int.MaxValue);

        }

        public async Task StartShards()
        {
            var discordConfig = new DiscordConfiguration()
            {
                AutoReconnect = true,
                ShardCount = await RootConfigHelper.GetShardCount(Config),
                Token = await RootConfigHelper.GetToken(Config)
            };

            ShardClient = new DiscordShardedClient(discordConfig);
            await LogHandler.LogInfo("Starting Shards... Please wait...");
            await ShardClient.StartAsync();
            await LogHandler.LogInfo($"{ShardClient.ShardClients.Count} Shards online");
        }

    }
}
