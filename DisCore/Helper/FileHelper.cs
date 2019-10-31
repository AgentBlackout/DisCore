using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DisCore.Helper
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
