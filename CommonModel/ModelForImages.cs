using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using CommonModel.Classes;

namespace CommonModel
{
    public partial class Model
    {
        public Bitmap Image { get; set; }

        public int ScopeRadius { get; set; }

        public Bitmap LoadBitmap(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                Image = new Bitmap(fs);
                return Image;
            }
        }

		public unsafe byte[,,] BitmapToByteRgbQ(Bitmap bmp)
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

        //private Func<Bitmap, Bitmap> _filterFunc;
        public Bitmap ApplyFilter(Bitmap image, string filterName)
        {
            var matrix = new ImageMatrix(BitmapToByteRgbQ(image));
            switch (filterName)
            {
                case "JL20":
                    return MultiplyImageToMask(matrix, _jL20).ToBitmap();
                case "JL30":
                    return MultiplyImageToMask(matrix, _jL30).ToBitmap();
                case "JL40":
                    return MultiplyImageToMask(matrix, _jL40).ToBitmap();
                case "JL50":
                    return MultiplyImageToMask(matrix, _jL50).ToBitmap();
                case "JH20":
                    return MultiplyImageToMask(matrix, _jH20).ToBitmap();
                case "JH30":
                    return MultiplyImageToMask(matrix, _jH30).ToBitmap();
                case "JH40":
                    return MultiplyImageToMask(matrix, _jH40).ToBitmap();
                case "JH50":
                    return MultiplyImageToMask(matrix, _jH50).ToBitmap();
                case "JK20":
                    return MultiplyImageToMask(matrix, _jK20).ToBitmap();
                case "JK30":
                    return MultiplyImageToMask(matrix, _jK30).ToBitmap();
                //case "JK40":
                //    return MultiplyImageToMask(matrix, _jK40).ToBitmap();
                //case "JK50":
                //    return MultiplyImageToMask(matrix, _jK50).ToBitmap();
                case "JS20":
                    return MultiplyImageToMask(matrix, _jS20).ToBitmap();
                case "JS30":
                    return MultiplyImageToMask(matrix, _jS30).ToBitmap();
                case "JS40":
                    return MultiplyImageToMask(matrix, _jS40).ToBitmap();
                case "JS50":
                    return MultiplyImageToMask(matrix, _jS50).ToBitmap();
				case "Subdiv21":
					return MultiplyImageToMask(matrix, _sub21).ToBitmap();
				default:
                    throw new NotImplementedException("Даний фільтр ще не реалізовано");
            }
        }
        public Bitmap ApplyFilter(Bitmap image, string filterName, List<Point> pointsToApply)
        {
            var matrix = BitmapToByteRgbQ(image);
            switch (filterName)
            {
                case "JL20":
                    return RgbToBitmapQ(MultiplyImageToMask(matrix, _jL20, pointsToApply));
                case "JL30":                               
                    return RgbToBitmapQ(MultiplyImageToMask(matrix, _jL30, pointsToApply));
                case "JL40":                                               
                    return RgbToBitmapQ(MultiplyImageToMask(matrix, _jL40, pointsToApply));
                case "JL50":                                               
                    return RgbToBitmapQ(MultiplyImageToMask(matrix, _jL50, pointsToApply));
                case "JH20":                                               
                    return RgbToBitmapQ(MultiplyImageToMask(matrix, _jH20, pointsToApply));
                case "JH30":                                               
                    return RgbToBitmapQ(MultiplyImageToMask(matrix, _jH30, pointsToApply));
                case "JH40":                                               
                    return RgbToBitmapQ(MultiplyImageToMask(matrix, _jH40, pointsToApply));
                case "JH50":                                               
                    return RgbToBitmapQ(MultiplyImageToMask(matrix, _jH50, pointsToApply));
                case "JK20":                                               
                    return RgbToBitmapQ(MultiplyImageToMask(matrix, _jK20, pointsToApply));
                case "JK30":                                               
                    return RgbToBitmapQ(MultiplyImageToMask(matrix, _jK30, pointsToApply));
                //case "JK40":                                             
                //    return MultiplyImageToMask(matrix, JK40).ToBitmap(); 
                //case "JK50":                                             
                //    return MultiplyImageToMask(matrix, JK50).ToBitmap(); 
                case "JS20":                                               
                    return RgbToBitmapQ(MultiplyImageToMask(matrix, _jS20, pointsToApply));
                case "JS30":                                               
                    return RgbToBitmapQ(MultiplyImageToMask(matrix, _jS30, pointsToApply));
                case "JS40":                                               
                    return RgbToBitmapQ(MultiplyImageToMask(matrix, _jS40, pointsToApply));
                case "JS50":                                               
                    return RgbToBitmapQ(MultiplyImageToMask(matrix, _jS50, pointsToApply));
                default:
                    throw new NotImplementedException("Даний фільтр ще не реалізовано");
            }
        }

