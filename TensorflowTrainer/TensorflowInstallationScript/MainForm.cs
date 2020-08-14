using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Prism.Events;
using TensorflowInstallationScript.ApplicationSettings;
using TensorflowInstallationScript.CommandLineWrapper;
using TensorflowInstallationScript.DataHelpers;
using TensorflowInstallationScript.Scripts;



namespace TensorflowInstallationScript
{
	public partial class MainForm : Form
	{
		private const double TestingToTrainingRatio = 0.20;
		private CommandLineWrapper.CommandLineWrapper cmdWrapper;
		private DirectoryPaths directoryPaths;
		private DownloadManager downloadManager;
		private IEventAggregator eventAggregator;
		private readonly string neuralNetFile = @"Downloads/neuralNet.tar.gz";
		private readonly string objectDetectionFile = @"Downloads/objectDetectionApi.zip";
		private readonly string protocbufFile = @"Downloads/protobuf.zip";
		private readonly ConcurrentBag<Process> runningProcesses = new ConcurrentBag<Process>();
		private Settings settings;
		private readonly SettingsManager settingsManager = new SettingsManager();

		public MainForm()
		{
			InitializeComponent();
			Init();
		}

		public void Subscribe()
		{
			
			eventAggregator.GetEvent<OutputChangedEvent>().Subscribe(RefreshOutputs, ThreadOption.BackgroundThread);
			eventAggregator.GetEvent<ErrorChangedEvent>().Subscribe(RefreshErrors, ThreadOption.BackgroundThread);
			eventAggregator.GetEvent<ProcessCompletedEvent>().Subscribe(RemoveProcess, ThreadOption.BackgroundThread);
			eventAggregator.GetEvent<ProcessStartedEvent>().Subscribe(AddProcess, ThreadOption.BackgroundThread);
		}

		public void Unsubscribe()
		{
			eventAggregator.GetEvent<OutputChangedEvent>().Unsubscribe(RefreshOutputs);
			eventAggregator.GetEvent<ErrorChangedEvent>().Subscribe(RefreshErrors);
			eventAggregator.GetEvent<ProcessCompletedEvent>().Unsubscribe(RemoveProcess);
			eventAggregator.GetEvent<ProcessStartedEvent>().Unsubscribe(AddProcess);
		}

		private void Init()
		{
			eventAggregator = new EventAggregator();
			cmdWrapper = new CommandLineWrapper.CommandLineWrapper(eventAggregator);
			downloadManager = new DownloadManager(eventAggregator);

			Subscribe();


			settings = settingsManager.Load();
			directoryTextBox.Text = settings.Directory;
			objectTextBox.Text = settings.Objects;
			directoryPaths = new DirectoryPaths(settings.Directory);

			string masterHttps = "https://github.com/tensorflow/models/archive/master.zip";
			downloadManager.RegisterFile(objectDetectionFile,
				new Uri(masterHttps), objectDetectionStateLabel);
			downloadManager.RegisterFile(neuralNetFile,
				new Uri(
					"http://download.tensorflow.org/models/object_detection/tf2/20200711/ssd_mobilenet_v2_fpnlite_640x640_coco17_tpu-8.tar.gz"),
				neuralNetLabel);
			downloadManager.RegisterFile(protocbufFile,
				new Uri(
					"https://github.com/protocolbuffers/protobuf/releases/download/v3.11.2/protoc-3.11.2-win64.zip"),
				protobufstate);

			downloadManager.CheckFile(objectDetectionFile);
			downloadManager.CheckFile(neuralNetFile);
			downloadManager.CheckFile(protocbufFile);


			//Combobox init
			NetSelector.DisplayMember = "Name";
			NetSelector.ValueMember = "Id";
			NetSelector.Items.Add(new DropDownMenuItem {Name = "ssd_mobilenet_v2 (fast)", Id = 1});
			NetSelector.SelectedIndex = 0;
		}

		public List<string> SplitObjects()
		{
			return objectTextBox.Text.Trim().Split(';').ToList();
		}

		public void CleanUpProcesses()
		{
			foreach (var item in runningProcesses) item.Close();

			foreach (var Proc in Process.GetProcesses())
				if (Proc.ProcessName.Equals("py.exe") || Proc.ProcessName.Equals("python") ||
				    Proc.ProcessName.Equals("python.exe")) //Process Python?
					Proc.Kill();

		}

		#region Events

		private void AddProcess(Process obj)
		{
			runningProcesses.Add(obj);
		}

		private void RemoveProcess(Process obj)
		{
			if (runningProcesses.Any(x => x == obj)) runningProcesses.TryTake(out obj);
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			settingsManager.Save(settings);
			Unsubscribe();
			CleanUpProcesses();
		}


