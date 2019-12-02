using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DisCore.Shared.Config;
using Newtonsoft.Json.Linq;

namespace DisCore.Shared.Config.Json
{
    public sealed class JsonSubConfig : IConfig
    {
        private readonly IConfig _parent;
        private readonly string _rootKey;
        private readonly JObject _rootObject;

        public JsonSubConfig(IConfig parent, string rootKey, JObject jObject)
        {
            _parent = parent ?? throw new ArgumentNullException(nameof(parent));
            _rootKey = rootKey;

            _rootObject = jObject ?? throw new ArgumentNullException(nameof(jObject));
        }

        public Task Load() => Task.CompletedTask;

        public async Task Save() => await _parent.Save();

        public Task<T> Get<T>(string key)
        {
            return Task.Run(() => _rootObject.TryGetValue(key, out var res) ? res.ToObject<T>() : default);
        }

        public Task Remove(string key)
        {
            return Task.Run(() =>
            {
                if (_rootObject.TryGetValue(key, out _))
                    _rootObject.Remove(key);

            });
        }

        public async Task Set(string key, object val)
        {
            await Remove(key);
            _rootObject.Add(key, JToken.FromObject(val));
            await _parent.Set(_rootKey, _rootObject);
        }
    }
}
