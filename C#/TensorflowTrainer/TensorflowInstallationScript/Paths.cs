using System.IO;

namespace TensorflowInstallationScript
{
	public class Paths
	{
		public Paths(string rootPath)
		{
			RootPath = rootPath;
		}

		public string RootPath { get; }
		public string Images => Path.Combine(RootPath, @"models\research\object_detection\images");
		public string Training => Path.Combine(RootPath, @"models\research\object_detection\training");
		public string ObjectDetection => Path.Combine(RootPath, @"models\research\object_detection");
		public string Research => Path.Combine(RootPath, @"models\research");
		public string DriveLetter => Path.GetPathRoot(RootPath).Replace(@"\", string.Empty);
	}
}