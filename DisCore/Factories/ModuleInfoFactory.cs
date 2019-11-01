using DisCore.Core.Module;

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
