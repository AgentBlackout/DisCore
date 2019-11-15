using DisCore.Api.Module;
using DisCore.Core.Entities.Modules;
using DisCore.Core.Loaders.Module;

namespace DisCore.Factories
{
    public static class ModuleInfoFactory
    {
        public static ModuleInfo GetModuleInfo(IModule module)
        {
            return new ModuleInfo(module.Name, module.Version, module.Author);
        }

    }
}
