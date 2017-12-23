using System;
using System.Collections.Generic;
using CommonModel.Miscellaneous;
using CommonModel.TwoDimSplines;

namespace CommonModel.OneDimSplines
{

	//public interface IVectorFilter
	//{
	//	List<double> AcceptFilter(IVectorFilterWorker visitor, List<double> data);
	//}

	internal class Vector : List<double>, IVectorFilter
	{
		public List<double> Accept(IFilterVisitor<List<double>> filter)
		{
			throw new NotImplementedException();
		}
	}
}
