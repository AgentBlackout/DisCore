using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DisCore.Api.Commands;
using DisCore.Api.Commands.Timeout;
using DisCore.Api.Permissions;
using DisCore.Core.Commands.Parser;

namespace DisCore.Core.Commands
{
    public class CommandGroup
    {
        private readonly ICommandParser _parser;

        private readonly string _commandName;
        private readonly List<Command> _commands;

        private readonly List<CommandGroup> _subCommandGroups;

        private readonly Func<CommandContext, Task<CommandResult>> _usageFunc;
        private readonly Func<CommandContext, Task<CommandResult>> _summaryFunc;

        public CommandAttribute CommandAttribute;
        public TimeoutAttribute Timeout;
        public RequiredPermissions RequiredPermissions;

        public bool IsSubGroup => CommandAttribute.Parent != null;

        public String CommandName => _commandName;

        public CommandGroup(
            string cmdName,
            Func<CommandContext, Task<CommandResult>> usage,
            Func<CommandContext, Task<CommandResult>> summary,
            CommandAttribute commandAttribute,
            RequiredPermissions perms = null,
            TimeoutAttribute timeout = null
            )
        {
            _commandName = cmdName;
            _commands = new List<Command>();
            _subCommandGroups = new List<CommandGroup>();

            CommandAttribute = commandAttribute ?? throw new ArgumentNullException(nameof(commandAttribute));
            
            _usageFunc = usage;
            _summaryFunc = summary;


            RequiredPermissions = perms ?? new RequiredPermissions(PermissionLevels.User);

            Timeout = timeout ?? new TimeoutAttribute();
        }

        public List<CommandGroup> GetSubCommands() => _subCommandGroups;

        public void AddCommand(Command cmd) => _commands.Add(cmd);

        public void RemoveCommand(Command cmd) => _commands.Remove(cmd);

        public IEnumerable<Command> GetCommands() => _commands;

        public void AddSubCommand(CommandGroup cmdG) => _subCommandGroups.Add(cmdG);

        public async Task<CommandResult> Call(List<CommandParameter[]> potentialArgs)
        {

            throw new NotImplementedException();
        }

    }
}
