﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DisCore.Api.Commands;
using DisCore.Api.Commands.Timeout;
using DisCore.Api.Logging;
using DisCore.Api.Permissions;
using DisCore.Core;
using DisCore.Core.Commands;

namespace DisCore.Factories
{
    public static class CommandGroupFactory
    {
        public static async Task<IEnumerable<CommandGroup>> GetCommandGroups(Assembly assembly, ILogHandler log)
        {
            var commands = new List<CommandGroup>();
            var types = assembly.GetTypes().ToList();
            foreach (Type t in types)
            {
                if (!t.GetInterfaces().Contains(typeof(ICommand)) || t.IsInterface || t.IsAbstract)
                    continue;

                await log.LogDebug($"Checking type {t} for valid commands.");

                var attrib = (CommandAttribute)t.GetCustomAttribute(typeof(CommandAttribute));
                if (attrib == null)
                {
                    await log.LogWarning(
                        $"Class {t.FullName} implements ICommand but doesn't have a Command attribute. Ignoring...");
                    continue;
                }

                ICommand c = (ICommand)Activator.CreateInstance(t);

                var commandAttribute = (CommandAttribute)t.GetCustomAttribute(typeof(CommandAttribute));

                var permAttribute = (RequiredPermissions)t.GetCustomAttribute(typeof(RequiredPermissions));
                var timeoutAttrib = (TimeoutAttribute)t.GetCustomAttribute(typeof(TimeoutAttribute));

                var cmdGroup = new CommandGroup(attrib.Name, c.Usage, c.Summary, commandAttribute, permAttribute, timeoutAttrib);

                foreach (var mInfo in t.GetMethods())
                    if (mInfo.ReturnType == typeof(Task<CommandResult>))
                        cmdGroup.AddCommand(CommandFactory.GetCommand(mInfo));

                commands.Add(cmdGroup);
            }

            var finalCommands = new Dictionary<String, CommandGroup>();

            foreach (var cmdGroup in commands.Where(item => !item.IsSubGroup))
            {
                finalCommands.Add(cmdGroup.CommandName, cmdGroup);
                await log.LogDebug(
                    $"Registered command \"{cmdGroup.CommandName}\"");
            }

            foreach (var cmdGroup in commands)
            {
                if (!cmdGroup.IsSubGroup)
                    continue;

                var parent = cmdGroup.CommandAttribute.Parent;
                var cmdAttribute = (CommandAttribute)parent.GetCustomAttribute(typeof(CommandAttribute));
                if (cmdAttribute == null)
                {
                    await log.LogWarning(
                        $"SubCommand {cmdGroup.GetType().FullName} references parent class which has no command attribute. Ignoring");
                    continue;
                }

                var parentName = cmdAttribute.Name;

                if (finalCommands.TryGetValue(parentName, out var parentGroup))
                {
                    parentGroup.AddSubCommand(cmdGroup);
                    await log.LogDebug(
                        $"Registered subcommand \"{cmdGroup.CommandName}\" to \"{parentGroup.CommandName}\"");
                }
                else
                {
                    await log.LogWarning(
                        $"SubCommand {cmdGroup.GetType().FullName} references parent class {parent.GetType().FullName} which is not registered. Make sure parent class implements ICommand and has Command attribute.");
                    continue;
                }
            }

            return finalCommands.Values;
        }
    }
}
