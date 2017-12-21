using System;
using System.Collections.Generic;

namespace CommonModel.OneDimSplines
{
	public interface IVectorFilterWorker
	{
		List<double> ApplyFilter(IVectorFilter filter, List<double> data);
	}

	class SplineFilterWorker : IVectorFilterWorker
	{
		public List<double> ApplyFilter(IVectorFilter filter, List<double> data)
		{
			throw new NotImplementedException();
		}
	}
}
