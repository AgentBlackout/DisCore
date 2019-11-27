using System;
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
using DisCore.Shared.Events;
using DisCore.Shared.Helpers;
using DisCore.Shared.Logging;
using DisCore.Shared.Modules;
using DisCore.Shared.Permissions;
using DSharpPlus;
using EventHandler = DisCore.Runner.Events.EventHandler;
namespace DisCore.Runner
{
    public sealed class Runner
    {
        public JsonConfig Config;

        public DiscordShardedClient ShardClient;

        public IPermissionHandler PermissionHandler = null;
        public ITimeoutHandler TimeoutHandler = null;
        public IEventHandler EventHandler = null;

        public ICommandParser Parser = null;

        public ILogHandler LogHandler = null;

        public readonly ModuleLoader ModuleLoader;
        public readonly LibraryLoader LibraryLoader;

        public List<DllModule> Modules => ModuleLoader.GetModules().ToList();

        public Runner()
        {

            LogHandler = new ConsoleLogHandler();

            EventHandler = new EventHandler(LogHandler);
            ModuleLoader = new ModuleLoader(EventHandler, LogHandler);
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
                var (result, module) = await ModuleLoader.LoadModule(fileLoc);
                if (result != LoadResult.Loaded)
                {
                    await LogHandler.LogInfo($"Could not load {Path.GetFileName(fileLoc)}.");
                    continue;
                }

                TrySet<IPermissionHandler>(ref PermissionHandler, module.PermissionHandler, async () => await LogHandler.LogWarning($"Module {module.Info.Name} is trying to replace IPermissionHandler but one is already set."));
                TrySet<ITimeoutHandler>(ref TimeoutHandler, module.TimeoutHandler, async () => await LogHandler.LogWarning($"Module {module.Info.Name} is trying to replace ITimeoutHandler but one is already set."));
                TrySet<IEventHandler>(ref EventHandler, module.EventHandler, async () => await LogHandler.LogWarning($"Module {module.Info.Name} is trying to replace IEventHandler but one is already set."));

            }
        }



        public async Task Run()
        {
            await Load();

            await StartShards();

            await EventHandler.SetupShardClient(ShardClient);

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

        /// <summary>
        /// Attempt to assign the variable. Runs errorAction if variable a is already defined.
        /// </summary>
        /// <typeparam name="T">Type of variable A</typeparam>
        /// <param name="a">Reference to assign to</param>
        /// <param name="b">Value to assign</param>
        /// <param name="errorAction">Called when variable is already assigned</param>
        private static void TrySet<T>(ref T a, T b, Func<Task> errorAction)
        {
            if (b == null)
                return;

            if (a == null)
            {
                a = b;
            }
            else
            {
                errorAction().Wait();
            }

        }

    }
}
