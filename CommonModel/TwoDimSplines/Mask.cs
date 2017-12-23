using System.Linq;

namespace CommonModel.TwoDimSplines
{
	public class ImageMask : IMaskFilter
	{
		public double[][] Matrix { get; set; }
		//public long Denominator { get; set; }

		public int Length => Matrix?.Length ?? -1;

		public double this[int i, int j]
		{
			get
			{
				return Matrix[i][j];
			}
			set
			{
				Matrix[i][j] = value;
			}
		}

		public ImageMask(double[][] mask, long denominator)
		{
			Matrix = mask.Select(row => row.Select(val => val / denominator).ToArray()).ToArray();
		}

		public ImageMask(double[][] mask)
		{
			Matrix = mask;
		}

		public ImageMatrix Accept(IFilterVisitor<ImageMatrix> visitor)
		{
			return visitor.Visit(this);
		}
	}
}
