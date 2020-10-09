using System.IO;
using Newtonsoft.Json;

namespace TensorflowInstallationScript.ApplicationSettings
{
	public class SettingsManager
	{
		private readonly string filename = "Settings.json";
		JsonSerializer serializer = new JsonSerializer()
		{
			Formatting = Formatting.Indented
		};
		public Settings Load()
		{
			if (File.Exists(filename))
			{
				try
				{
					using var sr = File.OpenText(filename);
					using JsonReader reader = new JsonTextReader(sr);
					return serializer.Deserialize<Settings>(reader);
				}
				catch
				{
				}
			}

			return new Settings();
		}

		public void Save(Settings settings)
		{
			using var sw = new StreamWriter(filename);
			using JsonWriter writer = new JsonTextWriter(sw);
			serializer.Serialize(writer, settings);
			
		}
	}
}