		private void RefreshOutputs(string obj)
		{
			if (obj == null) return;
			if (InvokeRequired)
				Invoke(new Action<string>(RefreshOutputs), obj);
			else
				STDOUT.AppendText($"{obj}{Environment.NewLine}");
		}

		private void RefreshErrors(string obj)
		{
			if (obj == null) return;
			if (InvokeRequired)
				Invoke(new Action<string>(RefreshErrors), obj);
			else
				STDOUT.AppendText($"{obj}{Environment.NewLine}");
		}

		private void objectTextBox_TextChanged(object sender, EventArgs e)
		{
			settings.Objects = objectTextBox.Text;
		}

		#endregion

		#region ButtonClicks

		private void btn_Cancel_Click(object sender, EventArgs e)
		{
			var i = runningProcesses.Count();
			CleanUpProcesses();

			STDOUT.AppendText(
				$"{Environment.NewLine}-----------------------------------------------------------------");
			STDOUT.AppendText($"{Environment.NewLine}Killed All Processes ({i})");
			STDOUT.AppendText(
				$"{Environment.NewLine}-----------------------------------------------------------------");
		}

		private void btn_InstallPip_Click(object sender, EventArgs e)
		{
			var Commands = new List<string>();

			Commands.Add("python -m pip install --upgrade pip");
			Commands.Add("pip install matplotlib");
			//Commands.Add("pip install numpy==1.16");
			Commands.Add("pip install jupyter");
			Commands.Add("pip install pandas");
			Commands.Add("pip install opencv-python");
			Commands.Add("pip install Cython");
			Commands.Add("pip install pillow");
			Commands.Add("pip install lxml");
			Commands.Add("pip install pip-autoremove");
			
			Commands.Add("pip install git+https://github.com/philferriere/cocoapi.git#subdirectory=PythonAPI");

			if (box_gpuSupport.CheckState == CheckState.Checked)
				//Commands.Add("pip install tensorflow-gpu=1.15.3");
				Commands.Add("pip install tensorflow-gpu");
			else
				//Commands.Add("pip install tensorflow==1.15.3");
				Commands.Add("pip install tensorflow");

			Commands.Add("pip install tf-models-official");
			cmdWrapper.Execute(Commands);
		}

		private void btn_UninstallPip_Click(object sender, EventArgs e)
		{
			var Commands = new List<string>();
			Commands.Add("pip freeze --exclude-editable > requirements.txt ");
			Commands.Add("pip uninstall -r requirements.txt -y");
			cmdWrapper.Execute(Commands);
		}

		private void btn_BrowseDirectory_Click(object sender, EventArgs e)
		{
			var dialogue = new FolderBrowserDialog();
			var result = dialogue.ShowDialog();
			if (result == DialogResult.OK) directoryTextBox.Text = dialogue.SelectedPath;
			settings.Directory = dialogue.SelectedPath;
			directoryPaths = new DirectoryPaths(settings.Directory);
		}

		private void btn_DownloadObjectDetectionAPI_Click(object sender, EventArgs e)
		{
			downloadManager.Download(objectDetectionFile);
		}

		private void btn_DownloadNeuralNet_Click(object sender, EventArgs e)
		{
			var uri = string.Empty;

			//Old Model
			//uri = "http://download.tensorflow.org/models/object_detection/ssd_mobilenet_v2_coco_2018_03_29.tar.gz";
			uri =
				"http://download.tensorflow.org/models/object_detection/tf2/20200711/ssd_mobilenet_v2_fpnlite_640x640_coco17_tpu-8.tar.gz";
			downloadManager.RegisterFile(neuralNetFile, new Uri(uri), neuralNetLabel);
			downloadManager.Download(neuralNetFile);
		}


		private void btn_DownloadProtobuf_Click(object sender, EventArgs e)
		{
			downloadManager.Download(protocbufFile);
		}


		private void btn_Unzip_Click(object sender, EventArgs e)
		{
			new DirectoryCreator(directoryPaths, objectDetectionFile, neuralNetFile, protocbufFile).BuildDirectory(
				settings.Directory);
		}


		private void btn_CompileProtoc_Click(object sender, EventArgs e)
		{
			var protocPath = Path.Combine(Environment.CurrentDirectory, @"Protobuf\bin\protoc.exe");

			var Commands = new List<string>();

			Commands.Add($"{Path.GetPathRoot(directoryTextBox.Text).Replace(@"\", string.Empty)}");

			Commands.Add($"cd {directoryPaths.Research}");

			Commands.Add($"{protocPath} {@".\object_detection\protos\*.proto"} --python_out=.");

			cmdWrapper.Execute(Commands);
		}

