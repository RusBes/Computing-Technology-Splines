namespace CommonModel.TwoDimSplines
{
	public class HaarMask : ComposedMask
	{
		public override ImageMatrix Accept(IFilterVisitor<ImageMatrix> visitor)
		{
			return visitor.Visit(this);
		}
	}
}
