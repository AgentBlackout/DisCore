using System.Collections.Generic;
using System.Threading.Tasks;
using DisCore.Shared.Modules;

namespace DisCore.Runner.Loaders.Module
{
    public interface IModuleLoader
    {
        IEnumerable<DllModule> GetModules();

        Task<LoadResult> LoadModule(string path);

    }

}