		private void btn_InstallObjectDetectionAPI_Click(object sender, EventArgs e)
		{
			var setupPath = directoryPaths.Research;

			var Commands = new List<string>();

			Commands.Add($"{Path.GetPathRoot(directoryTextBox.Text).Replace(@"\", string.Empty)}");

			Commands.Add($"cd {setupPath}");

			Commands.Add($"python setup.py build");

			Commands.Add($"python setup.py install");

			Commands.Add($"cd {Path.Combine(setupPath,"slim")}");

			Commands.Add($"pip install -e .");

			cmdWrapper.Execute(Commands);
		}

		private void btn_DeleteDownloads_Click(object sender, EventArgs e)
		{
			if (File.Exists(objectDetectionFile)) File.Delete(objectDetectionFile);
			if (File.Exists(neuralNetFile)) File.Delete(neuralNetFile);
			if (File.Exists(protocbufFile)) File.Delete(protocbufFile);
			neuralNetLabel.Text = "State: Missing";
			objectDetectionStateLabel.Text = "State: Missing";
			protobufstate.Text = "State: Missing";
		}

		private void btn_Clear_Click(object sender, EventArgs e)
		{
			STDOUT.Clear();
		}


		private void btn_XmlToCsv_Click(object sender, EventArgs e)
		{
			Directory.Delete(Path.Combine(directoryPaths.Images, @"train"), true);
			Directory.Delete(Path.Combine(directoryPaths.Images, @"test"), true);
			Directory.CreateDirectory(Path.Combine(directoryPaths.Images, @"test"));
			Directory.CreateDirectory(Path.Combine(directoryPaths.Images, @"train"));
			Directory.CreateDirectory(Path.Combine(directoryPaths.Images, @"input"));
			var random = new Random();

			Task.Run(() =>
			{
				var images = Directory.GetFiles(Path.Combine(directoryPaths.Images, @"input"), "*.xml").ToList();
				var TestingFilesCount = (int) (images.Count * TestingToTrainingRatio);
				for (var i = 0; i < TestingFilesCount; i++)
				{
					var xmlfile = images.ElementAt(random.Next(0, images.Count - 1));
					var jpgFile = Path.ChangeExtension(xmlfile, "jpg");
					var pngFile = Path.ChangeExtension(xmlfile, "png");
					if (File.Exists(jpgFile))
					{
						File.Copy(xmlfile, Path.Combine(directoryPaths.Images, @"test", Path.GetFileName(xmlfile)));
						File.Copy(jpgFile, Path.Combine(directoryPaths.Images, @"test", Path.GetFileName(jpgFile)));
						images.Remove(xmlfile);
					}

					else if (File.Exists(pngFile))
					{
						File.Copy(xmlfile, Path.Combine(directoryPaths.Images, @"test", Path.GetFileName(xmlfile)));
						File.Copy(pngFile, Path.Combine(directoryPaths.Images, @"test", Path.GetFileName(pngFile)));
						images.Remove(xmlfile);
					}
				}

				for (var i = 0; i < images.Count; i++)
				{
					var xmlfile = images.ElementAt(i);
					var jpgFile = Path.ChangeExtension(xmlfile, "jpg");
					var pngFile = Path.ChangeExtension(xmlfile, "png");
					if (File.Exists(jpgFile))
					{
						File.Copy(xmlfile, Path.Combine(directoryPaths.Images, @"train", Path.GetFileName(xmlfile)));
						File.Copy(jpgFile, Path.Combine(directoryPaths.Images, @"train", Path.GetFileName(jpgFile)));
					}
					else if (File.Exists(pngFile))
					{
						File.Copy(xmlfile, Path.Combine(directoryPaths.Images, @"train", Path.GetFileName(xmlfile)));
						File.Copy(pngFile, Path.Combine(directoryPaths.Images, @"train", Path.GetFileName(pngFile)));
					}
				}
			}).Wait();

			var xml = new Xml_to_Csv(directoryPaths);
			xml.Execute();
		}

		private void btn_tfRecord_Click(object sender, EventArgs e)
		{
			var tfrecords = new GenerateTfRecord(SplitObjects());
			tfrecords.WriteScript(directoryPaths.ObjectDetection);

			var Commands = new List<string>();

			Commands.Add($"{Path.GetPathRoot(directoryTextBox.Text).Replace(@"\", string.Empty)}");

			Commands.Add($"cd {directoryPaths.ObjectDetection}");

			Commands.Add(
				$@"{tfrecords.ScriptName} --csv_input=images\train_labels.csv --image_dir=images\train --output_path=train.record");

			Commands.Add(
				$@"{tfrecords.ScriptName} --csv_input=images\test_labels.csv --image_dir=images\test --output_path=test.record");

			cmdWrapper.Execute(Commands);
		}

