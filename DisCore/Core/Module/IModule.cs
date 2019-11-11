using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;

namespace DisCore.Core.Module
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
