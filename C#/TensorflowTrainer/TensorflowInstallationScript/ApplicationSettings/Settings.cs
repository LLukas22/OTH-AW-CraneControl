using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace TensorflowInstallationScript.ApplicationSettings
{
	public class Settings
	{
		[JsonIgnore]
		public Paths Directories { get; private set; }
		public string MainDirectory { get; set; }
		public string ObjectsToDetect { get; set; }
		public float TrainToEvaluateRatio { get; set; } = 0.20f;
		[JsonIgnore]
		public List<string> SplitObjectsToDetect => ObjectsToDetect.Trim().Split(';').ToList();

		public string ModelUrl { get; set; } =
			"http://download.tensorflow.org/models/object_detection/tf2/20200711/faster_rcnn_resnet50_v1_640x640_coco17_tpu-8.tar.gz";

		public string ApiUrl { get; set; } = "https://github.com/tensorflow/models/archive/master.zip";

		public string ProtbufUrl { get; set; } =
			"https://github.com/protocolbuffers/protobuf/releases/download/v3.11.2/protoc-3.11.2-win64.zip";

		public Settings Initialize()
		{
			Directories = new Paths(MainDirectory);
			DirectoryTextBox.Text = MainDirectory;
			ObjectsToDetectTextBox.Text = ObjectsToDetect;
			return this;
		}
		[JsonIgnore]
		public TextBox DirectoryTextBox;
		public Settings RegisterDirectoryTextBox(TextBox textBox)
		{
			DirectoryTextBox = textBox;
			DirectoryTextBox.TextChanged += (sender, args) =>
			{
				MainDirectory = DirectoryTextBox.Text;
				Directories = new Paths(MainDirectory);
			};
			return this;
		}
		[JsonIgnore]
		public TextBox ObjectsToDetectTextBox;
		public Settings RegisterObjectsToDetectTextBox(TextBox textBox)
		{
			ObjectsToDetectTextBox = textBox;
			ObjectsToDetectTextBox.TextChanged += (sender, args) => ObjectsToDetect = ObjectsToDetectTextBox.Text;
			return this;
		}


	}
}