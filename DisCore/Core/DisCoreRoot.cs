using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DisCore.Core.Commands;
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

        public ILogHandler LogHandler;

        public List<Entities.Modules.Module> Modules;

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
                    assembly = TryOrLog<Assembly, Exception>(() => Assembly.LoadFile(fileLoc), LogHandler);
                }
                catch (Exception e)
                {
                    await LogHandler.LogWarning($"Failed to load module {Path.GetFileName(fileLoc)} (Make sure it's up to date) - {e.Message}");
                    continue;
                }

                Type moduleType = AssemblyHelper.GetIModuleType(assembly);
                if (moduleType == null)
                {
                    await LogHandler.LogError(
                        $"{Path.GetFileName(fileLoc)} does not contain a class definition which extends IModule");
                    continue;
                }

                IModule modInstance = (IModule)Activator.CreateInstance(moduleType);

                List<CommandGroup> commands = (await CommandGroupFactory.GetCommandGroups(assembly)).ToList();

                int subCommands = commands.Sum(item => item.GetSubCommands().Count);
                await LogHandler.LogInfo(
                    $"Module \"{modInstance.Name}\" defines {(commands.Count == 0 ? "zero" : commands.Count.ToString())} command{(commands.Count == 1 ? "" : "s")} ({subCommands} subcommand{(subCommands == 1 ? "s" : "")})"
                    );

                var module = new Entities.Modules.Module(modInstance)
                {
                    Commands = commands
                };

                Modules.Add(module);
            }
        }
        public async Task Run()
        {
            await Load();
            throw new NotImplementedException();
        }

        private static T TryOrLog<T, TU>(Func<T> func, ILogHandler logHandler) where TU : Exception
        {
            try
            {
                return func();
            }
            catch (TU e)
            {
                logHandler.LogError("", e);
                throw;
            }
        }

    }
}
