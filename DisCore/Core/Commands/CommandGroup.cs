using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DisCore.Core.Commands.Parser;
using DisCore.Core.Commands.Timeouts;
using DisCore.Core.Entities.Commands;
using DisCore.Core.Permissions;
using DisCore.Helpers;
using DSharpPlus.Entities;

namespace DisCore.Core.Commands
{
    public class CommandGroup
    {
        private readonly ICommandParser _parser;

        private readonly string _commandName;
        private List<Command> _commands;

        private List<CommandGroup> _subCommandGroups;

        private Func<CommandContext, Task<CommandResult>> _usageFunc;
        private Func<CommandContext, Task<CommandResult>> _summaryFunc;

        public TimeoutAttribute Timeout;
        public RequiredPermissions RequiredPermissions;


        public CommandGroup(
            string cmdName,
            Func<CommandContext, Task<CommandResult>> usage,
            Func<CommandContext, Task<CommandResult>> summary,
            RequiredPermissions perms = null,
            TimeoutAttribute timeout = null
            )
        {
            _commandName = cmdName;
            _commands = new List<Command>();

            _usageFunc = usage;
            _summaryFunc = summary;


            RequiredPermissions = perms ?? new RequiredPermissions(PermissionLevels.User);

            Timeout = timeout ?? new TimeoutAttribute();
        }

        public void AddCommand(Command cmd) => _commands.Add(cmd);

        public void RemoveCommand(Command cmd) => _commands.Remove(cmd);

        public IEnumerable<Command>  GetCommands() => _commands;

        public void AddSubCommand(CommandGroup cmdG) => _subCommandGroups.Add(cmdG);

        public async Task<CommandResult> Call(List<CommandParameter[]> potentialArgs)
        {
            foreach (var argSet in potentialArgs)
            {
                if (argSet.Length == 0)
                    continue;
                var firstItem = argSet.First();

                if (firstItem.Type == typeof(string))
                {
                    string firstString = (String) firstItem.Object;
                    var subItem = _subCommandGroups.FirstOrDefault(item => item._commandName == firstString);
                    if (subItem != null)
                    {
                        return subItem.Call(argSet.Where(item => item != firstItem));
                    }
                }
            }

            throw new NotImplementedException();
        }

    }
}
