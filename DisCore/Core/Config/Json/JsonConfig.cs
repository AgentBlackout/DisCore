using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DisCore.Core.Config.Json
{
    public class JsonConfig : IConfig
    {
        private JObject _rootObject = new JObject();
        private bool _dirty = false;

        private readonly string _configFile;

        public JsonConfig(string path)
        {
            _configFile = path;
        }

        public Task<T> Get<T>(string key)
        {
            return Task.Run(() => _rootObject.TryGetValue(key, out var res) ? res.ToObject<T>() : default);
        }

        public Task Remove(string key)
        {
            _dirty = true;
            return Task.Run(() =>
            {
                if (_rootObject.TryGetValue(key, out _))
                    _rootObject.Remove(key);

            });
        }

        public async Task Set(string key, object val)
        {
            _dirty = true;
            await Remove(key);
            await Task.Run(() =>
            {
                _rootObject.Add(key, JToken.FromObject(val));
            });
        }

        public async Task Save()
        {
            if (!_dirty) //Don't write it if it's not dirty
                return;

            var json = JsonConvert.SerializeObject(_rootObject, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                /*typeNameHandling = TypeNameHandling.All*/
            });
            await File.WriteAllTextAsync(_configFile, json);
            _dirty = false;
        }

        public async Task Load()
        {
            if (!File.Exists(_configFile))
                throw new FileNotFoundException(_configFile);

            var content = await File.ReadAllTextAsync(_configFile);
            _rootObject = JsonConvert.DeserializeObject<JObject>(content);
        }
    }
}
