using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace DisCore.Shared.Commands.Parser
{
    public interface ICommandParser
    {
        Task<(CommandOverload Overload, CommandParameter[] Params)> ParseMessage(DiscordMessage message, CommandGroup[] commands);
    }
}
