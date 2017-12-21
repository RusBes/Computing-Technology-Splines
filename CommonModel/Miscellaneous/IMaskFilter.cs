using System.Collections.Generic;
using System.Drawing;

namespace CommonModel.Classes
{
	public interface IMaskFilter
	{
		ImageMatrix Accept(IMaskFilterWorker visitor, ImageMatrix matrix);
	}
}
