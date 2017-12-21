using System.Collections.Generic;
using CommonModel.Classes;

namespace CommonModel.OneDimSplines
{

	public interface IVectorFilter
	{
		List<double> AcceptFilter(IVectorFilterWorker visitor, List<double> data);
	}

	internal class Vector : List<double>, IVectorFilter
	{
		public List<double> AcceptFilter(IVectorFilterWorker visitor, List<double> data)
		{
			return visitor.ApplyFilter(this, data);
		}
	}
}
