using System;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace DisCore.Shared.Modules
{
    public interface IModule
    {
        String Name { get; }
        String Version { get; }
        String Author { get; }

        String Summary { get; }

        Task OnLoad();

        Task OnUnload();

        Task OnLoadGuild(DiscordGuild guild);

    }
}
