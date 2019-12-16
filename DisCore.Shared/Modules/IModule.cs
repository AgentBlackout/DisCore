using System;
using System.Threading.Tasks;
using DisCore.Shared.Commands;
using DisCore.Shared.Logging;
using DSharpPlus.Entities;

namespace DisCore.Shared.Modules
{
    public interface IModule
    {
        String Name { get; }
        String Version { get; }
        String Author { get; }

        String Summary { get; }

        Task OnLoad(ILogHandler log);

        Task OnUnload();

        Task OnLoadGuild(DiscordGuild guild);
    }
}
