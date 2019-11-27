using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DisCore.Runner.Factories;
using DisCore.Runner.Helpers;
using DisCore.Shared.Commands;
using DisCore.Shared.Config;
using DisCore.Shared.Events;
using DisCore.Shared.Logging;
using DisCore.Shared.Modules;
using DisCore.Shared.Permissions;

namespace DisCore.Runner.Loaders.Module
{
    public class ModuleLoader : IModuleLoader
    {

        private readonly AppDomain _domain;
        private readonly ILogHandler _logHandler;

        private readonly List<DllModule> _modules;
        private readonly IEventHandler _eventHandler;

        public IEnumerable<DllModule> GetModules() => _modules;

        public ModuleLoader(IEventHandler eventHandler, ILogHandler logHandler)
        {
            _logHandler = logHandler;
            _eventHandler = eventHandler;

            _modules = new List<DllModule>();

            _domain = AppDomain.CurrentDomain;
        }

        /// <summary>
        /// Load a module
        /// </summary>
        /// <param name="filepath">Filepath of the DLL to load</param>
        public async Task<(LoadResult, DllModule)> LoadModule(string filepath)
        {
            DllModule module;
            try
            {
                module = await TryLoadModule(filepath);
            }
            catch (Exception)
            {
                return (LoadResult.Error, null);
            }

            return (LoadResult.Loaded, module);
        }

        /// <summary>
        /// Tries to load a module, can raise a variety of exceptions.
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns>Task</returns>
        private async Task<DllModule> TryLoadModule(string filepath)
        {
            var fileName = Path.GetFileName(filepath);
            await _logHandler.LogDebug($"Trying to load DLL {fileName}");

            Assembly assembly = await TryOrLog<Assembly, Exception>(async () => await AssemblyHelper.ReadAndLoad(_domain, filepath), _logHandler);

            Type moduleType = await TryOrLog<Type, ReflectionTypeLoadException>(async () => AssemblyHelper.GetIModuleType(assembly), _logHandler);

            if (moduleType == null)
            {
                await _logHandler.LogError(
                    $"{fileName} does not contain a class definition which implements IModule");
                throw new Exception("Assembly does not contain a definition which implements IModule");
            }

            IModule modInstance = await TryOrLog<IModule, Exception>(() => Task.FromResult((IModule)Activator.CreateInstance(moduleType)), _logHandler);

            List<CommandGroup> commands = (await CommandGroupFactory.GetCommandGroups(assembly, _logHandler)).ToList();

            int subCommands = commands.Sum(item => item.GetSubGroups().Count);
            await _logHandler.LogInfo(
                        $"Module \"{modInstance.Name}\" defines {(commands.Count == 0 ? "zero" : commands.Count.ToString())} command{(commands.Count == 1 ? "" : "s")} ({subCommands} subcommand{(subCommands == 1 ? "s" : "")})"
                    );

            var module = new DllModule(modInstance)
            {
                Commands = commands
            };

            _modules.Add(module);

            Type permHandler = AssemblyHelper.GetImplementers<IPermissionHandler>(assembly).FirstOrDefault();
            if (permHandler != null)
                module.PermissionHandler = (IPermissionHandler)Activator.CreateInstance(permHandler);

            Type logHandler = AssemblyHelper.GetImplementers<ILogHandler>(assembly).FirstOrDefault();
            if (logHandler != null)
                module.LogHandler = (ILogHandler) Activator.CreateInstance(logHandler);

            //TODO 


            return module;
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
