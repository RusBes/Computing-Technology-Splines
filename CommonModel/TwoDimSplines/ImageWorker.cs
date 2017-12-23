using System;
using System.Drawing;
using System.Linq;

namespace CommonModel.TwoDimSplines
{
	public interface IFilterVisitor<out T>
	{
		T Visit(ImageMask mask);
		T Visit(ComposedMask mask);
		T Visit(HaarMask mask);
	}

	class ImageVisitor : IFilterVisitor<ImageMatrix>
	{
		private readonly ImageMatrix _image;

		public ImageVisitor(ImageMatrix image)
		{
			_image = image;
		}
		
		public ImageMatrix Visit(ImageMask mask)
		{
			return MultiplyImageToMask(_image, mask);
		}

		public ImageMatrix Visit(ComposedMask mask)
		{
			return MultiplyImageToMask(_image, mask);
		}

		public ImageMatrix Visit(HaarMask mask)
		{
			return MultiplyImageToMask(_image, mask);
		}

		private ImageMatrix MultiplyImageToMask(ImageMatrix p, ImageMask mask)
		{
			//var indent = mask.Length / 2;
			var res = new ImageMatrix(p.Width, p.Height);
			try
			{
				for (int i = 0/*indent*/; i < p.Width /*- indent*/; i++)
				{
					for (int j = 0/*indent*/; j < p.Height /*- indent*/; j++)
					{
						res[i, j] = ApplyFilterToPixel(i, j, p, mask);
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Помилка при проведенні згортки:\n\n" + ex.Message);
			}

			return res;
		}

		private ImageMatrix MultiplyImageToMask(ImageMatrix p, ComposedMask masks)
		{
			var res = new ImageMatrix(p.Width * 2, p.Height * 2);
			try
			{
				for (int i = 0; i < p.Width; i++)
				{
					for (int j = 0; j < p.Height; j++)
					{
						res[2 * i, 2 * j] = ApplyFilterToPixel(i, j, p, masks.A);
						res[2 * i + 1, 2 * j] = ApplyFilterToPixel(i, j, p, masks.B);
						res[2 * i, 2 * j + 1] = ApplyFilterToPixel(i, j, p, masks.C);
						res[2 * i + 1, 2 * j + 1] = ApplyFilterToPixel(i, j, p, masks.D);
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Помилка при проведенні згортки:\n\n" + ex.Message);
			}

			return res;
		}

		private ImageMatrix MultiplyImageToMask(ImageMatrix p, HaarMask masks)
		{
			var res = new ImageMatrix(p.Width, p.Height);
			var wid = p.Width / 2;
			var height = p.Height / 2;
			int i, j;
			try
			{
				for (i = 0; i < p.Width / 2; i++)
				{
					for (j = 0; j < p.Height / 2; j++)
					{
						res[i, j] = ApplyFilterToPixel(2 * i, 2 * j, p, masks.A);
						res[i + wid, j] = ApplyFilterToPixel(2 * i + 1, 2 * j, p, masks.B);
						res[i, j + height] = ApplyFilterToPixel(2 * i, 2 * j + 1, p, masks.C);
						res[i + wid, j + height] = ApplyFilterToPixel(2 * i + 1, 2 * j + 1, p, masks.D);
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Помилка при проведенні згортки:\n\n" + ex.Message);
			}

			return res;
		}

		private Pixel ApplyFilterToPixel(int i, int j, ImageMatrix p, ImageMask mask)
		{
			var indent = mask.Length / 2;
			// for even dimension mask rightIndent should be smaller by 1
			var rightIndent = mask.Length % 2 == 0 ? indent - 1 : indent;
			double r = 0, g = 0, b = 0;
			for (int ii = i - indent; ii <= i + rightIndent; ii++)
			{
				for (int jj = j - indent; jj <= j + rightIndent; jj++)
				{
					// to remove black borders
					var iInd = ii < 0 ? 0 : ii > p.Width - 1 ? p.Width - 1 : ii;
					var jInd = jj < 0 ? 0 : jj > p.Height - 1 ? p.Height - 1 : jj;
					//r += Convert.ToInt16((mask[ii - i + indent, jj - j + indent] * P[ii, jj].R) / mask.Denominator);
					//g += Convert.ToInt16((mask[ii - i + indent, jj - j + indent] * P[ii, jj].G) / mask.Denominator);
					//b += Convert.ToInt16((mask[ii - i + indent, jj - j + indent] * P[ii, jj].B) / mask.Denominator);

					r += mask[ii - i + indent, jj - j + indent] * p[iInd, jInd].R;
					g += mask[ii - i + indent, jj - j + indent] * p[iInd, jInd].G;
					b += mask[ii - i + indent, jj - j + indent] * p[iInd, jInd].B;

					//r += ((mask[ii - i + indent, jj - j + indent] * P[ii, jj].R) / mask.Denominator);
					//g += ((mask[ii - i + indent, jj - j + indent] * P[ii, jj].G) / mask.Denominator);
					//b += ((mask[ii - i + indent, jj - j + indent] * P[ii, jj].B) / mask.Denominator);
				}
			}
			//if (r > 1 || g > 1 || b > 1)
			//{
			//  return new Pixel(255, 255, 255);
			//}
			//else
			{
				//  return new Pixel(0, 0, 0);
				return new Pixel(Convert.ToInt16(r), Convert.ToInt16(g), Convert.ToInt16(b));
			}

			//return new Pixel(_getByteInCorrectBorders((int)r), _getByteInCorrectBorders((int)g), _getByteInCorrectBorders((int)b));

			//return new Pixel(Convert.ToByte((int)r), Convert.ToByte((int)g), Convert.ToByte((int)b));
		}
		//private Pixel ApplyFilterToPixelForHaar(int i, int j, ImageMatrix p, Mask mask)
		//{
		//	var indent = mask.Length / 2;
		//	double r = 0, g = 0, b = 0;
		//	for (int ii = i - indent; ii < i + indent; ii++)
		//	{
		//		for (int jj = j - indent; jj < j + indent; jj++)
		//		{
		//			// to remove black borders
		//			var iInd = ii < 0 ? 0 : ii > p.Width - 1 ? p.Width - 1 : ii;
		//			var jInd = jj < 0 ? 0 : jj > p.Height - 1 ? p.Height - 1 : jj;

		//			r += mask[ii - i + indent, jj - j + indent] * p[iInd, jInd].R;
		//			g += mask[ii - i + indent, jj - j + indent] * p[iInd, jInd].G;
		//			b += mask[ii - i + indent, jj - j + indent] * p[iInd, jInd].B;
		//		}
		//	}
		//	return new Pixel(Convert.ToInt16(r), Convert.ToInt16(g), Convert.ToInt16(b));

		//}
	}
}
