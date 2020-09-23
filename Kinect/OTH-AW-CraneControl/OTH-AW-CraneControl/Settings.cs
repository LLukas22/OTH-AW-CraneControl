using System.IO;
using Newtonsoft.Json;

namespace CraneControl
{
    public class Settings
    {
        private const string filename = "Settings.json";

        public int Port { get; set; }
        public float Acceleration { get; set; }

        public void Store()
        {
            using (var sw = new StreamWriter(filename))
            {
                sw.WriteLine(JsonConvert.SerializeObject(this));
            }
        }

        public Settings Load()
        {
            if (File.Exists(filename))
                using (var sw = new StreamReader(filename))
                {
                    return JsonConvert.DeserializeObject<Settings>(sw.ReadToEnd());
                }

            return new Settings
            {
                Acceleration = 1,
                Port = 54000,
            };
        }
    }
}