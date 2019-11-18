using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace DisCore.Runner.Loaders.Library
{

    public interface ILibraryLoader
    {
        IEnumerable<Assembly> GetLibraries();

        Task<LoadResult> LoadLibrary(string fileLoc);

        Task<LoadResult> UnloadLibrary(string name);
        Task<LoadResult> UnloadLibrary(Assembly a);
    }
}