        public ImageMatrix MultiplyImageToMask(ImageMatrix P, Mask mask)
        {
            var indent = mask.Length / 2;
            var res = new ImageMatrix(P.Width, P.Height);
            try
            {
                for (int i = 0/*indent*/; i < P.Width /*- indent*/; i++)
                {
                    for (int j = 0/*indent*/; j < P.Height /*- indent*/; j++)
                    {
                        res[i, j] = ApplyFilterToPixel(i, j, P, mask);
                    }
                }
            }
            catch (Exception ex)
            {
                var a = ex;
            }

            return res;
        }

	    public ImageMatrix MultiplyImageToMask(ImageMatrix P, List<Mask> masks)
	    {
		    if (masks.Count != 4)
		    {
			    throw new ArgumentException("Для Subdivision потрібно 4 матриці");
		    }

		    var indent = masks[0].Length;
		    var res = new ImageMatrix(P.Width * 2, P.Height * 2);
		    try
		    {
				for (int i = indent; i < P.Width - indent; i++)
				{
					for (int j = indent; j < P.Height - indent; j++)
					{
						res[2 * i, 2 * j] = ApplyFilterToPixel(i, j, P, masks[0]);
						res[2 * i + 1, 2 * j] = ApplyFilterToPixel(i, j, P, masks[1]);
						res[2 * i, 2 * j + 1] = ApplyFilterToPixel(i, j, P, masks[2]);
						res[2 * i + 1, 2 * j + 1] = ApplyFilterToPixel(i, j, P, masks[3]);
					}
				}
			}
			catch (Exception ex)
			{
				
			}

			return res;
		}

