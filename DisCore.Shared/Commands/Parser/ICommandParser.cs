using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace DisCore.Shared.Commands.Parser
{
    public interface ICommandParser
    {
        Task<(CommandOverload, CommandParameter[])> GetCommandAndParameters(CommandGroup masterGroup, DiscordMessage message);
    }
}
