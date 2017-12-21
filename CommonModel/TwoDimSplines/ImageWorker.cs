using System;
using System.Drawing;
using System.Linq;

namespace CommonModel.Classes
{
	public interface IMaskFilterWorker
	{
		ImageMatrix Mult(ImageMask mask, ImageMatrix matrix);
		ImageMatrix Mult(ComposedMask mask, ImageMatrix matrix);
		ImageMatrix Mult(HaarMask mask, ImageMatrix matrix);
	}

	class ImageWorker : IMaskFilterWorker
	{
		private static byte[,,] _scalePixels(Pixel[][] pixels)
		{
			var res = new byte[3, pixels.Length, pixels[0].Length];

			var maxR = pixels.Max(row => row.Max(p => p.R));
			var maxG = pixels.Max(row => row.Max(p => p.G));
			var maxB = pixels.Max(row => row.Max(p => p.B));

			var minR = pixels.Min(row => row.Min(p => p.R));
			var minG = pixels.Min(row => row.Min(p => p.G));
			var minB = pixels.Min(row => row.Min(p => p.B));

			//minR = minG = minB = Math.Min(Math.Min(minR, minG), minB);
			//maxR = maxG = maxB = Math.Max(Math.Max(maxR, maxG), maxB);


			//var maxR = pixels.Max(row => row.Max(p => Math.Abs(p.R)));
			//var maxG = pixels.Max(row => row.Max(p => Math.Abs(p.G)));
			//var maxB = pixels.Max(row => row.Max(p => Math.Abs(p.B)));

			//var minR = pixels.Min(row => row.Min(p => Math.Abs(p.R)));
			//var minG = pixels.Min(row => row.Min(p => Math.Abs(p.G)));
			//var minB = pixels.Min(row => row.Min(p => Math.Abs(p.B)));

			//minR = minG = minB = Math.Min(Math.Min(minR, minG), minB);
			//maxR = maxG = maxB = Math.Max(Math.Max(maxR, maxG), maxB);

			for (int i = 0; i < pixels.Length; i++)
			{
				for (int j = 0; j < pixels[0].Length; j++)
				{
					//res[0, i, j] = (byte)pixels[i][j].R < 127 ? (byte)0 : (byte)255;
					//res[1, i, j] = (byte)pixels[i][j].G < 127 ? (byte)0 : (byte)255;
					//res[2, i, j] = (byte)pixels[i][j].B < 127 ? (byte)0 : (byte)255;

					res[0, i, j] = (byte)((double)(pixels[i][j].R - minR) * 255 / (maxR - minR));
					res[1, i, j] = (byte)((double)(pixels[i][j].G - minG) * 255 / (maxG - minG));
					res[2, i, j] = (byte)((double)(pixels[i][j].B - minB) * 255 / (maxB - minB));

					//res[0, i, j] = (byte)(pixels[i][j].R < 0 ? 0 : pixels[i][j].R > 255 ? 255 : pixels[i][j].R);
					//res[1, i, j] = (byte)(pixels[i][j].G < 0 ? 0 : pixels[i][j].G > 255 ? 255 : pixels[i][j].G);
					//res[2, i, j] = (byte)(pixels[i][j].B < 0 ? 0 : pixels[i][j].B > 255 ? 255 : pixels[i][j].B);
				}
			}

			return res;
		}

