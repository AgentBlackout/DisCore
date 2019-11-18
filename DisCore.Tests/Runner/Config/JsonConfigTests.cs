using System.IO;
using System.Threading.Tasks;
using DisCore.Shared.Config.Json;
using Xunit;

namespace DisCore.Tests.Runner.Config
{
    public static class JsonConfigTests
    {
        private static async Task<JsonConfig> PrepareConfig(string loc, string key, object data)
        {
            if (File.Exists(loc))
                File.Delete(loc);

            JsonConfig conf = new JsonConfig(loc);
            await conf.Set(key, data);
            await conf.Save();

            return conf;
        }

        [Fact]
        public static async void JsonConfig_IntegrityTest_String()
        {
            const string fileLoc = "./integrity.json";

            const string exampleData = @"<html>\n\n\rwasdwasd123456@🤣<@123>";
            const string key = "exampleKey";

            var conf = await PrepareConfig(fileLoc, key, exampleData);

            await conf.Load();
            string output = await conf.Get<string>(key);

            Assert.Equal(exampleData, output);
        }

        [Fact]
        public static async void JsonConfig_IntegrityTest_Long()
        {
            const string fileLoc = "./integrity.json";

            const long exampleData = long.MaxValue;
            const string key = "exampleKey";

            var conf = await PrepareConfig(fileLoc, key, exampleData);

            await conf.Load();
            long output = await conf.Get<long>(key);

            Assert.Equal(exampleData, output);
        }

        [Fact]
        public static async void JsonConfig_IntegrityTest_LongArray()
        {

            const string fileLoc = "./integrity.json";

            long[] exampleData = new long[] { long.MaxValue, long.MinValue };

            const string key = "exampleKey";

            var conf = await PrepareConfig(fileLoc, key, exampleData);

            await conf.Load();
            long[] output = await conf.Get<long[]>(key);

            Assert.Equal(exampleData, output);
        }

    }
}
