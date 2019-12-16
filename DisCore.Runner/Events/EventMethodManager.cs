using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DisCore.Runner.Helpers;
using DisCore.Shared.Events;
using DisCore.Shared.Logging;
using DisCore.Shared.Modules;
using DSharpPlus.EventArgs;

namespace DisCore.Runner.Events
{
    internal class EventMethodManager
    {
        private readonly ILogHandler _log;

        public EventMethodManager(ILogHandler log)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        private async Task<EventMethod> TryRegisterEvent(MethodInfo method, IModule module)
        {
            var parameter = method.GetParameters().First();

            var eventMethod = GetEventMethod(method, module);
            
            await _log.LogDebug($"Added method {method.Name} from {method.Module.Assembly.GetShortName()} to actionmap");

            return eventMethod;
        }

        private async Task<IEnumerable<MethodInfo>> GetValidMethods(Assembly assembly)
        {
            var methods = AssemblyHelper.GetMethodsWithAttribute<ListenerAttribute>(assembly);
            var valid = new List<MethodInfo>();

            //Check validity of methods at creation
            foreach (var method in methods)
            {
                if (method.ReturnType != typeof(Task))
                {
                    await _log.LogWarning(
                        $"{method.Module.Name} contains method {method.Name} which has ListenerAttribute but the wrong return type. Ignoring");
                    continue;
                }

                var parameters = method.GetParameters();
                if (parameters.Length != 1)
                {
                    await _log.LogWarning(
                        $"{method.Module.Name} contains method {method.Name} which has ListenerAttribute but the wrong number of parameters. Ignoring.");
                    continue;
                }

                if (!parameters.First().ParameterType.IsSubclassOf(typeof(DiscordEventArgs)))
                {
                    await _log.LogWarning(
                        $"{method.Module.Name} contains method {method.Name} which has ListenerAttribute but the wrong parameter type (should be subclass of DiscordEventArgs). Ignoring.");
                    continue;
                }

                // Else it's valid
                valid.Add(method);
            }

            return valid;
        }

        private EventMethod GetEventMethod(MethodInfo info, IModule module)
        {
            var wrapperFunc = GetWrapper(info, module);
            var listenerAttrib = info.GetCustomAttribute<ListenerAttribute>();

            return new EventMethod(info, listenerAttrib, wrapperFunc);

        }

        /// <summary>
        /// Turn a MethodInfo with a module reference into a func
        /// </summary>
        /// <param name="info">MethodInfo</param>
        /// <param name="module">Object reference to class with functions</param>
        /// <returns></returns>
        private Func<DiscordEventArgs, Task> GetWrapper(MethodInfo info, IModule module)
        {
            var parameter = info.GetParameters().First();

            switch (parameter.ParameterType.Name)
            {
                case nameof(MessageCreateEventArgs):
                    return CreateWrapper<MessageCreateEventArgs>(info, module);
                case nameof(MessageDeleteEventArgs):
                    return CreateWrapper<MessageDeleteEventArgs>(info, module);
                case nameof(MessageReactionAddEventArgs):
                    return CreateWrapper<MessageReactionAddEventArgs>(info, module);
                case nameof(MessageReactionRemoveEventArgs):
                    return CreateWrapper<MessageReactionRemoveEventArgs>(info, module);

                default:
                    throw new ArgumentOutOfRangeException($"Parameter type {parameter.Name} is either invalid or not yet supported.");
            }
        }

        /// <summary>
        /// Convert a func with parameter T into a func which takes DiscordEventARgs
        /// </summary>
        /// <typeparam name="T">EventType</typeparam>
        /// <param name="method">Method assembly info</param>
        /// <param name="module">Owning object reference</param>
        /// <returns></returns>
        private Func<DiscordEventArgs, Task> CreateWrapper<T>(MethodInfo method, IModule module) =>
            async (obj) =>
            {
                var func = FuncHelper.MethodInfoToFunc<Func<T, Task>>(method, module);
                try
                {
                    await func((T)(object)obj);
                }
                catch (Exception e)
                {
                    await _log.LogError("", e);
                    return;
                }
            };

        public async Task<IEnumerable<EventMethod>> GetMethods(DllModule module)
        {
            var methods = new List<EventMethod>();
            IEnumerable<MethodInfo> validMethods;

            try
            {
                validMethods = await GetValidMethods(module.Assembly);
            }
            catch (Exception e)
            {
                await _log.LogError("", e);
                return Enumerable.Empty<EventMethod>();
            }

            //turn valid methods to funcs and add to dictionary
            foreach (var method in validMethods)
            {
                try
                {
                    methods.Add(await TryRegisterEvent(method, module.ModuleObject));
                }
                catch (Exception e)
                {
                    await _log.LogError($"Error registering event method {method.Name}", e);
                    continue;
                }
            }

            return methods;
        }
    }
}
