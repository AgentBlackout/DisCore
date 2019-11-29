using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DisCore.Runner.Loaders.Module;
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
        private readonly IModuleLoader _moduleLoader;
        private readonly List<CommandGroup> _commands;

            public CommandParser(IConfigManager configManager, IModuleLoader loader)
        {
            _configManager = configManager;
            _moduleLoader = loader;

            _commands = _moduleLoader.GetModules().SelectMany(module => module.Commands).ToList();
        }

        public async Task<(CommandParseResult Result, ParsedCommand Command)> ParseMessage(DiscordMessage message)
        {
            string prefix = await ConfigHelper.GetGuildPrefix(_configManager, message.Channel.Guild);

            if (!message.Content.StartsWith(prefix))
                return (CommandParseResult.NotCommand, null);// It's not a command

            //TODO: Aliases

            string command = message.Content.Substring(0, prefix.Length);
            var group = _commands.FirstOrDefault(group => String.Equals(@group.CommandName, command, StringComparison.CurrentCultureIgnoreCase));

            if (group == null)
                return (CommandParseResult.NoCommand, null);

            //Try and match the longest sequences first first
            var overloads = GetCommandOverloadsRecursive(group).OrderByDescending(overload => overload.GetParameters().Count());

            return (CommandParseResult.NotFound, null);
        }

        public bool IsCommand(DiscordMessage message)
        {
            //TODO
            return false;
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
