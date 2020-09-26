using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace DatasetStretcher
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			textBoxOutput.Text = @"C:\Users\lkreu\Desktop\Output";
		}

		ConcurrentBag<ImageClassification> Images = new ConcurrentBag<ImageClassification>();

		private void btn_Input_Click(object sender, EventArgs e)
		{
			var dialogue = new FolderBrowserDialog();
			var result = dialogue.ShowDialog();
			if (result == DialogResult.OK) textBoxInput.Text = dialogue.SelectedPath;
		}

		private void btn_Output_Click(object sender, EventArgs e)
		{
			var dialogue = new FolderBrowserDialog();
			var result = dialogue.ShowDialog();
			if (result == DialogResult.OK) textBoxOutput.Text = dialogue.SelectedPath;
		}

		private void btn_Load_Click(object sender, EventArgs e)
		{
			ConcurrentBag<string> PathsToXmls = new ConcurrentBag<string>(Directory.EnumerateFiles(textBoxInput.Text,"*.xml"));

			Parallel.ForEach(PathsToXmls, (xml) =>
			{
				var doc = new XmlDocument();
				doc.Load(xml);

				var Boxes = new List<ClassificationBox>();

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
						Boxes.Add(new ClassificationBox(name, xmin, ymin, xmax, ymax));
					}
				Images.Add(new ImageClassification(filename,height,width,Boxes));
			});
		}

		private string CleanUpFilename(string file)
		{
			{
				if (file.Contains('\\')) return CleanUpFilename(file.Substring(file.IndexOf('\\') + 1));
				return file;
			}
		}

		private void btn_Start_Click(object sender, EventArgs e)
		{
			var overwrite = this.ckb_Classifier.Checked ? txt_Classifier.Text : null;
			Directory.CreateDirectory(textBoxOutput.Text);
			Directory.Delete(textBoxOutput.Text,true);
			Directory.CreateDirectory(textBoxOutput.Text);

			if (grayscale_chbx.Checked)
			{
				Task.Run(() =>
				{
					Parallel.ForEach(Images, (image) =>
					{
						string input = Path.Combine(textBoxInput.Text, image.ImageFile);
						string outputpng = Path.Combine(textBoxOutput.Text, $"Grayscaled_{image.ImageFile}");
						string outputxml = Path.ChangeExtension(outputpng, ".xml");
						using (Bitmap bitmap = (Bitmap)Bitmap.FromFile(input))
						{
							bitmap.MakeGrayscale().Save(Path.Combine(textBoxOutput.Text, $"Grayscaled_{image.ImageFile}"));
						}
						CustomXmlWriter.WriteXml(outputxml, image, overwrite);
					});
				});
			}

			if (whiteNoisechbx.Checked)
			{
				Task.Run(() =>
				{
					Parallel.ForEach(Images, (image) =>
					{


						string input = Path.Combine(textBoxInput.Text, image.ImageFile);
						string outputpng = Path.Combine(textBoxOutput.Text, $"Noise_{image.ImageFile}");
						string outputxml = Path.ChangeExtension(outputpng, ".xml");

						using (Bitmap bitmap =
							(Bitmap)Bitmap.FromFile(input))
						{
							bitmap.GenerateNoise(90).Save(Path.Combine(textBoxOutput.Text, $"Noise_{image.ImageFile}"));
						}

						CustomXmlWriter.WriteXml(outputxml, image, overwrite);
					});
				});
			}

			if (blurchbx.Checked)
			{
				Task.Run(() =>
				{
					Parallel.ForEach(Images, (image) =>
					{


						string input = Path.Combine(textBoxInput.Text, image.ImageFile);
						string outputpng = Path.Combine(textBoxOutput.Text, $"Blur_{image.ImageFile}");
						string outputxml = Path.ChangeExtension(outputpng, ".xml");

						using (Bitmap bitmap =
							(Bitmap)Bitmap.FromFile(input))
						{
							bitmap.Blur(2).Save(Path.Combine(textBoxOutput.Text, $"Blur_{image.ImageFile}"));
						}

						CustomXmlWriter.WriteXml(outputxml, image, overwrite);
					});
				});
			}

			if (MAkro_Chbx.Checked)
			{
				Task.Run(() =>
				{
				
						Parallel.ForEach(Images, (image) =>
						{


							string input = Path.Combine(textBoxInput.Text, image.ImageFile);


							using (Bitmap bitmap = (Bitmap)Bitmap.FromFile(input))
							{
								for (int boxindex = 0; boxindex < image.Boxes.Count; boxindex++)
								{
									string outputpng = Path.Combine(textBoxOutput.Text, $"Makro_Box{boxindex}_{image.ImageFile}");
									string outputxml = Path.ChangeExtension(outputpng, ".xml");
									var activeBox = image.Boxes.ElementAt(boxindex);

									int newXmin = activeBox.XMin - 5;
									int newYmin = activeBox.YMin - 5;
									int newXMax = activeBox.XMax + 5;
									int newYMax = activeBox.YMax + 5;
									if (newXmin > 0 && newYmin > 0 && newXMax < image.Width && newYMax < image.Height)
									{
										try
										{
											var rectangle = new Rectangle(newXmin, newYmin, newXMax - newXmin,
												newYMax - newYmin);
											bitmap.CropImage(rectangle).Save(outputpng);
											CustomXmlWriter.WriteXml(outputxml, new ImageClassification(outputpng,
												rectangle.Height, rectangle.Width, new List<ClassificationBox>()
												{
													new ClassificationBox(activeBox.Classifier, 5, 5,
														rectangle.Width - 5, rectangle.Height - 5)
												}));
										}
										catch (Exception)
										{

										}
										
									}
									
								}
							}
						});
				});
			}


			if (chropchbx.Checked)
			{
				Task.Run(() =>
				{
					for (int i = 1; i <= (int) chropIts.Value; i++)
					{
						Parallel.ForEach(Images, (image) =>
						{


							string input = Path.Combine(textBoxInput.Text, image.ImageFile);
							

							using (Bitmap bitmap = (Bitmap)Bitmap.FromFile(input))
							{
								for(int boxindex = 0;boxindex<image.Boxes.Count;boxindex++)
								{
									string outputpng = Path.Combine(textBoxOutput.Text, $"Chroped{i}_Box{boxindex}_{image.ImageFile}");
									string outputxml = Path.ChangeExtension(outputpng, ".xml");
									var activeBox = image.Boxes.ElementAt(boxindex);
									var croppedImageClassification = CropImage(activeBox, image, bitmap, outputpng);
									CustomXmlWriter.WriteXml(outputxml, croppedImageClassification);
								}
							}

							
						});
					}
					
				});
			}



		}

		private ImageClassification CropImage(ClassificationBox activeBox, ImageClassification image, Bitmap bitmap,string filename)
		{
			var random = new Random();
			int newXmin = random.Next(0, ForceRange(activeBox.XMin - 5));
			int newYmin = random.Next(0, ForceRange(activeBox.YMin - 5));
			int newXMax = random.Next(ForceRange(activeBox.XMax + 5, 0, image.Width), image.Width);
			int newYMax = random.Next(ForceRange(activeBox.YMax + 5, 0, image.Height), image.Height);
			var rectangle = new Rectangle(newXmin, newYmin,newXMax-newXmin, newYMax-newYmin);

			var boxes = TransformBox(rectangle, image);
			if (boxes.Count > 1)
			{
				Exception exception = null;
				try
				{
					bitmap.CropImage(rectangle).Save(filename);
					return new ImageClassification(filename, rectangle.Height, rectangle.Width, boxes);

				}
				catch (Exception exc)
				{
					return null;
				}
				
				
			}
			return null;
		}

		private List<ClassificationBox> TransformBox(Rectangle rectangle, ImageClassification image)
		{
			List<ClassificationBox> transformedBoxes = new List<ClassificationBox>();
			int xminOffset = rectangle.X;
			int yminOffset = rectangle.Y;


			foreach (var box in image.Boxes)
			{

				int newBoxXmin = box.XMin - xminOffset;
				int newBoxYmin = box.YMin - yminOffset;
				int newBoxXMax = newBoxXmin + (box.XMax - box.XMin);
				int newBoxYMax = newBoxYmin + (box.YMax - box.YMin);

				//Check if box is in bounds 
				if ((newBoxXmin > 0) && (newBoxYmin > 0) &&
				    (newBoxXMax < rectangle.Width && newBoxXMax > newBoxXmin) &&
				    (newBoxYMax < rectangle.Height && newBoxYMax > newBoxYmin))
				{
					transformedBoxes.Add(new ClassificationBox(box.Classifier,newBoxXmin,newBoxYmin,newBoxXMax,newBoxYMax ));
				}
			}

			return transformedBoxes;
		}


		private int ForceRange(int value, int minrange = 0, int maxrange = int.MaxValue)
		{
			if (value < minrange) return minrange;
			if (value > maxrange) return maxrange;
			return value;
		}
	}
}
