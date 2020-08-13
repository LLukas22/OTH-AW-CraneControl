using System.IO;
using Newtonsoft.Json;

namespace TensorflowInstallationScript.ApplicationSettings
{
	public class SettingsManager
	{
		private readonly string filename = "Settings.json";

		public Settings Load()
		{
			if (File.Exists(filename))
				using (var sw = new StreamReader(filename))
				{
					return JsonConvert.DeserializeObject<Settings>(sw.ReadToEnd());
				}

			return new Settings();
		}

		public void Save(Settings settings)
		{
			using (var sw = new StreamWriter(filename))
			{
				sw.WriteLine(JsonConvert.SerializeObject(settings));
			}
		}
	}
}