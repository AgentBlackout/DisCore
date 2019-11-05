using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using DisCore.Core.Commands.Parser;
using DisCore.Core.Commands.Timeouts;
using DisCore.Core.Entities.Commands;
using DisCore.Core.Permissions;
using DSharpPlus.Entities;

namespace DisCore.Core.Commands
{
    public class CommandGroup
    {
        private readonly ICommandParser _parser;

        private readonly string _commandName;
        private List<MethodInfo> _methods;

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
            _methods = new List<MethodInfo>();

            _usageFunc = usage;
            _summaryFunc = summary;


            RequiredPermissions = perms ?? new RequiredPermissions(PermissionLevels.User);

            Timeout = timeout ?? new TimeoutAttribute();
        }

        public void AddMethod(MethodInfo info) => _methods.Add(info);

        public void RemoveMethod(MethodInfo info) => _methods.Remove(info);

        public IEnumerable<MethodInfo> GetMethods() => _methods;

        public void AddSubCommand(CommandGroup cmdG) => _subCommandGroups.Add(cmdG);

        public async Task<CommandResult> Call(DiscordMessage message)
        {

            throw new NotImplementedException();
        }

    }
}
