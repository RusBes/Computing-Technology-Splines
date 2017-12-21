using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CommonModel.Classes
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

		public ImageMatrix Accept(IMaskFilterWorker visitor, ImageMatrix matrix)
		{
			return visitor.Mult(this, matrix);
		}
	}
}
