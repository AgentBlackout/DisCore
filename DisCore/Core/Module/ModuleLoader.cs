using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DisCore.Core.Commands;
using DisCore.Core.Entities.Modules;
using DisCore.Core.Logging;
using DisCore.Factories;
using DisCore.Helpers;

namespace DisCore.Core.Module
{
    public class ModuleLoader : IModuleLoader
    {
        private readonly ILogHandler _logHandler;
        private readonly List<DllModule> _modules;

        public IEnumerable<DllModule> GetModules() => _modules;

        public ModuleLoader(ILogHandler logHandler)
        {
            _logHandler = logHandler;
            _modules = new List<DllModule>();
        }

        public async Task<(bool Success, string Reason)> LoadModule(string filepath)
        {
            try
            {
                await TryLoadModule(filepath);
            }
            catch (Exception e)
            {
                await _logHandler.LogError($"Uncaught error trying to load module {filepath}");
                return (false, e.Message);
            }

            return (true, "Success");
        }

        private async Task TryLoadModule(string filepath)
        {
            var fileName = Path.GetFileName(filepath);
            await _logHandler.LogDebug($"Trying to load DLL {fileName}");
            Assembly assembly;
            try
            {
                assembly = await TryOrLog<Assembly, Exception>(async () => Assembly.LoadFile(filepath), _logHandler);
            }
            catch (Exception e)
            {
                await _logHandler.LogWarning(
                    $"Failed to load module {fileName} (Make sure it's up to date) - {e.Message}");
                return;
            }

            Type moduleType;
            try
            {
                moduleType = await TryOrLog<Type, ReflectionTypeLoadException>(async () => AssemblyHelper.GetIModuleType(assembly), _logHandler);
            }
            catch (ReflectionTypeLoadException e)
            {
                await _logHandler.LogWarning($"Failed to load {fileName} (it's probably out of date)");
                return;
            }

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
