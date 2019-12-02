using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DisCore.Shared.Config.Json
{
    public class JsonConfig : IConfig, IDisposable
    {
        private JObject _rootObject = new JObject();
        private bool _dirty = false;

        private readonly string _configFile;
        private readonly FileStream _fileStream;

        public JsonConfig(string path)
        {
            _configFile = path;
            _fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
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

            var bytes = Encoding.Unicode.GetBytes(json);

            _fileStream.Seek(0, SeekOrigin.Begin);
            await _fileStream.WriteAsync(bytes, 0, bytes.Length);
            _fileStream.Flush();

            _dirty = false;
        }

        public async Task Load()
        {
            if (!File.Exists(_configFile))
                throw new FileNotFoundException(_configFile);

            StringBuilder sb = new StringBuilder();

            byte[] buffer = new byte[0x1000];
            int numRead;
            _fileStream.Seek(0, SeekOrigin.Begin);
            while ((numRead = await _fileStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
            {
                string text = Encoding.Unicode.GetString(buffer, 0, numRead);
                sb.Append(text);
            }
            
            _rootObject = JsonConvert.DeserializeObject<JObject>(sb.ToString());
        }

        public void Dispose()
        {
            _fileStream.Dispose();
        }
    }
}
