using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace DisCore.Core.Loaders.Library
{
    public interface ILibraryLoader
    {
        IEnumerable<Assembly> GetLibraries();

        Task LoadLibrary(string fileLoc);
        Task LoadLibraries();

        Task UnloadLibrary(string name);
        Task UnloadLibrary(Assembly a);
    }
}
