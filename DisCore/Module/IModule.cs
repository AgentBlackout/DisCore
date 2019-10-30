using System;
using System.Threading.Tasks;

namespace DisCore.Module
{
    public interface IModule
    {
        String Name { get; }
        String Version { get; }
        String Author { get; }

        Task OnLoad(CorePlaceholder placeholder);

        Task OnUnload();
    }
}
