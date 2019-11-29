using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DisCore.Runner.Config;
using DisCore.Runner.Helpers;
using DisCore.Runner.Loaders;
using DisCore.Runner.Loaders.Library;
using DisCore.Runner.Loaders.Module;
using DisCore.Runner.Parser;
using DisCore.Shared.Commands.Parser;
using DisCore.Shared.Commands.Timeout;
using DisCore.Shared.Config;
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
        public JsonConfig RunnerConfig;
        public IConfigManager ConfigManager;

        public DiscordShardedClient ShardClient;

        public readonly IEventHandler EventHandler;

        public IPermissionHandler PermissionHandler = null;
        public ITimeoutHandler TimeoutHandler = null;


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

        private async Task Load()
        {
            RunnerConfig = (JsonConfig)(await RootConfigHelper.InitOrLoad("./config.json", LogHandler));

            await LoadLibraries();
            await LoadModules();

            await LogHandler.LogInfo($"Loaded {ModuleLoader.GetModules().Count()} modules");

            if (await RootConfigHelper.UseMongo(RunnerConfig))
            {
                var mongoDetails = await RootConfigHelper.GetMongoDetails(RunnerConfig);
                ConfigManager = new MongoConfigManager(mongoDetails);
            }
            else
            {
                ConfigManager = new JsonConfigManager(RunnerConfig);
            }

            var global = await ConfigManager.GetGlobalConfig();
            var prefix = await global.Get<string>("prefix");
            if (prefix == null)
                await RootConfigHelper.InitConfigManager(ConfigManager);

            if (Parser == null)
            {
                await LogHandler.LogInfo("CommandParser not set, using default");
                Parser = new CommandParser(ConfigManager, ModuleLoader);
            }


            EventHandler.SetCommandParser(Parser);
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
                    await LogHandler.LogWarning($"Could not load {Path.GetFileName(fileLoc)}.");
            }
        }

        private async Task LoadModules()
        {
            //TODO: Load order and disabled modules.

            IEnumerable<string> dllLocations = FileHelper.GetDLLs("./modules");
            foreach (var fileLoc in dllLocations)
            {
                var (result, module) = await ModuleLoader.LoadModule(fileLoc);
                if (result != LoadResult.Loaded)
                {
                    await LogHandler.LogInfo($"Could not load {Path.GetFileName(fileLoc)}.");
                    continue;
                }

                //TODO: Probably a better way to do this
                TrySet<IPermissionHandler>(ref PermissionHandler, module.PermissionHandler, async () => await LogHandler.LogWarning($"Module {module.Info.Name} is trying to replace IPermissionHandler but one is already set. Ignoring"));
                TrySet<ITimeoutHandler>(ref TimeoutHandler, module.TimeoutHandler, async () => await LogHandler.LogWarning($"Module {module.Info.Name} is trying to replace ITimeoutHandler but one is already set. Ignoring."));

                TrySet<ICommandParser>(ref Parser, module.Parser, async () => await LogHandler.LogWarning($"Module {module.Info.Name} is trying to replace ICommandParser but one is already set. Ignoring."));
                TrySet<ILogHandler>(ref LogHandler, module.LogHandler, async () => await LogHandler.LogWarning($"Module {module.Info.Name} is trying to replace ILogHandler but one is already set. Ignoring."));

            }
        }



        public async Task Run()
        {
            await Load();

            await StartShards();

            await Task.Delay(int.MaxValue);

        }

        private async Task StartShards()
        {
            var discordConfig = new DiscordConfiguration()
            {
                AutoReconnect = true,
                ShardCount = await RootConfigHelper.GetShardCount(RunnerConfig),
                Token = await RootConfigHelper.GetToken(RunnerConfig)
            };

            ShardClient = new DiscordShardedClient(discordConfig);

            await EventHandler.SetupShardClient(ShardClient);

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
