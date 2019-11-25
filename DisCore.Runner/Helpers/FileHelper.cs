using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DisCore.Runner.Helpers
{
    class FileHelper
    {
        public static IEnumerable<string> GetDLLs(string path)
        {
            if (!Directory.Exists(path))
                return Enumerable.Empty<string>();

            var files = Directory.GetFiles(path, "*.dll");

            return files.Select(Path.GetFullPath);

        }
    }
}
