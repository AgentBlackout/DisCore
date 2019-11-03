using DisCore.Core.Permissions;

namespace DisCore.Core
{
    public class DisCoreRoot
    {
        public static DisCoreRoot Singleton { get; private set; }

        public IPermissionManager PermManager;

        public DisCoreRoot()
        {
            Singleton = this;

        }

    }
}
