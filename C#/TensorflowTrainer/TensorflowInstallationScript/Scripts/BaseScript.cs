using System.IO;

namespace TensorflowInstallationScript.Scripts
{
	public abstract class BaseScript
	{
		public string Script;

		public string ScriptName;


		public void WriteScript(string directory)
		{
			using (var sw = new StreamWriter(Path.Combine(directory, ScriptName)))
			{
				sw.WriteLine(Script);
			}
		}
	}
}