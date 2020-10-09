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
		
		private CommandLineWrapper.CommandLineWrapper cmdWrapper;
		private DownloadManager downloadManager;
		private IEventAggregator eventAggregator;
		private readonly string neuralNetFile = @"Downloads/neuralNet.tar.gz";
		private readonly string objectDetectionFile = @"Downloads/objectDetectionApi.zip";
		private readonly string protbufFile = @"Downloads/protobuf.zip";
		private readonly ConcurrentDictionary<int,Process> runningProcesses = new ConcurrentDictionary<int, Process>();
		public Settings Settings;
		private readonly SettingsManager settingsManager = new SettingsManager();

		public MainForm()
		{
			InitializeComponent();
			Init();
		}

		public void Subscribe()
		{
			
			eventAggregator.GetEvent<OutputChangedEvent>().Subscribe(RefreshOutputs, ThreadOption.UIThread);
			eventAggregator.GetEvent<ErrorChangedEvent>().Subscribe(RefreshOutputs, ThreadOption.UIThread);
			eventAggregator.GetEvent<ProcessCompletedEvent>().Subscribe(RemoveProcess, ThreadOption.UIThread);
			eventAggregator.GetEvent<ProcessStartedEvent>().Subscribe(AddProcess, ThreadOption.UIThread);
		}

		public void Unsubscribe()
		{
			eventAggregator.GetEvent<OutputChangedEvent>().Unsubscribe(RefreshOutputs);
			eventAggregator.GetEvent<ErrorChangedEvent>().Unsubscribe(RefreshOutputs);
			eventAggregator.GetEvent<ProcessCompletedEvent>().Unsubscribe(RemoveProcess);
			eventAggregator.GetEvent<ProcessStartedEvent>().Unsubscribe(AddProcess);
		}

		private void Init()
		{
			eventAggregator = new EventAggregator();
			cmdWrapper = new CommandLineWrapper.CommandLineWrapper(eventAggregator);
			downloadManager = new DownloadManager(eventAggregator);
			Subscribe();
			Settings = settingsManager.Load();
			Settings.RegisterDirectoryTextBox(directoryTextBox).RegisterObjectsToDetectTextBox(objectTextBox).Initialize();
			downloadManager.RegisterFile(objectDetectionFile, new Uri(Settings.ApiUrl), objectDetectionStateLabel);
			downloadManager.RegisterFile(neuralNetFile, new Uri(Settings.ModelUrl), neuralNetLabel);
			downloadManager.RegisterFile(protbufFile, new Uri(Settings.ProtbufUrl), protobufLabel);
			downloadManager.CheckFile(objectDetectionFile);
			downloadManager.CheckFile(neuralNetFile);
			downloadManager.CheckFile(protbufFile);
		}

		public void CleanUpProcesses()
		{
			foreach (var item in runningProcesses)
			{
				item.Value.Close();
			}
			runningProcesses.Clear();
		}

		#region Events

		private void AddProcess(Process obj)
		{
			
			runningProcesses.TryAdd(obj.Id,obj);
		}

		private void RemoveProcess(Process obj)
		{
			if (runningProcesses.ContainsKey(obj.Id))
			{
				runningProcesses.TryRemove(obj.Id, out _);
			}
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			settingsManager.Save(Settings);
			Unsubscribe();
			CleanUpProcesses();
		}


		private void RefreshOutputs(string obj)
		{
			if (obj == null) return;
			STDOUT.AppendText($"{obj}{Environment.NewLine}");
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
			Commands.Add("pip install jupyter");
			Commands.Add("pip install pandas");
			Commands.Add("pip install opencv-python");
			Commands.Add("pip install Cython");
			Commands.Add("pip install pillow");
			Commands.Add("pip install lxml");
			Commands.Add("pip install git+https://github.com/philferriere/cocoapi.git#subdirectory=PythonAPI");

			if (box_gpuSupport.CheckState == CheckState.Checked)
				Commands.Add("pip install tensorflow-gpu");
			else
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
			if (result == DialogResult.OK)
			{
				directoryTextBox.Text = dialogue.SelectedPath;
				Settings.MainDirectory = dialogue.SelectedPath;
				Settings.Initialize();
			}
			
		}

		private void btn_DownloadObjectDetectionAPI_Click(object sender, EventArgs e)
		{
			downloadManager.Download(objectDetectionFile);
		}

		private void btn_DownloadNeuralNet_Click(object sender, EventArgs e)
		{
			downloadManager.Download(neuralNetFile);
		}


		private void btn_DownloadProtobuf_Click(object sender, EventArgs e)
		{
			downloadManager.Download(protbufFile);
		}


		private void btn_Unzip_Click(object sender, EventArgs e)
		{
			new DirectoryCreator(Settings.Directories, objectDetectionFile, neuralNetFile, protbufFile).BuildDirectory(
				Settings.MainDirectory);
		}


		private void btn_CompileProtoc_Click(object sender, EventArgs e)
		{
			var protocPath = Path.Combine(Environment.CurrentDirectory, @"Protobuf\bin\protoc.exe");

			var Commands = new List<string>();

			Commands.Add($"{Settings.Directories.DriveLetter}");

			Commands.Add($"cd {Settings.Directories.Research}");

			Commands.Add($"{protocPath} {@".\object_detection\protos\*.proto"} --python_out=.");

			cmdWrapper.Execute(Commands);
		}

		private void btn_InstallObjectDetectionAPI_Click(object sender, EventArgs e)
		{
			var setupPath = Settings.Directories.Research;

			var Commands = new List<string>();

			Commands.Add($"{Path.GetPathRoot(directoryTextBox.Text).Replace(@"\", string.Empty)}");

			Commands.Add($"cd {setupPath}");

			Commands.Add($"cp {Path.Combine(Settings.Directories.ObjectDetection, @"packages\tf2","setup.py")}");
				
			Commands.Add($"python setup.py build");

			Commands.Add($"python setup.py install");

			//TF1 only
			//Commands.Add($"cd {Path.Combine(setupPath,"slim")}");
			//Commands.Add($"pip install -e .");

			Commands.Add($"python object_detection/builders/model_builder_tf2_test.py");

			cmdWrapper.Execute(Commands);
		}

		private void btn_DeleteDownloads_Click(object sender, EventArgs e)
		{
			if (File.Exists(objectDetectionFile)) File.Delete(objectDetectionFile);
			if (File.Exists(neuralNetFile)) File.Delete(neuralNetFile);
			if (File.Exists(protbufFile)) File.Delete(protbufFile);
			neuralNetLabel.Text = "State: Missing";
			objectDetectionStateLabel.Text = "State: Missing";
			protobufLabel.Text = "State: Missing";
		}

		private void btn_Clear_Click(object sender, EventArgs e)
		{
			STDOUT.Clear();
		}


		private void btn_XmlToCsv_Click(object sender, EventArgs e)
		{
			Directory.Delete(Path.Combine(Settings.Directories.Images, @"train"), true);
			Directory.Delete(Path.Combine(Settings.Directories.Images, @"test"), true);
			Directory.CreateDirectory(Path.Combine(Settings.Directories.Images, @"test"));
			Directory.CreateDirectory(Path.Combine(Settings.Directories.Images, @"train"));
			Directory.CreateDirectory(Path.Combine(Settings.Directories.Images, @"input"));
			var random = new Random();

			Task.Run(() =>
			{
				var images = Directory.GetFiles(Path.Combine(Settings.Directories.Images, @"input"), "*.xml").ToList();
				var TestingFilesCount = (int) (images.Count * Settings.TrainToEvaluateRatio);
				for (var i = 0; i < TestingFilesCount; i++)
				{
					var xmlfile = images.ElementAt(random.Next(0, images.Count - 1));
					var jpgFile = Path.ChangeExtension(xmlfile, "jpg");
					var pngFile = Path.ChangeExtension(xmlfile, "png");
					if (File.Exists(jpgFile))
					{
						File.Copy(xmlfile, Path.Combine(Settings.Directories.Images, @"test", Path.GetFileName(xmlfile)));
						File.Copy(jpgFile, Path.Combine(Settings.Directories.Images, @"test", Path.GetFileName(jpgFile)));
						images.Remove(xmlfile);
					}

					else if (File.Exists(pngFile))
					{
						File.Copy(xmlfile, Path.Combine(Settings.Directories.Images, @"test", Path.GetFileName(xmlfile)));
						File.Copy(pngFile, Path.Combine(Settings.Directories.Images, @"test", Path.GetFileName(pngFile)));
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
						File.Copy(xmlfile, Path.Combine(Settings.Directories.Images, @"train", Path.GetFileName(xmlfile)));
						File.Copy(jpgFile, Path.Combine(Settings.Directories.Images, @"train", Path.GetFileName(jpgFile)));
					}
					else if (File.Exists(pngFile))
					{
						File.Copy(xmlfile, Path.Combine(Settings.Directories.Images, @"train", Path.GetFileName(xmlfile)));
						File.Copy(pngFile, Path.Combine(Settings.Directories.Images, @"train", Path.GetFileName(pngFile)));
					}
				}
			}).Wait();

			var xml = new Xml_to_Csv(Settings.Directories);
			xml.Execute();
		}

		private void btn_tfRecord_Click(object sender, EventArgs e)
		{
			var tfrecords = new GenerateTfRecord(Settings.SplitObjectsToDetect);
			tfrecords.WriteScript(Settings.Directories.ObjectDetection);

			var Commands = new List<string>();

			Commands.Add($"{Settings.Directories.DriveLetter}");

			Commands.Add($"cd {Settings.Directories.ObjectDetection}");

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
				var labelmap = new GenerateLabelmap(Settings.SplitObjectsToDetect);
				labelmap.WriteScript(Settings.Directories.Training);
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
				var pipeline = new Pipeline(Settings.Directories, Settings.SplitObjectsToDetect);
				pipeline.WriteScript(Settings.Directories.Training);
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
			//TODO: Allow Growth in model_main_tf2.py

			//gpus = tf.config.experimental.list_physical_devices('GPU')
			//if gpus:
			//	try:
			//		for gpu in gpus:
			//			tf.config.experimental.set_memory_growth(gpu, True)
			//	except RuntimeError as e:
			//		print(e)

			var Commands = new List<string>();
			Commands.Add($"{Settings.Directories.DriveLetter}");
			Commands.Add($"cd {Settings.Directories.ObjectDetection}");
			Commands.Add($"model_main_tf2.py --model_dir={Settings.Directories.Training} --num_train_steps=50000 --sample_1_of_n_eval_examples=1 --pipeline_config_path={Path.Combine(Settings.Directories.Training, "customPipeline.config")} --alsologtostderr");
			cmdWrapper.Execute(Commands);
		}


		private void btn_Tesnorboard_Click(object sender, EventArgs e)
		{
			var Commands = new List<string>();
			Commands.Add($"{Settings.Directories.DriveLetter}");
			Commands.Add($"cd {Settings.Directories.ObjectDetection}");
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
			var Commands = new List<string>();
			Commands.Add($"{Settings.Directories.DriveLetter}");
			Commands.Add($"cd {Settings.Directories.ObjectDetection}");
			Commands.Add($"python exporter_main_v2.py --input_type image_tensor --pipeline_config_path training/customPipeline.config --trained_checkpoint_dir training --output_directory export/normal");
			cmdWrapper.Execute(Commands);
		}

		private void btn_ExportLiteGraph_Click(object sender, EventArgs e)
		{
			var Commands = new List<string>();
			Commands.Add($"{Settings.Directories.DriveLetter}");
			Commands.Add($"cd {Settings.Directories.ObjectDetection}");
			Commands.Add($"python export_tflite_graph_tf2.py --pipeline_config_path training/customPipeline.config --trained_checkpoint_dir training --output_directory  export/LiteConvertibleGraph");
			cmdWrapper.Execute(Commands);
		}

		private void btn_WebcamDemo_Click(object sender, EventArgs e)
		{
			var webcam = new Webcam();
			webcam.WriteScript(Settings.Directories.ObjectDetection);
			var Commands = new List<string>();
			Commands.Add($"{Settings.Directories.DriveLetter}");
			Commands.Add($"cd {Settings.Directories.ObjectDetection}");
			Commands.Add($"python webcam.py");
			cmdWrapper.Execute(Commands);
		}


		private void btn_OpenFolderinExplorer_Click(object sender, EventArgs e)
		{
			Process.Start("explorer.exe", Settings.Directories.ObjectDetection);
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