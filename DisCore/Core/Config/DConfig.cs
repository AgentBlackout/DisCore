using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DisCore.Core.Config
{
    public class DConfig : IConfig
    {
        private Dictionary<string, object> _dictionary = new Dictionary<string, object>();
        private bool _dirty = false;

        private readonly string _configFile;

        public DConfig(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(path);

            _configFile = path;
        }

        public Task<T> Get<T>(string key)
        {
            return Task.Run(() =>
            {
                if (_dictionary.TryGetValue(key, out var val))
                    return (T)val;
                return default;
            });
        }

        public Task Remove(string key)
        {
            return Task.Run(() =>
            {
                if (_dictionary.TryGetValue(key, out _))
                    _dictionary.Remove(key);
                
            });
        }

        public Task Set(string key, object val)
        {
            return Task.Run(() =>
            {
                if (_dictionary.TryGetValue(key, out var _))
                    _dictionary.Remove(key);

                _dictionary.Add(key, val);
            });
        }

        public async Task Save()
        {
            if (!_dirty) //Don't write it if it's not dirty
                return;

            var content = await File.ReadAllTextAsync(_configFile);
            _dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
        }

        public async Task Load()
        {
            var json = JsonConvert.SerializeObject(_dictionary, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            await File.WriteAllTextAsync(_configFile, json);
        }
    }
}
