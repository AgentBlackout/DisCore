using System;
using System.Threading.Tasks;

namespace DisCore.Core.Module
{
    public interface IModule
    {
        String Name { get; }
        String Version { get; }
        String Author { get; }

        String Summary { get; }

        Task OnLoad(CorePlaceholder placeholder);

        Task OnUnload();
    }
}
