using System;
using System.Threading.Tasks;
using DisCore.Shared.Commands;
using DSharpPlus.Entities;

namespace DisCore.Shared.Modules
{
    public interface IModule
    {
        String Name { get; }
        String Version { get; }
        String Author { get; }

        String Summary { get; }

        Task OnLoad(HandlerGroup handlers);

        Task OnUnload(HandlerGroup handlers);

        Task OnLoadGuild(DiscordGuild guild);
    }
}