		private void btn_labelmap_Click(object sender, EventArgs e)
		{
			Exception exception = null;
			try
			{
				var labelmap = new GenerateLabelmap(SplitObjects());
				labelmap.WriteScript(directoryPaths.Training);
			}
			catch (Exception ex)
			{
				exception = ex;
			}
			finally
			{
				if (exception == null)
					MessageBoxHelper.Info("Created LabelMap!");
				else
					MessageBoxHelper.Error(exception);
			}
		}

		private void btn_Pipeline(object sender, EventArgs e)
		{
			Exception exception = null;
			try
			{
				var pipeline = new Pipeline(directoryPaths, SplitObjects());
				pipeline.WriteScript(directoryPaths.Training);
			}
			catch (Exception ex)
			{
				exception = ex;
			}
			finally
			{
				if (exception == null)
					MessageBoxHelper.Info("Configured Pipline");
				else
					MessageBoxHelper.Error(exception);
			}
		}

		private void btn_Start_Click(object sender, EventArgs e)
		{

			CleanUpProcesses();

			//var train = new Train(directoryPaths);
			//train.WriteScript(directoryPaths.ObjectDetection);
			//MessageBoxHelper.Info($"Wrote Training Script! Please Execute: {Path.Combine(directoryPaths.ObjectDetection, train.ScriptName)}");



			//TODO: Allow Growth in model_main_tf2.py

			//gpus = tf.config.experimental.list_physical_devices('GPU')
			//if gpus:
			//	try:
			//		for gpu in gpus:
			//			tf.config.experimental.set_memory_growth(gpu, True)
			//	except RuntimeError as e:
			//		print(e)

			var Commands = new List<string>();
			Commands.Add($"{Path.GetPathRoot(directoryTextBox.Text).Replace(@"\", string.Empty)}");
			Commands.Add($"cd {directoryPaths.ObjectDetection}");
			Commands.Add($"model_main_tf2.py --model_dir={directoryPaths.Training} --num_train_steps=50000 --sample_1_of_n_eval_examples=1 --pipeline_config_path={Path.Combine(directoryPaths.Training, "customPipeline.config")} --alsologtostderr");
			cmdWrapper.Execute(Commands);
		}


		private void btn_Tesnorboard_Click(object sender, EventArgs e)
		{
			var Commands = new List<string>();
			Commands.Add($"{Path.GetPathRoot(directoryTextBox.Text).Replace(@"\", string.Empty)}");
			Commands.Add($"cd {directoryPaths.ObjectDetection}");
			Commands.Add("tensorboard --logdir=training");

			cmdWrapper.Execute(Commands);
			var psi = new ProcessStartInfo
			{
				FileName = "http://localhost:6006",
				UseShellExecute = true
			};
			Process.Start(psi);
		}

		private void btn_ExportGraph_Click(object sender, EventArgs e)
		{
			var export = new Exportgraph(directoryPaths);
			export.WriteScript(directoryPaths.ObjectDetection);
			var list = Directory.GetFiles(directoryPaths.Training);
			string name = "";
			int lastindex = 0;
			foreach (var file in list)
			{
				if (file.Contains("data-00000-of-00001"))
				{
					var index = int.Parse(file.Substring(file.IndexOf('-')+1,
						file.IndexOf('.', file.IndexOf('.') + 1) - file.IndexOf('-') -1));
					if (lastindex < index)
					{
						name = Path.GetFileName( file.Substring(0, file.IndexOf('.', file.IndexOf('.') + 1)));
						lastindex = index;
					}
				}
			}
			if(Directory.Exists(Path.Combine(directoryPaths.ObjectDetection, "inference_graph")))
				Directory.Delete(Path.Combine(directoryPaths.ObjectDetection, "inference_graph"),true);
			Directory.CreateDirectory(Path.Combine(directoryPaths.ObjectDetection, "inference_graph"));

			var Commands = new List<string>();

			Commands.Add($"{Path.GetPathRoot(directoryTextBox.Text).Replace(@"\", string.Empty)}");

			Commands.Add($"cd {directoryPaths.ObjectDetection}");

			Commands.Add(
				$"{export.ScriptName} --input_type image_tensor --pipeline_config_path training/customPipeline.config --trained_checkpoint_prefix training/{name} --output_directory inference_graph");

			cmdWrapper.Execute(Commands);
		}

