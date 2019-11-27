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
        /// <summary>
        /// Setup event delegates on a sharded client1
        /// </summary>
        /// <param name="shardedClient">Client</param>
        /// <returns></returns>
        Task SetupShardClient(DiscordShardedClient shardedClient);

        /// <summary>
        /// Register all events in an assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        Task RegisterEvents(Assembly assembly);

        /// <summary>
        /// Processes and dispatches event to handler
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        Task HandleEvent(DiscordEventArgs args);
    }
}
