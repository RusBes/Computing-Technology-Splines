using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModel
{
	public struct PointD
	{
		public double X { get; }
		public double Y { get; }

		public PointD(double x, double y)
		{
			X = x;
			Y = y;
		}
	}
}
