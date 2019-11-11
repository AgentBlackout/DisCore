using Newtonsoft.Json;

namespace DisCore.Helpers
{
    public static class JsonHelper
    {
        public static string ToJson(object o)
        {
            return JsonConvert.SerializeObject(o,
                new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
        }

        public static T FromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

    }
}
