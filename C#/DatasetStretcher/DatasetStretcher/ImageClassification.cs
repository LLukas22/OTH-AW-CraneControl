using System.Collections.Concurrent;

namespace DatasetStretcher
{
	using System;
	using System.Collections.Generic;
	using System.Text;


	public class ImageClassification
	{
		public ConcurrentBag<ClassificationBox> Boxes { get; }
		public string ImageFile { get; }
		public int Height { get; }
		public int Width { get; }

		public ImageClassification(string ImageFile,int Height,int width,IEnumerable<ClassificationBox> boxes)
		{
			this.ImageFile = ImageFile;
			this.Height = Height;
			this.Width = width;
			this.Boxes = new ConcurrentBag<ClassificationBox>(boxes);
		}
	}
}
