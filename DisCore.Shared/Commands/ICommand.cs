using System.Threading.Tasks;

namespace DisCore.Shared.Commands
{
    public interface ICommand
    {

        Task<string> Usage();
        Task<string> Summary();

    }
}
