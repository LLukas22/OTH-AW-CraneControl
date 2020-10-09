using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace TensorflowInstallationScript.Scripts
{
	public class Pipeline : BaseScript
	{
		public Pipeline(Paths paths, List<string> Objects)
		{
			ScriptName = "customPipeline.config";
			Script = ReturnCustomPipeline(paths, Objects);
		}

		private string ReturnCustomPipeline(Paths paths, List<string> Objects)
		{
			var input = File.ReadAllText(Path.Combine(paths.ObjectDetection, @"net/pipeline.config"));
			var list = input.Split('\n').ToList();

			if (list.Any(x => x.Contains("num_classes:")))
				list[list.IndexOf(list.FirstOrDefault(x => x.Contains("num_classes:")))] =
					$"num_classes: {Objects.Count}";


			if (list.Any(x => x.Contains("fine_tune_checkpoint:")))
				list[list.IndexOf(list.FirstOrDefault(x => x.Contains("fine_tune_checkpoint:")))]
					= $"fine_tune_checkpoint: {CleanUpPath(Path.Combine(paths.ObjectDetection, @"net/checkpoint/ckpt-0"))}";

			if (list.Any(x => x.Contains("label_map_path:")))
			{
				list[list.IndexOf(list.FirstOrDefault(x => x.Contains("label_map_path:")))]
					= $"label_map_path: {CleanUpPath(Path.Combine(paths.Training, @"labelmap.pbtxt"))}";

				list[list.IndexOf(list.LastOrDefault(x => x.Contains("label_map_path:")))]
					= $"label_map_path: {CleanUpPath(Path.Combine(paths.Training, @"labelmap.pbtxt"))}";
			}

			if (list.Any(x => x.Contains("input_path:")))
			{
				list[list.IndexOf(list.FirstOrDefault(x => x.Contains("input_path:")))]
					= $"input_path: {CleanUpPath(Path.Combine(paths.ObjectDetection, @"train.record"))}";

				list[list.IndexOf(list.LastOrDefault(x => x.Contains("input_path:")))]
					= $"input_path: {CleanUpPath(Path.Combine(paths.ObjectDetection, @"test.record"))}";
			}

			if (list.Any(x => x.Contains("num_examples:")))
				list[list.IndexOf(list.FirstOrDefault(x => x.Contains("num_examples:")))]
					= $"num_examples: {CountFiles(Path.Combine(paths.Images, @"test"))}";

			if (list.Any(x => x.Contains("batch_size:")))
				list[list.IndexOf(list.FirstOrDefault(x => x.Contains("batch_size:")))]
					= "batch_size: 10";
			if (list.Any(x => x.Contains("fine_tune_checkpoint_type:")))
				list[list.IndexOf(list.FirstOrDefault(x => x.Contains("fine_tune_checkpoint_type:")))]
					= "fine_tune_checkpoint_type: \"detection\"";

			if (list.Any(x => x.Contains("batch_norm_trainable:")))
				list.RemoveAt(list.IndexOf(list.FirstOrDefault(x => x.Contains("batch_norm_trainable:"))));


			var output = string.Empty;
			foreach (var item in list) output += item + "\n";
			return output;
		}

		private string CleanUpPath(string input)
		{
			return '"' + input.Replace(@"\", "/") + '"';
		}

		private int CountFiles(string path)
		{
			var jpg = Directory.GetFiles(path, "*.jpg").Length;
			var jpeg = Directory.GetFiles(path, "*.jpeg").Length;
			var png = Directory.GetFiles(path, "*.png").Length;
			var PNG = Directory.GetFiles(path, "*.PNG").Length;

			return jpg + jpeg + png + PNG;
		}
	}
}