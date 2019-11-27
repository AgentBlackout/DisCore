using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DisCore.Shared.Commands.Parser;
using DisCore.Shared.Commands.Timeout;
using DisCore.Shared.Permissions;

namespace DisCore.Shared.Commands
{
    public class CommandGroup
    {
        private readonly ICommandParser _parser;

        private readonly string _commandName;
        private readonly List<CommandOverload> _overloads;

        private readonly List<CommandGroup> _subCommandGroups;

        private readonly Func<Task<string>> _usageFunc;
        private readonly Func<Task<string>> _summaryFunc;

        public CommandAttribute CommandAttribute;
        public TimeoutAttribute Timeout;
        public RequiredPermissions RequiredPermissions;

        public bool IsSubGroup => CommandAttribute.Parent != null;

        public String CommandName => _commandName;

        public CommandGroup(
            string cmdName,
            Func<Task<string>> usage,
            Func<Task<string>> summary,
            CommandAttribute commandAttribute,
            RequiredPermissions perms = null,
            TimeoutAttribute timeout = null
            )
        {
            _commandName = cmdName;
            _overloads = new List<CommandOverload>();
            _subCommandGroups = new List<CommandGroup>();

            CommandAttribute = commandAttribute ?? throw new ArgumentNullException(nameof(commandAttribute));
            
            _usageFunc = usage;
            _summaryFunc = summary;


            RequiredPermissions = perms ?? new RequiredPermissions(PermissionLevels.User);

            Timeout = timeout ?? new TimeoutAttribute();
        }

        public List<CommandGroup> GetSubGroups() => _subCommandGroups;

        public void AddOverload(CommandOverload cmd) => _overloads.Add(cmd);
        
        public IEnumerable<CommandOverload> GetOverloads() => _overloads;

        public void AddSubCommand(CommandGroup cmdG) => _subCommandGroups.Add(cmdG);

        public async Task<CommandResult> Call(List<CommandParameter[]> potentialArgs)
        {
            //TODO
            throw new NotImplementedException();
        }

    }
}
