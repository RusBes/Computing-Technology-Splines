using System;
using System.Drawing;
using System.Drawing.Imaging;
using CommonModel.Miscellaneous;

namespace CommonModel.TwoDimSplines
{
	public class ImageMatrix
	{
		private readonly Pixel[][] _pixels;

		public int Width => _pixels.Length;

		public int Height => _pixels[0].Length;

		public Pixel this[int i, int j]
		{
			get
			{
				return _pixels[i][j];
			}
			set
			{
				_pixels[i][j] = value;
			}
		}

		public ImageMatrix(Bitmap bmp) : this(bmp.Width, bmp.Height)
		{
			var pixels = BitmapToByteRgbQ(bmp);
			for (int i = 0; i < Width; i++)
			{
				for (int j = 0; j < Height; j++)
				{
					_pixels[j][i] = new Pixel(pixels[0, i, j], pixels[1, i, j], pixels[2, i, j]);
				}
			}
		}
		public ImageMatrix(byte[,,] pixels) : this(pixels.GetLength(1), pixels.GetLength(2))
		{
			for (int i = 0; i < Width; i++)
			{
				for (int j = 0; j < Height; j++)
				{
					_pixels[j][i] = new Pixel(pixels[0, i, j], pixels[1, i, j], pixels[2, i, j]);
				}
			}
		}
		public ImageMatrix(int width, int height)
		{
			_pixels = new Pixel[width][];
			for (int j = 0; j < width; j++)
			{
				_pixels[j] = new Pixel[height];
			}
		}

		private unsafe byte[,,] BitmapToByteRgbQ(Bitmap bmp)
		{
			int width = bmp.Width,
				height = bmp.Height;
			byte[,,] res = new byte[3, height, width];
			BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly,
				PixelFormat.Format24bppRgb);
			try
			{
				byte* curpos;
				fixed (byte* _res = res)
				{
					byte* _r = _res, _g = _res + width * height, _b = _res + 2 * width * height;
					for (int h = 0; h < height; h++)
					{
						curpos = ((byte*)bd.Scan0) + h * bd.Stride;
						for (int w = 0; w < width; w++)
						{
							*_b = *(curpos++);
							++_b;
							*_g = *(curpos++);
							++_g;
							*_r = *(curpos++);
							++_r;
						}
					}
				}
			}
			finally
			{
				bmp.UnlockBits(bd);
			}
			return res;
		}

		public ImageMatrix ApplyFilter(IMaskFilter mask)
		{
			return mask.Accept(new ImageVisitor(this));
		}

		public Bitmap ToBitmap(Func<Pixel[][], byte[,,]> scaleFunc = null)
		{
			byte[,,] scaledPixels;
			Bitmap res;

			try
			{
				scaledPixels = scaleFunc != null ? scaleFunc(_pixels) : _pixelsToBytes(_pixels);
			}
			catch (Exception ex)
			{
				throw new Exception("Помилка в роботі scaleFunc:\n\n" + ex.Message);
			}

			try
			{
				res = _rgbToBitmapQ(scaledPixels);
			}
			catch (Exception ex)
			{
				throw new Exception("Помилка у переведенні масиву байт в картинку:\n\n" + ex.Message);
			}

			return res;
		}

		private byte[,,] _pixelsToBytes(Pixel[][] pixels)
		{
			var res = new byte[3, Width, Height];
			for (int i = 0; i < Width; i++)
			{
				for (int j = 0; j < Height; j++)
				{
					res[0, i, j] = (byte)pixels[i][j].R;
					res[1, i, j] = (byte)pixels[i][j].G;
					res[2, i, j] = (byte)pixels[i][j].B;
				}
			}
			return res;
		}
		
		private unsafe Bitmap _rgbToBitmapQ(byte[,,] rgb)
		{
			if ((rgb.GetLength(0) != 3))
			{
				throw new ArrayTypeMismatchException("Size of first dimension for passed array must be 3 (RGB components)");
			}

			int width = rgb.GetLength(2),
				height = rgb.GetLength(1);

			Bitmap result = new Bitmap(width, height, PixelFormat.Format24bppRgb);

			BitmapData bd = result.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly,
				PixelFormat.Format24bppRgb);

			try
			{
				byte* curpos;
				fixed (byte* _rgb = rgb)
				{
					byte* _r = _rgb, _g = _rgb + width * height, _b = _rgb + 2 * width * height;
					for (int h = 0; h < height; h++)
					{
						curpos = ((byte*)bd.Scan0) + h * bd.Stride;
						for (int w = 0; w < width; w++)
						{
							*(curpos++) = *_b; ++_b;
							*(curpos++) = *_g; ++_g;
							*(curpos++) = *_r; ++_r;
						}
					}
				}
			}
			finally
			{
				result.UnlockBits(bd);
			}

			return result;
		}
	}
}
