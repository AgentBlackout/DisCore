using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DisCore.Core.Commands;
using DisCore.Core.Config;
using DisCore.Core.Entities.Modules;
using DisCore.Core.Logging;
using DisCore.Factories;
using DisCore.Helpers;

namespace DisCore.Core.Loaders.Module
{
    public class ModuleLoader : IModuleLoader
    {

        private readonly AppDomain _domain;
        private readonly ILogHandler _logHandler;
        private readonly string _modulesLocation;

        private readonly IConfig _config;

        private readonly List<DllModule> _modules;

        public IEnumerable<DllModule> GetModules() => _modules;

        public ModuleLoader(string moduleLocation, IConfig config, ILogHandler logHandler)
        {
            _modulesLocation = moduleLocation;
            _config = config;

            _logHandler = logHandler;
            _modules = new List<DllModule>();

            _domain = AppDomain.CurrentDomain;
        }

        public async Task<int> LoadModules()
        {
            if (_modules.Any())
                throw new InvalidOperationException("Modules already loaded");

            IEnumerable<string> locations = FileHelper.GetDLLs(_modulesLocation);
            foreach (string location in locations)
            {
                bool success;
                string reason;
                try
                {
                    (success, reason) = await LoadModule(location);
                }
                catch (Exception e)
                {
                    success = false;
                    reason = e.Message;
                }

                if (success)
                    await _logHandler.LogInfo($"Loaded {location} module successfully");
                else
                    await _logHandler.LogWarning($"Failed to failed to load {location} ({reason})");

            }

            return _modules.Count();
        }

        public async Task<(bool Success, string Reason)> LoadModule(string filepath)
        {
            try
            {
                await TryLoadModule(filepath);
            }
            catch (Exception e)
            {
                return (false, e.Message);
            }

            return (true, "Success");
        }

        private async Task TryLoadModule(string filepath)
        {
            var fileName = Path.GetFileName(filepath);
            await _logHandler.LogDebug($"Trying to load DLL {fileName}");

            Assembly assembly = await TryOrLog<Assembly, Exception>(async () => await AssemblyHelper.ReadAndLoad(filepath), _logHandler);

            Type moduleType = await TryOrLog<Type, ReflectionTypeLoadException>(async () => AssemblyHelper.GetIModuleType(assembly), _logHandler);

            if (moduleType == null)
            {
                await _logHandler.LogError(
                    $"{fileName} does not contain a class definition which extends IModule");
                return;
            }

            IModule modInstance = await TryOrLog<IModule, Exception>(async () => (IModule)Activator.CreateInstance(moduleType), _logHandler);

            List<CommandGroup> commands = (await CommandGroupFactory.GetCommandGroups(assembly)).ToList();

            int subCommands = commands.Sum(item => item.GetSubCommands().Count);
            await _logHandler.LogInfo(
                        $"Module \"{modInstance.Name}\" defines {(commands.Count == 0 ? "zero" : commands.Count.ToString())} command{(commands.Count == 1 ? "" : "s")} ({subCommands} subcommand{(subCommands == 1 ? "s" : "")})"
                    );

            var module = new Entities.Modules.DllModule(modInstance)
            {
                Commands = commands
            };

            _modules.Add(module);
        }

        /// <summary>
        /// Try the function or trigger log event
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <typeparam name="TU">Exception to catch</typeparam>
        /// <param name="func">Action</param>
        /// <param name="logFunc">LogHandler</param>
        /// <returns></returns>
        private static async Task<T> TryOrLog<T, TU>(Func<Task<T>> func, ILogHandler log) where TU : Exception
        {
            try
            {
                return await func();
            }
            catch (TU e)
            {
                await log.LogError("", e);
                throw;
            }
        }


    }
}
