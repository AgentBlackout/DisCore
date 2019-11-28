using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using DisCore.Runner.Helpers;
using DisCore.Shared.Events;
using DisCore.Shared.Logging;
using DisCore.Shared.Modules;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace DisCore.Runner.Events
{
    public class EventHandler : IEventHandler
    {

        private Dictionary<string, List<Func<DiscordEventArgs, Task>>> _funcMap;
        private ILogHandler _log;

        public EventHandler(ILogHandler log)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));

            _funcMap = new Dictionary<string, List<Func<DiscordEventArgs, Task>>>();
        }


        public async Task SetupShardClient(DiscordShardedClient shardedClient)
        {
            shardedClient.MessageCreated += async e => await HandleEvent(e);
            //TODO
        }

        public async Task<int> RegisterEvents(DllModule module)
        {
            int successes = 0;

            //turn valid methods to funcs and add to dictionary
            foreach (var method in await GetValidMethods(module.Assembly))
            {
                try
                {
                    await TryRegisterEvent(method, module.ModuleObject);
                }
                catch (Exception e)
                {
                    await _log.LogError($"Error registering event method {method.Name}", e);
                    continue;
                }

                successes++;//Can only get here if there was no error
            }

            return successes;
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

        private async Task<bool> TryRegisterEvent(MethodInfo method, IModule module)
        {
            var parameter = method.GetParameters().First();
            var key = parameter.ParameterType.Name;

            List<Func<DiscordEventArgs, Task>> funcs;

            if (!_funcMap.TryGetValue(key, out funcs)) //If key doesn't exist
            {
                funcs = new List<Func<DiscordEventArgs, Task>>();
                _funcMap.Add(key, funcs);
            }

            await _log.LogDebug($"Adding method {method.Name} from {method.Module.Assembly.FullName} to actionmap");

            Func<DiscordEventArgs, Task> wrapper;
            switch (parameter.ParameterType.Name)
            {
                case "MessageCreateEventArgs": //TODO there has to be a way to do this through reflection 
#warning This desperately needs changing.
                    {
                        var realFunc = FuncHelper.MethodInfoToFunc<Func<MessageCreateEventArgs, Task>>(method,
                            module);
                        wrapper = (obj) => realFunc((MessageCreateEventArgs)obj);
                        break;
                    }
                default:
                    throw new InvalidOperationException();
            }

            funcs.Add(wrapper);
            return true;
        }



        public async Task HandleEvent(DiscordEventArgs eventArgs)
        {
            //TODO

            Console.WriteLine(eventArgs.GetType());
            //throw new NotImplementedException();
        }
    }
}
