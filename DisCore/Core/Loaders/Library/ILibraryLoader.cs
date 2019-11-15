using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace DisCore.Core.Loaders.Library
{

    public interface ILibraryLoader
    {
        IEnumerable<Assembly> GetLibraries();

        Task<LoadResult> LoadLibrary(string fileLoc);

        Task<LoadResult> UnloadLibrary(string name);
        Task<LoadResult> UnloadLibrary(Assembly a);
    }
}
