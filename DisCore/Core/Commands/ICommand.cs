using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DisCore.Core.Entities.Commands;

namespace DisCore.Core.Commands
{
    public interface ICommand
    {

        Task<CommandResult> Usage(CommandContext ctx);
        Task<CommandResult> Summary(CommandContext ctx);

    }
}