		private void btn_ExportLightGraph_Click(object sender, EventArgs e)
		{
			var exportLight = new ExportOptimicedGraph(directoryPaths);
			exportLight.WriteScript(directoryPaths.ObjectDetection);
			var list = Directory.EnumerateFiles(directoryPaths.Training);
			var file = new FileInfo(list.LastOrDefault(x => x.Contains("data-00000-of-00001"))).Name;
			var name = file.Substring(0, file.IndexOf('.', file.IndexOf('.') + 1));

			var Commands = new List<string>();

			Commands.Add($"{Path.GetPathRoot(directoryTextBox.Text).Replace(@"\", string.Empty)}");

			Commands.Add($"cd {directoryPaths.ObjectDetection}");

			Commands.Add(
				$"{exportLight.ScriptName} --pipeline_config_path training/customPipeline.config --trained_checkpoint_prefix training/{name} --output_directory light_graph --add_postprocessing_op True");

			Commands.Add(
				"tflite_convert --graph_def_file=light_graph/tflite_graph.pb --output_file=light_graph/detect.tflite --output_format=TFLITE --input_shapes=1,300,300,3 --input_arrays=normalized_input_image_tensor --output_arrays=TFLite_Detection_PostProcess,TFLite_Detection_PostProcess:1,TFLite_Detection_PostProcess:2,TFLite_Detection_PostProcess:3 --post_training_quantize --inference_type=QUANTIZED_UINT8 --mean_values=128 --std_dev_values=128 --change_concat_input_ranges=false --allow_custom_ops");

			cmdWrapper.Execute(Commands);
		}

		private void btn_WebcamDemo_Click(object sender, EventArgs e)
		{
			var webcam = new Webcam(directoryPaths, SplitObjects());
			webcam.WriteScript(directoryPaths.ObjectDetection);
			var Commands = new List<string>();
			Commands.Add($"{Path.GetPathRoot(directoryTextBox.Text).Replace(@"\", string.Empty)}");
			Commands.Add($"cd {directoryPaths.ObjectDetection}");
			Commands.Add($"{webcam.ScriptName}");

			cmdWrapper.Execute(Commands);
		}


		private void btn_OpenFolderinExplorer_Click(object sender, EventArgs e)
		{
			Process.Start("explorer.exe", directoryPaths.ObjectDetection);
		}


		private void btn_ExportAll_Click(object sender, EventArgs e)
		{
			var dialogue = new FolderBrowserDialog();
			var result = dialogue.ShowDialog();
			if (result == DialogResult.OK)
			{
				if (File.Exists(Path.Combine(directoryPaths.FrozenGraph, "frozen_inference_graph.pb")))
				{
					File.Copy(Path.Combine(directoryPaths.FrozenGraph, "frozen_inference_graph.pb"),Path.Combine(dialogue.SelectedPath, "frozen_inference_graph.pb"),true);
				}

				if (File.Exists(Path.Combine(directoryPaths.LiteGraph, "detect.tflite")))
				{
					File.Copy(Path.Combine(directoryPaths.LiteGraph, "detect.tflite"), Path.Combine(dialogue.SelectedPath, "detect.tflite"), true);
				}

				if (File.Exists(Path.Combine(directoryPaths.Training, "labelmap.pbtxt")))
				{
					File.Copy(Path.Combine(directoryPaths.Training, "labelmap.pbtxt"), Path.Combine(dialogue.SelectedPath, "labelmap.pbtxt"), true);
				}

				if (SplitObjects().Count > 0)
				{
					File.WriteAllLines(Path.Combine(dialogue.SelectedPath, "labelmap.txt"),SplitObjects());
				}
				MessageBoxHelper.Info("Export Complete!");
			}
			
		}

		private void Prerequisits_Label_Click(object sender, EventArgs e)
		{
			MessageBoxHelper.Info("Windows:\n" +
			                      "\t-Python 3.7.X\n" +
			                      "\t-Git\n" +
			                      "\t-Microsoft C++ Build Tool(v14.0 or newer)\n" +
			                      "\t-GPU:\n" +
								  "\t -CUDA 10 for TF1 or CUDA 10.1 for TF2\n" +
			                      "\t -cuDNN(7.6.X) for CUDA 10/10.1"
								  +"To Check Your Installation Run:" +
								  "\t import tensorflow as tf"+
								  "\t assert tf.test.is_gpu_available()"+
								  "\t print(assert tf.test.is_built_with_cuda())"

			);
		}

		#endregion


	}
}