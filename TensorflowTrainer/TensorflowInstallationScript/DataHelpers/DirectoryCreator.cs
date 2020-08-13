using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Forms;

namespace TensorflowInstallationScript.DataHelpers
{
	public class DirectoryCreator
	{
		public DirectoryCreator(DirectoryPaths directoryPaths, string objectdetectionFile, string neuralNetFile,
			string protbufFile)
		{
			DirectoryPaths = directoryPaths;
			ObjectdetectionFile = objectdetectionFile;
			NeuralNetFile = neuralNetFile;
			ProtbufFile = protbufFile;
		}

		public DirectoryPaths DirectoryPaths { get; }
		public string ObjectdetectionFile { get; }
		public string NeuralNetFile { get; }
		public string ProtbufFile { get; }

		public void BuildDirectory(string directory)
		{
			Exception exception = null;
			try
			{
				if (string.IsNullOrEmpty(directory))
				{
					MessageBox.Show("Directory is not Valid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				Directory.Delete(directory, true);

				ZipFile.ExtractToDirectory(ObjectdetectionFile, directory);
				Directory.Move(Path.Combine(directory, "models-master"),
					Path.Combine(directory, "models"));


				//Check if tar.gz or just tar 
				using (var fs = new FileStream(NeuralNetFile, FileMode.Open, FileAccess.Read))
				{
					var buffer = new byte[2];
					fs.Read(buffer, 0, buffer.Length);
					fs.Seek(0, SeekOrigin.Begin);
					if (buffer[0] == 0x1F
					    && buffer[1] == 0x8B)
						Tar.ExtractTarGz(fs, DirectoryPaths.ObjectDetection);
					else
						Tar.ExtractTar(fs, DirectoryPaths.ObjectDetection);
					var dirinfo = new DirectoryInfo(DirectoryPaths.ObjectDetection);
					var filesInDir = dirinfo.GetDirectories("*" + "net" + "*.*");
					if (filesInDir.Any())
					{
						Directory.Move(filesInDir[0].FullName, Path.Combine(DirectoryPaths.ObjectDetection, "net"));
					}
					
				}


				Directory.CreateDirectory(@"Protobuf");
				Directory.Delete(@"Protobuf", true);
				ZipFile.ExtractToDirectory(ProtbufFile, @"Protobuf");


				Directory.CreateDirectory(DirectoryPaths.Images);
				Directory.CreateDirectory(Path.Combine(DirectoryPaths.Images, @"test"));
				Directory.CreateDirectory(Path.Combine(DirectoryPaths.Images, @"train"));
				Directory.CreateDirectory(Path.Combine(DirectoryPaths.Images, @"input"));
				Directory.CreateDirectory(DirectoryPaths.Training);
				Directory.CreateDirectory(Path.Combine(DirectoryPaths.ObjectDetection, @"light_graph"));
			}
			catch (Exception ex)
			{
				exception = ex;
			}
			finally
			{
				if (exception == null)
					MessageBoxHelper.Info("Sucessfully copied files!");
				else
					MessageBoxHelper.Error(exception);
			}
		}
	}
}