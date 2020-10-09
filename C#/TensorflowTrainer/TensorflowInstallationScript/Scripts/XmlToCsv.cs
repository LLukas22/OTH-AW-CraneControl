using System;
using System.IO;
using System.Text;
using System.Xml;

namespace TensorflowInstallationScript.Scripts
{
	public class Xml_to_Csv : BaseScript
	{
		private readonly Paths paths;

		public Xml_to_Csv(Paths paths)
		{
			this.paths = paths;
		}

		public void Execute()
		{
			Exception exception = null;
			try
			{
				GenerateCSV(Path.Combine(paths.Images, @"test"), paths.Images, "test_labels.csv");
				GenerateCSV(Path.Combine(paths.Images, @"train"), paths.Images, "train_labels.csv");
			}
			catch (Exception ex)
			{
				exception = ex;
			}
			finally
			{
				if (exception == null)
					MessageBoxHelper.Info("Created test_labels.csv and train_labels.csv");

				else
					MessageBoxHelper.Error(exception);
			}
		}

		private void GenerateCSV(string xmlPath, string exportDirectory, string file)
		{
			var testxml = Directory.GetFiles(xmlPath, "*.xml");

			var csv = new StringBuilder();
			csv.Append("filename,width,height,class,xmin,ymin,xmax,ymax\n");

			foreach (var xml in testxml)
			{
				var doc = new XmlDocument();
				doc.Load(xml);


				var name = string.Empty;
				var xmin = int.MinValue;
				var ymin = int.MinValue;
				var xmax = int.MaxValue;
				var ymax = int.MaxValue;


				var filename = CleanUpFilename(doc.DocumentElement.SelectSingleNode("/annotation/filename").InnerText);

				var width = int.Parse(doc.DocumentElement.SelectSingleNode("/annotation/size/width").InnerText);

				var height = int.Parse(doc.DocumentElement.SelectSingleNode("/annotation/size/height").InnerText);

				foreach (XmlNode node in doc.DocumentElement.ChildNodes)
					if (node.Name == "object")
					{
						foreach (XmlNode child in node)
						{
							if (child.Name == "name") name = child.InnerText;

							if (child.Name == "bndbox")
								foreach (XmlNode cord in child)
								{
									if (cord.Name == "xmin") xmin = int.Parse(cord.InnerText);

									if (cord.Name == "ymin") ymin = int.Parse(cord.InnerText);

									if (cord.Name == "xmax") xmax = int.Parse(cord.InnerText);

									if (cord.Name == "ymax") ymax = int.Parse(cord.InnerText);
								}
						}

						var newLine = $"{filename},{width},{height},{name},{xmin},{ymin},{xmax},{ymax}\n";
						csv.Append(newLine);
					}
			}

			File.WriteAllText(Path.Combine(exportDirectory, file), csv.ToString());
		}

		private string CleanUpFilename(string file)
		{
			{
				if (file.Contains('\\')) return CleanUpFilename(file.Substring(file.IndexOf('\\') + 1));
				return file;
			}
		}
	}
}