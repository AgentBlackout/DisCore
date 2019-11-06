using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus.Entities;

namespace DisCore.Core.Commands.Parser
{
    public interface ICommandParser
    {
        (Command, CommandParameter[]) GetCommandAndParameters(CommandGroup masterGroup, DiscordMessage message);
    }
}
