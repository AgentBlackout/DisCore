using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace DisCore.Shared.Commands.Parser
{
    public enum CommandParseResult
    {
        Success,
        NoCommand,
        NotCommand,
        NoArgs,
        NotFound
    }

    public class ParsedCommand
    {
        public readonly CommandOverload Overload;
        public readonly IEnumerable<CommandParameter> Params;

        public ParsedCommand(CommandOverload overload, IEnumerable<CommandParameter> parameters)
        {
            Overload = overload;
            Params = parameters;
        }
    }

    public interface ICommandParser
    {
        Task<CommandResult> ParseAndRun(DiscordMessage message);
        Task<(CommandParseResult Result, ParsedCommand Command)> ParseMessage(DiscordMessage message);
        Task<bool> IsCommand(DiscordMessage message);
    }
}
