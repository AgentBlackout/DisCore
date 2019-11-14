using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DisCore.Core.Logging;
using DisCore.Helpers;

namespace DisCore.Core.Loaders.Library
{
    public class LibraryLoader : ILibraryLoader
    {
        private readonly AppDomain _domain;

        private readonly ILogHandler _logHandler;

        private readonly string _configLocation;

        private readonly List<Assembly> _assemblies;

        public LibraryLoader(string location, ILogHandler logHandler)
        {
            _configLocation = location;
            _logHandler = _logHandler ?? throw new ArgumentNullException(nameof(_logHandler));

            _assemblies = new List<Assembly>();
            _domain = AppDomain.CurrentDomain;
        }

        public IEnumerable<Assembly> GetLibraries() => _assemblies;

        public async Task LoadLibrary(string fileLoc)
        {
            var fileName = Path.GetFileName(fileLoc);

            await _logHandler.LogDebug($"Loading library {fileName}");

            Assembly assembly = await AssemblyHelper.ReadAndLoad(fileLoc);

            _assemblies.Add(assembly);
        }

        public async Task LoadLibraries()
        {
            IEnumerable<string> dllLocations = FileHelper.GetDLLs("./libraries");
            foreach (var fileLoc in dllLocations)
            {
                try
                {
                    await LoadLibrary(fileLoc);
                }
                catch (Exception e)
                {
                    await _logHandler.LogWarning(
                        $"Failed to load library {fileLoc} - {e.Message}");
                }

                await _logHandler.LogInfo($"Loaded {fileLoc} successfully");
            }
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
