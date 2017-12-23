using System.Collections.Generic;

namespace CommonModel.TwoDimSplines
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
