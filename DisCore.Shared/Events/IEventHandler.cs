using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace DisCore.Shared.Events
{
    public interface IEventHandler
    {
        Task SetupShardClient(DiscordShardedClient shardedClient);

        Task RegisterEvents(Assembly assembly);

        Task HandleEvent(DiscordEventArgs args);
    }
}
