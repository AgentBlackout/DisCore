using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus.Entities;

namespace DisCore.Core.Commands.Parser
{
    public interface ICommandParser
    {
        List<(Type t, object o)[]> ParseArgs(CommandGroup masterGroup, DiscordMessage message);
    }
}
