using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModel.Classes
{
	struct ComposedMask : IMask
	{
		public Mask A { get; set; }

		public Mask B { get; set; }

		public Mask C { get; set; }

		public Mask D { get; set; }

		public ComposedMask(Mask a, Mask b, Mask c, Mask d)
		{
			A = a;
			B = b;
			C = c;
			D = d;
		}
	}
}
