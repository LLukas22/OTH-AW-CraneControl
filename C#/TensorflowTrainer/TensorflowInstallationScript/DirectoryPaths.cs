using System.IO;

namespace TensorflowInstallationScript
{
	public class DirectoryPaths
	{
		public DirectoryPaths(string rootPath)
		{
			RootPath = rootPath;
		}

		public string RootPath { get; set; }
		public string Images => Path.Combine(RootPath, @"models\research\object_detection\images");
		public string Training => Path.Combine(RootPath, @"models\research\object_detection\training");
		public string ObjectDetection => Path.Combine(RootPath, @"models\research\object_detection");
		public string Research => Path.Combine(RootPath, @"models\research");
		public string FrozenGraph => Path.Combine(ObjectDetection, @"inference_graph");
		public string LiteGraph => Path.Combine(ObjectDetection, @"light_graph");
	}
}