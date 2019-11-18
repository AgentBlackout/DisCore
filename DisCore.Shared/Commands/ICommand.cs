using System.Threading.Tasks;

namespace DisCore.Shared.Commands
{
    public interface ICommand
    {

        Task<CommandResult> Usage(CommandContext ctx);
        Task<CommandResult> Summary(CommandContext ctx);

    }
}
