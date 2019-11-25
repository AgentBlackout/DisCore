using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DisCore.Shared.Commands;
using DisCore.Shared.Commands.Parser;
using DisCore.Shared.Config;
using DSharpPlus.Entities;

namespace DisCore.Runner.Parser
{
    class CommandParser : ICommandParser
    {
        private IConfigManager _configManager;

        public CommandParser(IConfigManager configManager)
        {
            _configManager = configManager;
        }

        public async Task<(CommandOverload Overload, CommandParameter[] Params)> ParseMessage(DiscordMessage message, CommandGroup[] commands)
        {
            var overloads = commands.SelectMany(GetCommandOverloadsRecursive);
            var guildConfig = await _configManager.GetGuildConfig(message.Channel.Guild);

            throw new NotImplementedException();
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
