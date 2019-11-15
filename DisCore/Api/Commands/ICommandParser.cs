using System.Threading.Tasks;
using DisCore.Core.Commands;
using DisCore.Core.Commands.Parser;
using DSharpPlus.Entities;

namespace DisCore.Api.Commands
{
    public interface ICommandParser
    {
        Task<(Command, CommandParameter[])> GetCommandAndParameters(CommandGroup masterGroup, DiscordMessage message);
    }
}
