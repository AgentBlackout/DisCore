using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using DisCore.Runner.Helpers;
using DisCore.Shared.Commands.Parser;
using DisCore.Shared.Events;
using DisCore.Shared.Logging;
using DisCore.Shared.Modules;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace DisCore.Runner.Events
{
    public class EventHandler : IEventHandler
    {

        private Dictionary<string, List<EventMethod>> _funcMap;
        private ILogHandler _log;
        private ICommandParser _parser;

        public EventHandler(ILogHandler log)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));

            _funcMap = new Dictionary<string, List<EventMethod>>();
        }


        public async Task SetupShardClient(DiscordShardedClient shardedClient)
        {
            shardedClient.MessageCreated += async e => await HandleEvent(e);
            shardedClient.MessageDeleted += async e => await HandleEvent(e);
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

            if (!_funcMap.TryGetValue(key, out var methods)) //If key doesn't exist
            {
                methods = new List<EventMethod>();
                _funcMap.Add(key, methods);
            }

            var eventMethod = GetEventMethod(method, module);

            methods.Add(eventMethod);

            await _log.LogDebug($"Added method {method.Name} from {method.Module.Assembly.GetShortName()} to actionmap");

            return true;
        }

        private EventMethod GetEventMethod(MethodInfo info, IModule module)
        {
            var wrapperFunc = GetWrapper(info, module);
            var listenerAttrib = info.GetCustomAttribute<ListenerAttribute>();

            return new EventMethod(info, listenerAttrib, wrapperFunc);

        }

        private Func<DiscordEventArgs, Task> GetWrapper(MethodInfo info, IModule module)
        {
            //This is less than ideal but I don't know what else to do
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
                    throw new InvalidOperationException();
            }
        }

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

        public async Task HandleEvent(DiscordEventArgs eventArgs)
        {
            if (_parser == null)
            {
                await _log.LogError("Cannot handle event, parser is null!");
                return;
            }

            if (ShouldIgnore(eventArgs))
                return;

            //TODO IsCommand
            bool isCommand = false;
            if (eventArgs is MessageCreateEventArgs mcea)
            {
                isCommand = _parser.IsCommand(mcea.Message);

                if (isCommand)
                    await _parser.ParseMessage(mcea.Message);
            }

            var type = eventArgs.GetType();

            if (!_funcMap.TryGetValue(type.Name, out var events))
                return;


            await _log.LogDebug($"Starting tasks {events.Count}");
            foreach (var eventMethod in events)
            {
                if (eventMethod.ListenerAttribute.IgnoreCommands && isCommand)
                    continue;

                var func = eventMethod.Func;
                _ = func(eventArgs);
            }
            await _log.LogDebug("finished starting tasks");
        }

        public void SetCommandParser(ICommandParser parser)
        {
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
        }

        private bool ShouldIgnore(DiscordEventArgs eventArgs)
        {
            if (eventArgs is MessageCreateEventArgs mcea) return mcea.Author.IsBot;
            if (eventArgs is MessageDeleteEventArgs mdea) return mdea.Message.Author.IsBot;
            if (eventArgs is MessageReactionAddEventArgs mraea) return mraea.User.IsBot;
            if (eventArgs is MessageReactionRemoveEventArgs mrrea) return mrrea.User.IsBot;

            return false;
        }
    }
}
