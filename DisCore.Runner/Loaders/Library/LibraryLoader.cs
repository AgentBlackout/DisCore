using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using DisCore.Runner.Helpers;
using DisCore.Shared.Logging;

namespace DisCore.Runner.Loaders.Library
{
    public class LibraryLoader : ILibraryLoader
    {
        private readonly AppDomain _domain;

        private readonly ILogHandler _logHandler;

        private readonly List<Assembly> _assemblies;

        public IEnumerable<Assembly> GetLibraries() => _assemblies;
        public async Task<LoadResult> LoadLibrary(string fileLoc)
        {
            //TODO
            throw new NotImplementedException();
        }

        public LibraryLoader(ILogHandler logHandler)
        {
            _logHandler = logHandler ?? throw new ArgumentNullException(nameof(logHandler));

            _assemblies = new List<Assembly>();
            _domain = AppDomain.CurrentDomain;
        }



        private async Task TryLoadLibrary(string fileLoc)
        {
            var fileName = Path.GetFileName(fileLoc);

            await _logHandler.LogDebug($"Loading library {fileName}");

            Assembly assembly = await AssemblyHelper.ReadAndLoad(_domain, fileLoc);

            _assemblies.Add(assembly);
        }

        public async Task<LoadResult> UnloadLibrary(string name)
        {
            throw new NotImplementedException(); //TODO
        }

        public async Task<LoadResult> UnloadLibrary(Assembly a)
        {
            throw new NotImplementedException(); //TODO
        }
    }
}
