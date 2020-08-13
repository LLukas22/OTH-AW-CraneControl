using System.IO;
using System.Xml;

namespace DatasetStretcher
{
	using System;
	using System.Collections.Generic;
	using System.Text;


	public static class CustomXmlWriter
	{
		public static void WriteXml(string path, ImageClassification image,string overwrite = null)
		{
			if(image == null)return;
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.OmitXmlDeclaration = true;
			settings.NewLineOnAttributes = true;

			using (XmlWriter writer = XmlWriter.Create(path, settings))
			{
				writer.WriteStartElement("annotation");
				writer.WriteElementString("folder", "StretchedData");
				writer.WriteElementString("filename", $"{Path.GetFileName(Path.ChangeExtension(path,".jpg"))}");
				writer.WriteElementString("path", Path.ChangeExtension(path, ".jpg"));
				writer.WriteStartElement("source");
				writer.WriteElementString("database", "DatasetStretcher");
				writer.WriteEndElement();
				writer.WriteStartElement("size");
				writer.WriteElementString("width", $"{image.Width}");
				writer.WriteElementString("height", $"{image.Height}");
				writer.WriteElementString("depth", "3");
				writer.WriteEndElement();
				writer.WriteElementString("segmented", "0");
				foreach (var item in image.Boxes)
				{
					writer.WriteStartElement("object");
					writer.WriteElementString("name", string.IsNullOrEmpty(overwrite) ? item.Classifier : overwrite);
					writer.WriteElementString("pose", "Unspecified");
					writer.WriteElementString("truncated", "0");
					writer.WriteElementString("difficult", "0");
					writer.WriteStartElement("bndbox");
					writer.WriteElementString("xmin", $"{item.XMin}");
					writer.WriteElementString("ymin", $"{item.YMin}");
					writer.WriteElementString("xmax", $"{item.XMax}");
					writer.WriteElementString("ymax", $"{item.YMax}");
					writer.WriteEndElement();
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
				writer.Flush();

			}
		}
	}
}
