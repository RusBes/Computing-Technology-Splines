using System.Collections.Generic;
using CommonModel.TwoDimSplines;

namespace CommonModel.Miscellaneous
{
	public interface IFilterAcceptor<T>
	{
		T Accept(IFilterVisitor<T> visitor);
	}

	public interface IMaskFilter : IFilterAcceptor<ImageMatrix>
	{

	}

	public interface IVectorFilter : IFilterAcceptor<List<double>>
	{
		
	}
}
