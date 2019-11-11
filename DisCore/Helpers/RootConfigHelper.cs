using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DisCore.Core.Config;

namespace DisCore.Helpers
{
    public static class RootConfigHelper
    {
        public static async Task InitConfig(String filepath)
        {
            if (!File.Exists(filepath))
                File.Create(filepath);

            DConfig conf =  new DConfig(filepath);
            await conf.Set("token", "YOUR-AUTH-TOKEN-HERE");
            await conf.Set("owners", new List<long>() {123, 456});

            await conf.Save();
        }
    }
}
