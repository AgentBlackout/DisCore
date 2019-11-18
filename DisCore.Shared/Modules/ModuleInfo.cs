namespace DisCore.Shared.Modules
{
    public class ModuleInfo
    {
        public string Name { get; }
        public string Version { get; }
        public string Author { get; }

        public ModuleInfo(string name, string version, string author)
        {
            Name = name;
            Version = version;
            Author = author;
        }

    }
}
