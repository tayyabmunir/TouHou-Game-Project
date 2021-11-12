using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace CrazyFour.Core.Helpers
{
    public class ConfigReader
    {
        public String ConfigFile = "./config.json";

        public Config conf = new Config();

        public ConfigReader() { }

        public Config ReadJson()
        {
            Config config;
            
            using (StreamReader r = new StreamReader(ConfigFile))
            {
                string json = r.ReadToEnd();
                config = JsonConvert.DeserializeObject<Config>(json);
            }

            return config;
        }
    }
}
