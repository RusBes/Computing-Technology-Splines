using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace CommonModel
{
    public partial class Model
    {
        #region Filter masks

        // Low
        private readonly Mask JL20 = new Mask(
            new double[][] 
            {
                new double[] { 1, 6, 1 },
                new double[] { 6, 36, 6 },
                new double[] { 1, 6, 1 }
            }, 64);

        private readonly Mask JL30 = new Mask(
            new double[][]
            {
                new double[] { 1, 4, 1 },
                new double[] { 4, 16, 4 },
                new double[] { 1, 4, 1 }
            }, 36);

        private readonly Mask JL40 = new Mask(
            new double[][]
            {
                new double[] { 1, 76, 230, 76, 1},
                new double[] { 76, 5776, 17480, 5776, 76},
                new double[] { 230, 17480, 52900, 17480, 230},
                new double[] { 1, 76, 230, 76, 1},
                new double[] { 76, 5776, 17480, 5776, 76}
            }, 147456);

        private readonly Mask JL50 = new Mask(
            new double[][]
            {
                new double[] { 1, 26, 66, 276, 1},
                new double[] { 26, 676, 1716, 676, 26},
                new double[] { 66, 1716, 4386, 1716, 66},
                new double[] { 1, 26, 66,276, 1},
                new double[] { 26, 676, 1716, 676, 26}
            }, 14400);

        // Hight
        private readonly Mask JH20 = new Mask(
            new double[][]
            {
                new double[] { -1, -6, -1},
                new double[] { -6, 28, -6},
                new double[] { -1, -6, -1}
            }, 64);

        private readonly Mask JH30 = new Mask(
            new double[][]
            {
                new double[] { -1, -4, -1},
                new double[] { -4, 20, -4},
                new double[] { -1, -4, -1}
            }, 36);

        private readonly Mask JH40 = new Mask(
            new double[][]
            {
                new double[] { -1, -76, -230, -76, -1},
                new double[] { -76, -5776, -17480, -5776, -76},
                new double[] { -230, -17480, 94556, -17480, -230},
                new double[] { -1, -76, -230, -76, -1},
                new double[] { -76, -5776, -17480, -5776, -76}
            }, 147456);

        private readonly Mask JH50 = new Mask(
            new double[][]
            {
                new double[] { -1, -26, -66,-276, -1},
                new double[] { -26, -676, -1716, -676, -26},
                new double[] { -66, -1716, 10044, -1716, -66},
                new double[] { -1, -26, -66,-276, -1},
                new double[] { -26, -676, -1716, -676, -26}
            }, 14400);

        // Kontrast
        private readonly Mask JK20 = new Mask(
            new double[][]
            {
                new double[] { 1, -8, 48, -8, 1},
                new double[] { -8, 64, -384, 64, -8},
                new double[] { 48, -384, 2304, -384, 48},
                new double[] { -8, 64, -384, 64, -8},
                new double[] { 1, -8, 48, -8, 1}
            }, 1156);

        private readonly Mask JK30 = new Mask(
            new double[][]
            {
                new double[] { 1, -6, 24, -6, 1},
                new double[] { -6, 36, -144, 36, -6},
                new double[] { 24, -144, 576, -144, 24},
                new double[] { -6, 36, -144, 36, -6},
                new double[] { 1, -6, 24, -6, 1}
            }, 196);

        private readonly Mask JK40 = new Mask(
            new double[][]
            {
                new double[] { 1, -8, 48, -8, 1},
                new double[] { -8, 64, -384, 64, -8},
                new double[] { 48, -384, 2304, -384, 48},
                new double[] { -8, 64, -384, 64, -8},
                new double[] { 1, -8, 48, -8, 1}
            }, 1);

        private readonly Mask JK50 = new Mask(
            new double[][]
            {
                new double[] { 1, 6, 1 },
                new double[] { 6, 36, 6 },
                new double[] { 1, 6, 1 }
            }, 25784009476);

        // stabilisators
        private readonly Mask JS20 = new Mask(
            new double[][]
            {
                new double[] {3.75457E-09,8.93587E-07,5.40282E-06,-7.38748E-05, 5.40282E-06, 8.93587E-07, 3.75457E-09 },
                new double[] {8.93587E-07,0.000212674,0.001285871,-0.011758221,0.001285871,0.000212674,8.93587E-07 },
                new double[] {5.40282E-06,0.001285871, 0.007774658,-0.106305883,0.007774658,0.001285871,5.40282E-06},
                new double[] {-7.38748E-05,-0.01758221,-0.106305883,1.45356119,-0.106305883,-0.01758221,-7.38748E-05 },
                new double[] {5.40282E-06,0.001285871, 0.007774658,-0.106305883,0.007774658,0.001285871,5.40282E-06 },
                new double[] {8.93587E-07,0.000212674,0.001285871,-0.011758221,0.001285871,0.000212674,8.93587E-07  },
                new double[] {3.75457E-09,8.93587E-07,5.40282E-06,-7.38748E-05, 5.40282E-06, 8.93587E-07, 3.75457E-09 }
            }, 1);

        private readonly Mask JS30 = new Mask(
            new double[][]
            {
                new double[] {1.24562E-08,2.96456E-06,1.159314E-05,-0.000149424,1.159314E-05,2.96456E-06,1.24562E-08 },
                new double[] {2.96456E-06,0.000705566,0.003791678,-0.035562919,0.003791678,0.000705566,2.96456E-06 },
                new double[] {1.59314E-05,0.003791678,0.020376288,-0.191113331, 0.020376288,0.003791678,1.59314E-05},
                new double[] {-0.000149424,-0.035562919,-0.191113391,1.792490633,-0.191113391,-0.035562919, -0.000149424},
                new double[] {1.59314E-05,0.003791678,0.020376288,-0.191113331, 0.020376288,0.003791678,1.59314E-05 },
                new double[] {2.96456E-06,0.000705566,0.003791678,-0.035562919,0.003791678,0.000705566,2.96456E-06  },
                new double[] { 1.24562E-08,2.96456E-06,1.159314E-05,-0.000149424,1.159314E-05,2.96456E-06,1.24562E-08}
            }, 1);

        private readonly Mask JS40 = new Mask(
            new double[][]
            {
                new double[] {1.6236E-10,4.1165E-08,9.20847E-07,2.14132E-06,-1.8949E-05,2.14132E-06,9.20847E-07,4.1165E-08 ,1.6236E-10},
                new double[] {4.1165E-08,1.0437E-05,0.000233473,0.000542915,-0.004804375,0.000542915,0.000233473,1.0437E-05,4.1165E-08 },
                new double[] {9.20847E-07,0.000233473,0.000522272,0.012144825,-0.107472272,0.012144825,0.000522272,0.000233473,9.20847E-07 },
                new double[] {2.14132E-06,0.000542915,0.012144825,0.028241375,-0.249914233,0.028241375,0.012144825,0.000542915,2.14132E-06 },
                new double[] {-1.8949E-05,-0.004804375,-0.107472272,-0.249914233,2.211546853,-0.249914233,-0.107472272,-0.004804375,-1.8949E-05 },
                new double[] {2.14132E-06,0.000542915,0.012144825,0.028241375,-0.249914233,0.028241375,0.012144825,0.000542915,2.14132E-06  },
                new double[] {9.20847E-07,0.000233473,0.000522272,0.012144825,-0.107472272,0.012144825,0.000522272,0.000233473,9.20847E-07 },
                new double[] {4.1165E-08,1.0437E-05,0.000233473,0.000542915,-0.004804375,0.000542915,0.000233473,1.0437E-05,4.1165E-08 },
                new double[] {1.6236E-10,4.1165E-08,9.20847E-07,2.14132E-06,-1.8949E-05,2.14132E-06,9.20847E-07,4.1165E-08 ,1.6236E-10 }
            }, 1);

        private readonly Mask JS50 = new Mask(
            new double[][]
            {
                new double[] {5.26177E-10,1.32248E-07,3.7676E-06,4.92362E-06,-3.85865E-05,4.92362E-06,3.7676E-06,1.32248E-07,5.26177E-10},
                new double[] {1.32248E-07,3.32391E-05,0.000695605,0.001237494,-0.009698272,0.001237494,0.000695605,3.32391E-05, 1.32248E-07},
                new double[] {3.7676E-06,0.000695605,0.0145571357,0.025897446,-0.202958992,0.025897446,0.0145571357,0.000695605,3.7676E-06},
                new double[] {4.92362E-06,0.001237494,0.025897446,0.046072025,-0.361067725,0.046072025, 0.025897446,0.001237494,4.92362E-06 },
                new double[] {-3.85865E-05,-0.009698272,-0.202958992,-0.361067725,2.829697678,-0.361067725,-0.202958992,-0.009698272,-3.85865E-05 },
                new double[] {4.92362E-06,0.001237494,0.025897446,0.046072025,-0.361067725,0.046072025, 0.025897446,0.001237494,4.92362E-06  },
                new double[] {3.7676E-06,0.000695605,0.0145571357,0.025897446,-0.202958992,0.025897446,0.0145571357,0.000695605,3.7676E-06},
                new double[] {1.32248E-07,3.32391E-05,0.000695605,0.001237494,-0.009698272,0.001237494,0.000695605,3.32391E-05, 1.32248E-07 },
                new double[] {5.26177E-10,1.32248E-07,3.7676E-06,4.92362E-06,-3.85865E-05,4.92362E-06,3.7676E-06,1.32248E-07,5.26177E-10 }
            }, 1);

        #endregion

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
                    return MultiplyImageToMask(matrix, JL20).ToBitmap();
                case "JL30":
                    return MultiplyImageToMask(matrix, JL30).ToBitmap();
                case "JL40":
                    return MultiplyImageToMask(matrix, JL40).ToBitmap();
                case "JL50":
                    return MultiplyImageToMask(matrix, JL50).ToBitmap();
                case "JH20":
                    return MultiplyImageToMask(matrix, JH20).ToBitmap();
                case "JH30":
                    return MultiplyImageToMask(matrix, JH30).ToBitmap();
                case "JH40":
                    return MultiplyImageToMask(matrix, JH40).ToBitmap();
                case "JH50":
                    return MultiplyImageToMask(matrix, JH50).ToBitmap();
                case "JK20":
                    return MultiplyImageToMask(matrix, JK20).ToBitmap();
                case "JK30":
                    return MultiplyImageToMask(matrix, JK30).ToBitmap();
                //case "JK40":
                //    return MultiplyImageToMask(matrix, JK40).ToBitmap();
                //case "JK50":
                //    return MultiplyImageToMask(matrix, JK50).ToBitmap();
                case "JS20":
                    return MultiplyImageToMask(matrix, JS20).ToBitmap();
                case "JS30":
                    return MultiplyImageToMask(matrix, JS30).ToBitmap();
                case "JS40":
                    return MultiplyImageToMask(matrix, JS40).ToBitmap();
                case "JS50":
                    return MultiplyImageToMask(matrix, JS50).ToBitmap();
                default:
                    throw new NotImplementedException("Даний фільтр ще не реалізовано");
            }
        }
        public Bitmap ApplyFilter(Bitmap image, string filterName, List<Point> pointsToApply)
        {
            var matrix = new ImageMatrix(BitmapToByteRgbQ(image));
            switch (filterName)
            {
                case "JL20":
                    return MultiplyImageToMask(matrix, JL20, pointsToApply).ToBitmap();
                case "JL30":                               
                    return MultiplyImageToMask(matrix, JL30, pointsToApply).ToBitmap();
                case "JL40":                               
                    return MultiplyImageToMask(matrix, JL40, pointsToApply).ToBitmap();
                case "JL50":                               
                    return MultiplyImageToMask(matrix, JL50, pointsToApply).ToBitmap();
                case "JH20":                               
                    return MultiplyImageToMask(matrix, JH20, pointsToApply).ToBitmap();
                case "JH30":                               
                    return MultiplyImageToMask(matrix, JH30, pointsToApply).ToBitmap();
                case "JH40":                               
                    return MultiplyImageToMask(matrix, JH40, pointsToApply).ToBitmap();
                case "JH50":                               
                    return MultiplyImageToMask(matrix, JH50, pointsToApply).ToBitmap();
                case "JK20":                               
                    return MultiplyImageToMask(matrix, JK20, pointsToApply).ToBitmap();
                case "JK30":                               
                    return MultiplyImageToMask(matrix, JK30, pointsToApply).ToBitmap();
                //case "JK40":
                //    return MultiplyImageToMask(matrix, JK40).ToBitmap();
                //case "JK50":
                //    return MultiplyImageToMask(matrix, JK50).ToBitmap();
                case "JS20":
                    return MultiplyImageToMask(matrix, JS20, pointsToApply).ToBitmap();
                case "JS30":                               
                    return MultiplyImageToMask(matrix, JS30, pointsToApply).ToBitmap();
                case "JS40":                               
                    return MultiplyImageToMask(matrix, JS40, pointsToApply).ToBitmap();
                case "JS50":                               
                    return MultiplyImageToMask(matrix, JS50, pointsToApply).ToBitmap();
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
                for (int i = indent; i < P.Width - indent; i++)
                {
                    for (int j = indent; j < P.Height - indent; j++)
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
        public ImageMatrix MultiplyImageToMask(ImageMatrix P, Mask mask, List<Point> pointsToApply)
        {
            var indent = mask.Length / 2;
            var res = new ImageMatrix(P.Width, P.Height);
            try
            {
                for (int i = indent; i < P.Width - indent; i++)
                {
                    for (int j = indent; j < P.Height - indent; j++)
                    {
                        if (pointsToApply.Contains(new Point(i, j)))
                        {
                            res[i, j] = ApplyFilterToPixel(i, j, P, mask);
                        }
                        else
                        {
                            res[i, j] = P[i, j];
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
                    r += Convert.ToInt16((mask[ii - i + indent, jj - j + indent] * P[ii, jj].R) / mask.Denominator);
                    g += Convert.ToInt16((mask[ii - i + indent, jj - j + indent] * P[ii, jj].G) / mask.Denominator);
                    b += Convert.ToInt16((mask[ii - i + indent, jj - j + indent] * P[ii, jj].B) / mask.Denominator);
                    //r += ((mask[ii - i + indent, jj - j + indent] * P[ii, jj].R) / mask.Denominator);
                    //g += ((mask[ii - i + indent, jj - j + indent] * P[ii, jj].G) / mask.Denominator);
                    //b += ((mask[ii - i + indent, jj - j + indent] * P[ii, jj].B) / mask.Denominator);
                }
            }

            return new Pixel(_getByteInCorrectBorders((int)r), _getByteInCorrectBorders((int)g), _getByteInCorrectBorders((int)b));
            //return new Pixel(Convert.ToByte((int)r), Convert.ToByte((int)g), Convert.ToByte((int)b));
        }
        private static byte _getByteInCorrectBorders(int num)
        {
            return Convert.ToByte(num < 0 ? 0 : num > 255 ? 255 : num);
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
        public byte A { get; set; }
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        
        public Pixel(byte r, byte g, byte b) : this(255, r, g, b) { }
        public Pixel(byte a, byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }
    }

    public struct Mask
    {
        public double[][] Matrix { get; set; }
        public long Denominator { get; set; }

        public int Length => Matrix != null ? Matrix.Length : -1;

        public double this[int i, int j]
        {
            get
            {
                return Matrix[i][j];
            }
            set
            {
                Matrix[i][j] = value;
            }
        }

        public Mask(double[][] mask, long denominator)
        {
            Matrix = mask;
            Denominator = denominator;
        }
    }

    public class ImageMatrix
    {
        private Pixel[][] Pixels;

        public int Width => Pixels.Length;
        public int Height => Pixels[0].Length;


        public Pixel this[int i, int j]
        {
            get
            {
                return Pixels[i][j];
            }
            set
            {
                Pixels[i][j] = value;
            }
        }


        //public ImageMatrix(double[][] pixels)
        //{
        //    Pixels = new Pixel[][];
        //}
        public ImageMatrix(byte[,,] pixels) : this(pixels.GetLength(1), pixels.GetLength(2))
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Pixels[i][j] = new Pixel(pixels[0, i, j], pixels[1, i, j], pixels[2, i, j]);
                }
            }
        }
        public ImageMatrix(int width, int height)
        {
            Pixels = new Pixel[width][];
            for (int j = 0; j < width; j++)
            {
                Pixels[j] = new Pixel[height];
            }
        }
        public ImageMatrix() { }

        public Bitmap ToBitmap()
        {
            var pixels = new byte[3, Width, Height];
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    pixels[0, i, j] = Pixels[i][j].R;
                    pixels[1, i, j] = Pixels[i][j].G;
                    pixels[2, i, j] = Pixels[i][j].B;
                }
            }
            return RgbToBitmapQ(pixels);
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
