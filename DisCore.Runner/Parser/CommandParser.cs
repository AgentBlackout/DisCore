using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DisCore.Shared.Commands;
using DisCore.Shared.Commands.Parser;
using DisCore.Shared.Config;
using DisCore.Shared.Helpers;
using DSharpPlus.Entities;

namespace DisCore.Runner.Parser
{
    public sealed class CommandParser : ICommandParser
    {
        private readonly IConfigManager _configManager;

        public CommandParser(IConfigManager configManager)
        {
            _configManager = configManager;
        }

        public async Task<(CommandParseResult Result, ParsedCommand Command)> ParseMessage(DiscordMessage message, IEnumerable<CommandGroup> commands)
        {
            string prefix = await ConfigHelper.GetGuildPrefix(_configManager, message.Channel.Guild);

            if (!message.Content.StartsWith(prefix))
                return (CommandParseResult.NotCommand, null);// It's not a command

            //TODO: Aliases

            string command = message.Content.Substring(0, prefix.Length);
            var group = commands.FirstOrDefault(group => String.Equals(@group.CommandName, command, StringComparison.CurrentCultureIgnoreCase));

            if (group == null)
                return (CommandParseResult.NoCommand, null);

            //Try and match the longest sequences first first
            var overloads = GetCommandOverloadsRecursive(group).OrderByDescending(overload => overload.GetParameters().Count());

            return (CommandParseResult.NotFound, null);
        }


        public IEnumerable<CommandOverload> GetCommandOverloadsRecursive(CommandGroup group)
        {
            var overloads = new List<CommandOverload>();
            foreach (var overload in group.GetOverloads())
            {
                overloads.Add(overload);
            }

            foreach (var subGroup in group.GetSubGroups())
            {
                foreach (var overload in GetCommandOverloadsRecursive(subGroup))
                {
                    overloads.Add(overload);
                }
            }

            return overloads;
        }
    }
}
