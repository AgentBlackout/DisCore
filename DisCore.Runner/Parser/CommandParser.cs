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
using DisCore.Shared.Logging;
using DSharpPlus.Entities;

namespace DisCore.Runner.Parser
{
    public sealed class CommandParser : ICommandParser
    {
        private readonly IConfigManager _configManager;
        private readonly IModuleLoader _moduleLoader;
        private readonly ILogHandler _log;

        private readonly List<CommandGroup> _commands;

        public CommandParser(IConfigManager configManager, IModuleLoader loader, ILogHandler log)
        {
            _log = log;
            _configManager = configManager;
            _moduleLoader = loader;

            _commands = _moduleLoader.GetModules().SelectMany(module => module.Commands).ToList();
        }

        public async Task ParseAndRun(DiscordMessage message)
        {
            //TODO: Warning - Untested
            var parsedCommand = await ParseMessage(message);

            var execResult = await parsedCommand.Overload.GetGroup().Call(parsedCommand.Params.ToList());

            switch (execResult.Result)
            {
                case (CommandResultType.PermissionDenied):
                    {
                        await message.RespondAsync("You do not have permissions for that");
                        break;
                    }
                case (CommandResultType.Cooldown):
                    {
                        var time = (TimeSpan)execResult.Object;
                        await message.RespondAsync($"That command is still on cooldown. Available in {time.TotalSeconds} seconds");
                        break;
                    }
                case (CommandResultType.BadArgs):
                    {
                        var usage = await parsedCommand.Overload.GetGroup().GetUsage();
                        var embed = new DiscordEmbedBuilder()
                        {
                            Title = "Invalid Arguments",
                            Description = usage
                        };

                        await message.Channel.SendMessageAsync(embed: embed);
                        break;
                    }

                default:
                case (CommandResultType.Exception):
                    {
                        await message.Channel.SendMessageAsync($"Exception occured while executing command `{parsedCommand.Overload.GetGroup().CommandName}`. It has been logged");
                        await _log.LogError($"Error while processing command {parsedCommand.Overload.GetGroup().CommandName}", e: (Exception)execResult.Object);
                        break;
                    }
            }

            return;

        }

        public async Task<ParsedCommand> ParseMessage(DiscordMessage message)
        {
            string prefix = await ConfigHelper.GetGuildPrefix(_configManager, message.Channel.Guild);

            //TODO: Aliases

            string command = message.Content.Substring(prefix.Length).Split(" ").FirstOrDefault();
            if (command == null)
                return null;

            var group = _commands.FirstOrDefault(group => String.Equals(group.CommandName, command, StringComparison.CurrentCultureIgnoreCase));

            if (group == null)
                return null;


            //Try and match the longest sequences first first
            var overloads = GetCommandOverloadsRecursive(group).OrderByDescending(overload => overload.GetParameters().Count());

            foreach (var overload in overloads)
            {
                var parameters = overload.GetParameterTypes();
                if (parameters.Count() > 2)
                {
                    var parameterGroupSums = parameters.GroupBy(i => i);
                }
            }

            return null;
        }

        public async Task<bool> IsCommand(DiscordMessage message)
        {
            string prefix = await ConfigHelper.GetGuildPrefix(_configManager, message.Channel.Guild);
            return message.Content.StartsWith(prefix);
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