		public byte[,,] MultiplyImageToMask(byte[,,] P, Mask mask, List<Point> pointsToApply)
        {
            var indent = mask.Length / 2;
            var width = P.GetLength(1);
            var height = P.GetLength(2);
            var res = new byte[3, width, height];
            try
            {

                for (int i = indent; i < width - indent; i++)
                {
                    for (int j = indent; j < height - indent; j++)
                    {
                        if (pointsToApply.Contains(new Point(i, j)))
                        {
                            //var startTime = DateTime.Now;
                            ApplyFilterToPixel(i, j, P, mask, res);
                            //var endTime = (DateTime.Now - startTime).TotalMilliseconds;
                        }
                        else
                        {
                            res[0, i, j] = P[0, i, j];
                            res[1, i, j] = P[1, i, j];
                            res[2, i, j] = P[2, i, j];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var a = ex;
            }

            return res;
        }
        private Pixel ApplyFilterToPixel(int i, int j, ImageMatrix P, Mask mask)
        {
            var indent = mask.Length / 2;
            //int r = 0, g = 0, b = 0;
            double r = 0, g = 0, b = 0;
            for (int ii = i - indent; ii <= i + indent; ii++)
            {
                for (int jj = j - indent; jj <= j + indent; jj++)
                {
					// to remove black borders
	                var iInd = ii < 0 ? 0 : ii > P.Width - 1 ? P.Width - 1 : ii;
	                var jInd = jj < 0 ? 0 : jj > P.Height - 1 ? P.Height - 1 : jj;
					//r += Convert.ToInt16((mask[ii - i + indent, jj - j + indent] * P[ii, jj].R) / mask.Denominator);
					//g += Convert.ToInt16((mask[ii - i + indent, jj - j + indent] * P[ii, jj].G) / mask.Denominator);
					//b += Convert.ToInt16((mask[ii - i + indent, jj - j + indent] * P[ii, jj].B) / mask.Denominator);

					r += ((mask[ii - i + indent, jj - j + indent] * P[iInd, jInd].R) / mask.Denominator);
                    g += ((mask[ii - i + indent, jj - j + indent] * P[iInd, jInd].G) / mask.Denominator);
                    b += ((mask[ii - i + indent, jj - j + indent] * P[iInd, jInd].B) / mask.Denominator);

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
        private void ApplyFilterToPixel(int i, int j, byte[,,] P, Mask mask, byte[,,] resArr)
        {
            var indent = mask.Length / 2;
            int r = 0, g = 0, b = 0;
            for (int ii = i - indent; ii <= i + indent; ii++)
            {
                for (int jj = j - indent; jj <= j + indent; jj++)
                {
                    r += Convert.ToInt16((mask[ii - i + indent, jj - j + indent] * P[0, ii, jj]) / mask.Denominator);
                    g += Convert.ToInt16((mask[ii - i + indent, jj - j + indent] * P[1, ii, jj]) / mask.Denominator);
                    b += Convert.ToInt16((mask[ii - i + indent, jj - j + indent] * P[2, ii, jj]) / mask.Denominator);

                    //r += ((mask[ii - i + indent, jj - j + indent] * P[ii, jj].R) / mask.Denominator);
                    //g += ((mask[ii - i + indent, jj - j + indent] * P[ii, jj].G) / mask.Denominator);
                    //b += ((mask[ii - i + indent, jj - j + indent] * P[ii, jj].B) / mask.Denominator);
                }
            }

            resArr[0, i, j] = _getByteInCorrectBorders(r);
            resArr[1, i, j] = _getByteInCorrectBorders(g);
            resArr[2, i, j] = _getByteInCorrectBorders(b);
            
            //return new Pixel(_getByteInCorrectBorders((int)r), _getByteInCorrectBorders((int)g), _getByteInCorrectBorders((int)b));

            //return new Pixel(Convert.ToByte((int)r), Convert.ToByte((int)g), Convert.ToByte((int)b));
        }
        private static byte _getByteInCorrectBorders(int num)
        {
            return Convert.ToByte(num < 0 ? 0 : num > 255 ? 255 : num);
        }
        private unsafe Bitmap RgbToBitmapQ(byte[,,] rgb)
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

        public unsafe byte[,,] BitmapToByteRgb(Bitmap bmp)
        {
            int width = bmp.Width,
                height = bmp.Height;
            byte[,,] res = new byte[3, height, width];
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            try
            {
                byte* curpos;
                for (int h = 0; h < height; h++)
                {
                    curpos = ((byte*)bd.Scan0) + h * bd.Stride;
                    for (int w = 0; w < width; w++)
                    {
                        res[2, h, w] = *(curpos++);
                        res[1, h, w] = *(curpos++);
                        res[0, h, w] = *(curpos++);
                    }
                }
            }
            finally
            {
                bmp.UnlockBits(bd);
            }
            return res;
        }
        public unsafe byte[] BitmapToByteRgbMarshal(Bitmap bmp)
        {
            int width = bmp.Width,
                height = bmp.Height;
            byte[] res = new byte[3 * height * width];
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height)
                , ImageLockMode.ReadOnly
                , PixelFormat.Format24bppRgb);
            try
            {
                int lineSize = width * 3;
                for (int h = 0; h < height; h++)
                {
                    int pos = h * lineSize;
                    IntPtr curpos = (IntPtr)((byte*)bd.Scan0) + h * bd.Stride;
                    Marshal.Copy(curpos, res, pos, lineSize);
                }
            }
            finally
            {
                bmp.UnlockBits(bd);
            }
            return res;
        }
        public unsafe Bitmap ByteRgbToBitmapMarshal(byte[] rgb, int width, int height)
        {
            Bitmap res = new Bitmap(width, height);
            BitmapData bd = res.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            try
            {
                int lineSize = width * 3;
                for (int h = 0; h < height; h++)
                {
                    int pos = h * lineSize;
                    IntPtr curpos = (IntPtr)((byte*)bd.Scan0) + h * bd.Stride;
                    Marshal.Copy(rgb, pos, curpos, lineSize);
                }
            }
            finally
            {
                res.UnlockBits(bd);
            }
            return res;
        }
        /// <summary>
        /// Функция предназначена для извлечения из экземпляра класса Bitmap данных о
        /// яркости отдельных пикселов и преобразования их в формат double[,,].
        /// При этом первый индекс соответствует одной из трех цветовых компонент (R, 
        /// G или B соответственно), второй - номер строки (координата Y), третий -
        /// номер столбца (координата X).
        /// </summary>
        /// <param name="bmp">Экземпляр Bitmap, из которого необходимо извлечь 
        /// яркостные данные.</param>
        /// <returns>Mассив double с данными о яркости каждой компоненты
        /// каждого пиксела.</returns>
        public unsafe double[,,] BitmapToDoubleRgb(Bitmap bmp)
        {
            int width = bmp.Width,
                height = bmp.Height;
            double[,,] res = new double[3, height, width];
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            try
            {
                byte* curpos;
                fixed (double* _res = res)
                {
                    double* _r = _res, _g = _res + 1, _b = _res + 2;
                    for (int h = 0; h < height; h++)
                    {
                        curpos = ((byte*)bd.Scan0) + h * bd.Stride;
                        for (int w = 0; w < width; w++)
                        {
                            *_b = *(curpos++); _b += 3;
                            *_g = *(curpos++); _g += 3;
                            *_r = *(curpos++); _r += 3;
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
        public byte[,,] BitmapToByteRgbNaive(Bitmap bmp)
        {
            int width = bmp.Width,
                height = bmp.Height;
            byte[,,] res = new byte[3, height, width];
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    Color color = bmp.GetPixel(x, y);
                    res[0, y, x] = color.R;
                    res[1, y, x] = color.G;
                    res[2, y, x] = color.B;
                }
            }
            return res;
        }
        /// <summary>
        /// Функция предназначена для создания нового экземпляра класса Bitmap на 
        /// базе имеющейся в виде byte[,,]-массива информацией о яркости каждого пиксела.
        /// При этом первый индекс соответствует одной из трех цветовых компонент (R, 
        /// G или B соответственно), второй - номер строки (координата Y), третий -
        /// номер столбца (координата X).
        /// </summary>
        /// <param name="rgb">Byte массив с данными о яркости каждой компоненты
        /// каждого пиксела</param>
        /// <returns>Новый экземпляр класса Bitmap</returns>
        public unsafe Bitmap RgbToBitmap(byte[,,] rgb)
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
                for (int h = 0; h < height; h++)
                {
                    curpos = ((byte*)bd.Scan0) + h * bd.Stride;
                    for (int w = 0; w < width; w++)
                    {
                        *(curpos++) = rgb[2, h, w];
                        *(curpos++) = rgb[1, h, w];
                        *(curpos++) = rgb[0, h, w];
                    }
                }
            }
            finally
            {
                result.UnlockBits(bd);
            }

            return result;
        }
        /// <summary>
        /// Функция предназначена для создания нового экземпляра класса Bitmap на 
        /// базе имеющейся в виде byte[,,]-массива информацией о яркости каждого пиксела.
        /// При этом первый индекс соответствует одной из трех цветовых компонент (R, 
        /// G или B соответственно), второй - номер строки (координата Y), третий -
        /// номер столбца (координата X).
        /// </summary>
        /// <param name="rgb">Double массив с данными о яркости каждой компоненты
        /// каждого пиксела</param>
        /// <returns>Новый экземпляр класса Bitmap</returns>
        public unsafe Bitmap RgbToBitmap(double[,,] rgb)
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
                fixed (double* _rgb = rgb)
                {
                    double* _r = _rgb, _g = _rgb + 1, _b = _rgb + 2;
                    for (int h = 0; h < height; h++)
                    {
                        curpos = ((byte*)bd.Scan0) + h * bd.Stride;
                        for (int w = 0; w < width; w++)
                        {
                            *(curpos++) = Limit(*_b); _b += 3;
                            *(curpos++) = Limit(*_g); _g += 3;
                            *(curpos++) = Limit(*_r); _r += 3;
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
        private byte Limit(double x)
        {
            if (x < 0)
                return 0;
            if (x > 255)
                return 255;
            return (byte)x;
        }
        public Bitmap RgbToBitmapNaive(byte[,,] rgb)
        {
            if ((rgb.GetLength(0) != 3))
            {
                throw new ArrayTypeMismatchException("Size of first dimension for passed array must be 3 (RGB components)");
            }

            int width = rgb.GetLength(2),
                height = rgb.GetLength(1);

            Bitmap result = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    result.SetPixel(x, y, Color.FromArgb(rgb[0, y, x], rgb[1, y, x], rgb[2, y, x]));
                }
            }

            return result;
        }
    }

    public struct Pixel
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        
        public Pixel(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
        }
    }
	
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

        public ImageMatrix(byte[,,] pixels) : this(pixels.GetLength(1), pixels.GetLength(2))
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    _pixels[i][j] = new Pixel(pixels[0, i, j], pixels[1, i, j], pixels[2, i, j]);
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
        public ImageMatrix() { }


        public Bitmap ToBitmap()
        {
            //var pixels = new byte[3, Width, Height];
            //for (int i = 0; i < Width; i++)
            //{
            //    for (int j = 0; j < Height; j++)
            //    {
            //        pixels[0, i, j] = (byte)Pixels[i][j].R;
            //        pixels[1, i, j] = (byte)Pixels[i][j].G;
            //        pixels[2, i, j] = (byte)Pixels[i][j].B;
            //    }
            //}
            //return RgbToBitmapQ(pixels);
            return RgbToBitmapQ(ScalePixels(_pixels));
        }


        public byte[,,] ScalePixels(Pixel[][] pixels)
        {
            var res = new byte[3, pixels.Length, pixels[0].Length];

			//   var maxR = pixels.Max(row => row.Max(p => p.R));
			//   var maxG = pixels.Max(row => row.Max(p => p.G));
			//   var maxB = pixels.Max(row => row.Max(p => p.B));

			//   var minR = pixels.Min(row => row.Min(p => p.R));
			//   var minG = pixels.Min(row => row.Min(p => p.G));
			//   var minB = pixels.Min(row => row.Min(p => p.B));

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

					//res[0, i, j] = (byte)((double)(pixels[i][j].R - minR) * 255 / (maxR - minR));
					//res[1, i, j] = (byte)((double)(pixels[i][j].G - minG) * 255 / (maxG - minG));
					//res[2, i, j] = (byte)((double)(pixels[i][j].B - minB) * 255 / (maxB - minB));

					res[0, i, j] = (byte)(pixels[i][j].R < 0 ? 0 : pixels[i][j].R > 255 ? 255 : pixels[i][j].R);
					res[1, i, j] = (byte)(pixels[i][j].G < 0 ? 0 : pixels[i][j].G > 255 ? 255 : pixels[i][j].G);
					res[2, i, j] = (byte)(pixels[i][j].B < 0 ? 0 : pixels[i][j].B > 255 ? 255 : pixels[i][j].B);
				}
            }

            return res;
        }


        private unsafe Bitmap RgbToBitmapQ(byte[,,] rgb)
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
