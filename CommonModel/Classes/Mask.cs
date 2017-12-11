using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModel.Classes
{
	public struct Mask : IMask
	{
		public double[][] Matrix { get; set; }
		public long Denominator { get; set; }

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

		public Mask(double[][] mask, long denominator)
		{
			Matrix = mask;
			Denominator = denominator;
		}
	}
}
