using System;
using System.Collections;
using System.Collections.Concurrent;
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
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace DisCore.Runner.Events
{
    public class EventHandler : IEventHandler
    {

        private readonly Dictionary<string, List<EventMethod>> _funcMap;
        private readonly ILogHandler _log;
        private readonly EventMethodManager _methodManager;

        private readonly List<(DiscordChannel channel, TaskCompletionSource<DiscordMessage> completionToken)>
            _watchedChannels;

        private ICommandParser _parser;

        private readonly Dictionary<DiscordChannel, ConcurrentQueue<DiscordMessage>> _channelQueues;

        public EventHandler(ILogHandler log)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));

            _methodManager = new EventMethodManager(_log);

            _funcMap = new Dictionary<string, List<EventMethod>>();
            _channelQueues = new Dictionary<DiscordChannel, ConcurrentQueue<DiscordMessage>>();
            _watchedChannels = new List<(DiscordChannel channel, TaskCompletionSource<DiscordMessage> completionToken)>();
        }


        public async Task SetupShardClient(DiscordShardedClient shardedClient)
        {
            shardedClient.MessageCreated += async e => await HandleEvent(e);
            shardedClient.MessageDeleted += async e => await HandleEvent(e);
            //TODO Rest of the events
        }
        public async Task SetParser(ICommandParser parser)
        {
            await _log.LogDebug($"Updating EventHandler Parser to instance of {parser.GetType().FullName}");
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
        }

        public async Task<DiscordMessage> GetNextMessageAsync(DiscordChannel channel, TimeSpan timeout)
        {
            var completeSource = new TaskCompletionSource<DiscordMessage>();

            //Create a task to automatically return null after the timeout
            _ = Task.Factory.StartNew(
                async () => {
                    await Task.Delay((int)timeout.TotalMilliseconds);
                    completeSource.TrySetResult(null);
                }
            );

            _watchedChannels.Add((channel, completeSource));
            await _log.LogDebug("Registering waiting for message");
            return await completeSource.Task;
        }


        public async Task<int> RegisterEvents(DllModule module)
        {
            int successes;
            try
            {
                successes = await TryRegisterEvents(module);
            }
            catch (Exception e)
            {
                await _log.LogError($"Could not register events for module {module.Assembly.GetShortName()}", e);
                return 0;
            }

            return successes;
        }

        private async Task<int> TryRegisterEvents(DllModule module)
        {
            int successes = 0;

            foreach (var method in await _methodManager.GetMethods(module))
            {
                var key = method.Parameter.Name;

                if (!_funcMap.TryGetValue(key, out var methods)) //If key doesn't exist
                {
                    methods = new List<EventMethod>();
                    _funcMap.Add(key, methods);
                }

                methods.Add(method);

            }

            return successes;
        }

        public async Task HandleEvent(DiscordEventArgs args)
        {
            try
            {
                await TryHandleEvent(args);
            }
            catch (Exception e)
            {
                await _log.LogError("", e);
            }
        }



        private async Task TryHandleEvent(DiscordEventArgs eventArgs)
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
                isCommand = await _parser.IsCommand(mcea.Message);

                if (isCommand)
                    _ = _parser.ParseAndRun(mcea.Message);//Parse & run message asynchronously  
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
