using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using DisCore.Api.Logging;
using DisCore.Helpers;

namespace DisCore.Core.Loaders.Library
{
    public class LibraryLoader : ILibraryLoader
    {
        private readonly AppDomain _domain;

        private readonly ILogHandler _logHandler;

        private readonly string _configLocation;

        private readonly List<Assembly> _assemblies;

        public IEnumerable<Assembly> GetLibraries() => _assemblies;
        public async Task<LoadResult> LoadLibrary(string fileLoc)
        {
            throw new NotImplementedException();
        }

        async Task<LoadResult> ILibraryLoader.UnloadLibrary(string name)
        {
            throw new NotImplementedException();
        }

        async Task<LoadResult> ILibraryLoader.UnloadLibrary(Assembly a)
        {
            throw new NotImplementedException();
        }

        public LibraryLoader(string location, ILogHandler logHandler)
        {
            _configLocation = location;
            _logHandler = logHandler ?? throw new ArgumentNullException(nameof(logHandler));

            _assemblies = new List<Assembly>();
            _domain = AppDomain.CurrentDomain;
        }



        private async Task TryLoadLibrary(string fileLoc)
        {
            var fileName = Path.GetFileName(fileLoc);

            await _logHandler.LogDebug($"Loading library {fileName}");

            Assembly assembly = await AssemblyHelper.ReadAndLoad(fileLoc);

            _assemblies.Add(assembly);
        }

        public async Task UnloadLibrary(string name)
        {
            throw new NotImplementedException();
        }

        public async Task UnloadLibrary(Assembly a)
        {
            throw new NotImplementedException();
        }
    }
}
