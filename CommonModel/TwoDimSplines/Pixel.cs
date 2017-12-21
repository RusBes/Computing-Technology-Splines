namespace CommonModel.Classes
{
	public struct Pixel
	{
		public int R { get; set; }
		public int G { get; set; }
		public int B { get; set; }

		public Pixel(int r, int g, int b)
		{
			R = r;
			G = g;
			B = b;
		}
	}
}
