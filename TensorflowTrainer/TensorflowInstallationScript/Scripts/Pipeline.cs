using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace TensorflowInstallationScript.Scripts
{
	public class Pipeline : BaseScript
	{
		public Pipeline(DirectoryPaths directoryPaths, List<string> Objects)
		{
			ScriptName = "customPipeline.config";
			Script = ReturnCustomPipeline(directoryPaths, Objects);
		}

		private string ReturnCustomPipeline(DirectoryPaths directoryPaths, List<string> Objects)
		{
			var input = File.ReadAllText(Path.Combine(directoryPaths.ObjectDetection, @"net/pipeline.config"));
			var list = input.Split('\n').ToList();

			if (list.Any(x => x.Contains("num_classes:")))
				list[list.IndexOf(list.FirstOrDefault(x => x.Contains("num_classes:")))] =
					$"num_classes: {Objects.Count}";


			if (list.Any(x => x.Contains("fine_tune_checkpoint:")))
				list[list.IndexOf(list.FirstOrDefault(x => x.Contains("fine_tune_checkpoint:")))]
					//= $"fine_tune_checkpoint: {CleanUpPath(Path.Combine(directoryPaths.ObjectDetection, @"net/model.ckpt"))}";
					= $"fine_tune_checkpoint: {CleanUpPath(Path.Combine(directoryPaths.ObjectDetection, @"net/checkpoint/ckpt-0"))}";

			if (list.Any(x => x.Contains("label_map_path:")))
			{
				list[list.IndexOf(list.FirstOrDefault(x => x.Contains("label_map_path:")))]
					= $"label_map_path: {CleanUpPath(Path.Combine(directoryPaths.Training, @"labelmap.pbtxt"))}";

				list[list.IndexOf(list.LastOrDefault(x => x.Contains("label_map_path:")))]
					= $"label_map_path: {CleanUpPath(Path.Combine(directoryPaths.Training, @"labelmap.pbtxt"))}";
			}

			if (list.Any(x => x.Contains("input_path:")))
			{
				list[list.IndexOf(list.FirstOrDefault(x => x.Contains("input_path:")))]
					= $"input_path: {CleanUpPath(Path.Combine(directoryPaths.ObjectDetection, @"train.record"))}";

				list[list.IndexOf(list.LastOrDefault(x => x.Contains("input_path:")))]
					= $"input_path: {CleanUpPath(Path.Combine(directoryPaths.ObjectDetection, @"test.record"))}";
			}

			if (list.Any(x => x.Contains("num_examples:")))
				list[list.IndexOf(list.FirstOrDefault(x => x.Contains("num_examples:")))]
					= $"num_examples: {CountFiles(Path.Combine(directoryPaths.Images, @"test"))}";

			if (list.Any(x => x.Contains("batch_size:")))
				list[list.IndexOf(list.FirstOrDefault(x => x.Contains("batch_size:")))]
					= "batch_size: 10";


			if (list.Any(x => x.Contains("batch_norm_trainable:")))
				list.RemoveAt(list.IndexOf(list.FirstOrDefault(x => x.Contains("batch_norm_trainable:"))));

			var batchSizeIndex = list.FindIndex(x => x.Contains("batch_size"));
			var optimizerIndex = list.FindIndex(x => x.Contains("optimizer"));

			if (batchSizeIndex > 0 && optimizerIndex > 0)
				InsertAugemtationOptions(batchSizeIndex, optimizerIndex,list);


			var output = string.Empty;

			foreach (var item in list) output += item + "\n";
			return output;
		}

		private void InsertAugemtationOptions(int batchSizeIndex,int optimizerIndex, List<string> list)
		{
			list.RemoveRange(batchSizeIndex+1, (optimizerIndex-batchSizeIndex)-1);

			List<string> insert = new List<string>()
			{
				"\tdata_augmentation_options {",
				"\t\t random_adjust_brightness  {",
				"\t\t}",
				"\t}",
				"\tdata_augmentation_options {",
				"\t\t random_adjust_contrast  {",
				"\t\t}",
				"\t}",
				"\tdata_augmentation_options {",
				"\t\t ssd_random_crop_pad {",
				"\t\t}",
				"\t}",
			};
			list.InsertRange(batchSizeIndex + 1, insert);


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