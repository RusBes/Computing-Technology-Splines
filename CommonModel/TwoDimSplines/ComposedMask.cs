using CommonModel.Miscellaneous;

namespace CommonModel.TwoDimSplines
{
	public class ComposedMask : IMaskFilter
	{
		public ImageMask A { get; set; }

		public ImageMask B { get; set; }

		public ImageMask C { get; set; }

		public ImageMask D { get; set; }

		public ComposedMask(ImageMask a, ImageMask b, ImageMask c, ImageMask d)
		{
			A = a;
			B = b;
			C = c;
			D = d;
		}

		public ComposedMask() { }

		public virtual ImageMatrix Accept(IFilterVisitor<ImageMatrix> visitor)
		{
			return visitor.Visit(this);
		}
	}
}