		private static byte[,,] _scalePixelsWidthAbs(Pixel[][] pixels)
		{
			var res = new byte[3, pixels.Length, pixels[0].Length];

			var maxR = pixels.Max(row => row.Max(p => Math.Abs(p.R)));
			var maxG = pixels.Max(row => row.Max(p => Math.Abs(p.G)));
			var maxB = pixels.Max(row => row.Max(p => Math.Abs(p.B)));

			var minR = pixels.Min(row => row.Min(p => Math.Abs(p.R)));
			var minG = pixels.Min(row => row.Min(p => Math.Abs(p.G)));
			var minB = pixels.Min(row => row.Min(p => Math.Abs(p.B)));

			//minR = minG = minB = Math.Min(Math.Min(minR, minG), minB);
			//maxR = maxG = maxB = Math.Max(Math.Max(maxR, maxG), maxB);

			for (int i = 0; i < pixels.Length; i++)
			{
				for (int j = 0; j < pixels[0].Length; j++)
				{
					//res[0, i, j] = (byte)pixels[i][j].R < 127 ? (byte)0 : (byte)255;
					//res[1, i, j] = (byte)pixels[i][j].G < 127 ? (byte)0 : (byte)255;
					//res[2, i, j] = (byte)pixels[i][j].B < 127 ? (byte)0 : (byte)255;

					res[0, i, j] = (byte)((double)(pixels[i][j].R - minR) * 255 / (maxR - minR));
					res[1, i, j] = (byte)((double)(pixels[i][j].G - minG) * 255 / (maxG - minG));
					res[2, i, j] = (byte)((double)(pixels[i][j].B - minB) * 255 / (maxB - minB));

					//res[0, i, j] = (byte)(pixels[i][j].R < 0 ? 0 : pixels[i][j].R > 255 ? 255 : pixels[i][j].R);
					//res[1, i, j] = (byte)(pixels[i][j].G < 0 ? 0 : pixels[i][j].G > 255 ? 255 : pixels[i][j].G);
					//res[2, i, j] = (byte)(pixels[i][j].B < 0 ? 0 : pixels[i][j].B > 255 ? 255 : pixels[i][j].B);
				}
			}

			return res;
		}

		private static byte[,,] _cutPixels(Pixel[][] pixels)
		{
			var res = new byte[3, pixels.Length, pixels[0].Length];

			for (int i = 0; i < pixels.Length; i++)
			{
				for (int j = 0; j < pixels[0].Length; j++)
				{
					//res[0, i, j] = (byte)pixels[i][j].R < 127 ? (byte)0 : (byte)255;
					//res[1, i, j] = (byte)pixels[i][j].G < 127 ? (byte)0 : (byte)255;
					//res[2, i, j] = (byte)pixels[i][j].B < 127 ? (byte)0 : (byte)255;

					res[0, i, j] = (byte)(pixels[i][j].R < 0 ? 0 : pixels[i][j].R > 255 ? 255 : pixels[i][j].R);
					res[1, i, j] = (byte)(pixels[i][j].G < 0 ? 0 : pixels[i][j].G > 255 ? 255 : pixels[i][j].G);
					res[2, i, j] = (byte)(pixels[i][j].B < 0 ? 0 : pixels[i][j].B > 255 ? 255 : pixels[i][j].B);
				}
			}

			return res;
		}

		private static byte[,,] _convertImplicitPixelsToByte(Pixel[][] pixels)
		{
			var res = new byte[3, pixels.Length, pixels[0].Length];

			for (int i = 0; i < pixels.Length; i++)
			{
				for (int j = 0; j < pixels[0].Length; j++)
				{
					res[0, i, j] = (byte)(pixels[i][j].R);
					res[1, i, j] = (byte)(pixels[i][j].G);
					res[2, i, j] = (byte)(pixels[i][j].B);
				}
			}

			return res;
		}

		public static Bitmap ImageToBitmap(ImageMatrix image)
		{
			return image.ToBitmap(_convertImplicitPixelsToByte);
		}

		public ImageMatrix Mult(ImageMask mask, ImageMatrix matrix)
		{
			return MultiplyImageToMask(matrix, mask);
		}

		public ImageMatrix Mult(ComposedMask mask, ImageMatrix matrix)
		{
			return MultiplyImageToMask(matrix, mask);
		}

		public ImageMatrix Mult(HaarMask mask, ImageMatrix matrix)
		{
			return MultiplyImageToMask(matrix, mask);
		}

		public ImageMatrix MultiplyImageToMask(ImageMatrix p, ImageMask mask)
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

		public ImageMatrix MultiplyImageToMask(ImageMatrix p, ComposedMask masks)
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

		public ImageMatrix MultiplyImageToMask(ImageMatrix p, HaarMask masks)
		{
			var res = new ImageMatrix(p.Width, p.Height);
			var wid = p.Width / 2;
			var height = p.Height / 2;

			try
			{
				for (int i = 0; i < p.Width / 2; i++)
				{
					for (int j = 0; j < p.Height / 2; j++)
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
			var rightIndent = indent % 2 == 0 ? indent - 1 : indent;
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
