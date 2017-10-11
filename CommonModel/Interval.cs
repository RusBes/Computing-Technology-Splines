using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModel
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
