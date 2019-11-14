using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DisCore.Helpers
{
    class FileHelper
    {
        public static IEnumerable<string> GetDLLs(string path)
        {
            var files = Directory.GetFiles(path, "*.dll");

            return files.Select(Path.GetFullPath);

        }
    }
}
