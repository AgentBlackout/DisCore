using System.Collections.Generic;
using System.Threading.Tasks;
using DisCore.Core.Entities.Modules;

namespace DisCore.Core.Loaders.Module
{
    public interface IModuleLoader
    {
        IEnumerable<DllModule> GetModules();

        Task<LoadResult> LoadModule(string path);

    }

}
