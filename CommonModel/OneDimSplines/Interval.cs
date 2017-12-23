namespace CommonModel.OneDimSplines
{
	public struct Interval
	{
		public int Left { get; }
		public int Right { get; }

		public Interval(int left, int right)
		{
			Left = left;
			Right = right;
		}
	}
}
