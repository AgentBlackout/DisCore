using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DisCore.Shared.Events;
using DisCore.Shared.Logging;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace DisCore.Runner.Events
{
    public class EventHandler : IEventHandler
    {

        private Dictionary<string, List<Action<DiscordEventArgs>>> _actionMap;
        private ILogHandler _log;

        public EventHandler(ILogHandler log)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }


        public async Task SetupShardClient(DiscordShardedClient shardedClient)
        {
            shardedClient.MessageCreated += async e => await HandleEvent(e);
        }

        public async Task RegisterEvents(Assembly assembly)
        {
            throw new NotImplementedException();
        }

        public async Task HandleEvent(DiscordEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }
    }
}
