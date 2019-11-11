using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DisCore.Core.Config
{
    public class DConfig : IConfig
    {
        private JObject rootObject = new JObject();
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
                if (rootObject.TryGetValue(key, out var res))
                    return res.ToObject<T>();
                return default;
            });
        }

        public Task Remove(string key)
        {
            _dirty = true;
            return Task.Run(() =>
            {
                if (rootObject.TryGetValue(key, out _))
                    rootObject.Remove(key);

            });
        }

        public async Task Set(string key, object val)
        {
            _dirty = true;
            await Remove(key);
            await Task.Run(() =>
            {
                rootObject.Add(key, JToken.FromObject(val));
            });
        }

        public async Task Save()
        {
            if (!_dirty) //Don't write it if it's not dirty
                return;

            var json = JsonConvert.SerializeObject(rootObject, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                /*typeNameHandling = TypeNameHandling.All*/
            });
            await File.WriteAllTextAsync(_configFile, json);
            _dirty = false;
        }

        public async Task Load()
        {
            var content = await File.ReadAllTextAsync(_configFile);
            rootObject = JsonConvert.DeserializeObject<JObject>(content);
        }
    }
}
