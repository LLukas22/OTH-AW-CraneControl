namespace DatasetStretcher
{

	public class ClassificationBox
	{
		public string Classifier { get; }
		public int XMin { get; }
		public int YMin { get; }
		public int XMax { get; }
		public int YMax { get; }

		public ClassificationBox(string Classifier,int xMin, int yMin,int xMax, int yMax)
		{
			this.Classifier = Classifier;
			XMin = xMin;
			YMin = yMin;
			XMax = xMax;
			YMax = yMax;
		}

	}
}
