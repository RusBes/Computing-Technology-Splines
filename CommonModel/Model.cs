using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using CommonModel.Miscellaneous;
using CommonModel.OneDimSplines;
using CommonModel.TwoDimSplines;

// ReSharper disable InconsistentNaming

namespace CommonModel
{
    public class Model
    {
        private readonly Random _rnd = new Random();
        public delegate double CalcValueInPoint(double d, int i);
        public delegate double CalcValueInPoint2(double d);

	    public Bitmap Image { get; set; }

	    public Bitmap FilteredImage { get; set; }

	    public int ScopeRadius { get; set; }
		
		private int _pointsInInterval => (int)Settings["PointsInInterval"].Value;
        private int n => (int)Settings["PointCount"].Value;
        private double intervalLength => (double)Settings["IntervalLength"].Value;
	    private double[] X;

        private double[] GetNewX(int pointsInInterval)
        {
            var x = new double[pointsInInterval];
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = -1 + (2.0 / (pointsInInterval - 1)) * i;
            }
            return x;
        }

        public void RefreshX()
        {
            X = GetNewX(_pointsInInterval);
        }

        public Dictionary<string, Setting> DefaultSettings => new Dictionary<string, Setting>()
        {
            { "IntervalLength", new Setting("IntervalLength", "Довжина інтервалу", 1) },
            { "PointCount", new Setting("PointCount", "Кількість точок", 20, true) },
            { "PointsInInterval", new Setting("PointsInInterval", "Кількість точок в інтервалі", 11, true) },
            { "ShownPoint", new Setting("ShownPoint", "Показати значення в точці", null, true) }
        };

        public double[] P { get; set; }

        public Dictionary<string, Setting> Settings { get; }


        public Model()
        {
            Settings = DefaultSettings;
            P = GetP(n);
            X = GetNewX(_pointsInInterval);

            ScopeRadius = 20;
		}

	    public Bitmap LoadBitmap(string fileName)
	    {
		    using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
		    {
			    Image = new Bitmap(fs);
			    return Image;
		    }
	    }

	    public Bitmap ApplyFilter(Bitmap image, string filterName)
	    {
		    var matrix = new ImageMatrix(image);
		    var filter = FilterStorage.GetMask(filterName);
		    return matrix.ApplyFilter(filter).ToBitmap(ScaleImageHelper._cutPixels);
	    }

		public Spline CreateSpline(string name)
        {
            return new Spline(name, P);
        }

        public Spline CreateSpline(string name, double[] regularY)
        {
            return new Spline(name, regularY);
        }

        public double[] GetP(int len)
        {
            var res = new double[len];
            for (int i = 0; i < res.Length - 1; i++)
            {
                if (i == 1)
                {
                    res[i] = res[i - 1];
                }
                else if (i == res.Length - 2)
                {
                    res[i + 1] = res[i];
                }
                else
                {
                    res[i] = _rnd.NextDouble();
                }
            }
            return res;
        }

        public double[] GetUnevenT(int len)
        {
            var res = new double[len];
            for (int i = 0; i < res.Length; i++)
            {
                if (i == 0)
                {
                    res[i] = _rnd.NextDouble();
                }
                else
                {
                    res[i] = res[i - 1] + _rnd.NextDouble();
                }
            }
            return res;
        }

        public List<PointD> GetRectantlePoints(Interval additionalPointsCount)
        {
            List<PointD> res = new List<PointD>();
            res.Add(new PointD(2 + _rnd.NextDouble(), 1 + _rnd.NextDouble()));
            res.Add(new PointD(4 + _rnd.NextDouble(), 1 + _rnd.NextDouble()));
            res.Add(new PointD(4 + _rnd.NextDouble(), 3 + _rnd.NextDouble()));
            res.Add(new PointD(2 + _rnd.NextDouble(), 3 + _rnd.NextDouble()));

            _addAdditionalPointsWithCycle(res, additionalPointsCount);

            return res;
        }
        
        public List<PointD> GetStarPoints(Interval additionalPointsCount)
        {
            List<PointD> res = new List<PointD>();
            res.Add(new PointD(3 + _rnd.NextDouble(), 2 + _rnd.NextDouble() / 4));
            res.Add(new PointD(5 + _rnd.NextDouble(), 3 + _rnd.NextDouble() / 4));
            res.Add(new PointD(7 + _rnd.NextDouble(), 2 + _rnd.NextDouble() / 4));
            res.Add(new PointD(6 + _rnd.NextDouble(), 4 + _rnd.NextDouble() / 4));
            res.Add(new PointD(8 + _rnd.NextDouble(), 5 + _rnd.NextDouble() / 4));
            res.Add(new PointD(6 + _rnd.NextDouble(), 5 + _rnd.NextDouble() / 4));
            res.Add(new PointD(5 + _rnd.NextDouble(), 7 + _rnd.NextDouble() / 4));
            res.Add(new PointD(4 + _rnd.NextDouble(), 5 + _rnd.NextDouble() / 4));
            res.Add(new PointD(2 + _rnd.NextDouble(), 5 + _rnd.NextDouble() / 4));
            res.Add(new PointD(3.5 + _rnd.NextDouble(), 4 + _rnd.NextDouble() / 4));

            _addAdditionalPointsWithCycle(res, additionalPointsCount);

            return res;
        }

        public List<PointD> GetPetlyaPoints(Interval additionalPointsCount)
        {
            List<PointD> res = new List<PointD>();
            res.Add(new PointD(1, _rnd.NextDouble() / 4));
            res.Add(new PointD(2, _rnd.NextDouble() / 4));
            res.Add(new PointD(2 + _rnd.NextDouble(), _rnd.NextDouble() / 4));
            res.Add(new PointD(4 + _rnd.NextDouble(), _rnd.NextDouble() / 4));
            res.Add(new PointD(6 + _rnd.NextDouble(), _rnd.NextDouble()));
            res.Add(new PointD(7 + _rnd.NextDouble(), 2 + _rnd.NextDouble()));
            res.Add(new PointD(6 + _rnd.NextDouble(), 4 + _rnd.NextDouble()));
            res.Add(new PointD(4 + _rnd.NextDouble(), 2 + _rnd.NextDouble()));
            res.Add(new PointD(5 + _rnd.NextDouble(), _rnd.NextDouble()));
            res.Add(new PointD(7 + _rnd.NextDouble(), _rnd.NextDouble()));
            res.Add(new PointD(9 + _rnd.NextDouble(), _rnd.NextDouble()));
            res.Add(new PointD(10 + _rnd.NextDouble(), _rnd.NextDouble()));
            res.Add(new PointD(11, _rnd.NextDouble() / 4));
            res.Add(new PointD(12, _rnd.NextDouble() / 4));

            return res;
        }
        
        private void _addAdditionalPointsWithCycle(List<PointD> list, Interval pointsCount)
        {
            int n = list.Count - 1;
            var tmpRes = new List<PointD>(list);
            int count = 0;
            for (int i = 0; i < pointsCount.Right + pointsCount.Left + 1; i++, count++)
            {
                if (count > n)
                {
                    count = 0;
                }
                list.Add(tmpRes[count]);
            }
        }

        public Interval GetAdditionalPointsCount(string spline)
		{
            return new Spline(spline, null).GetAdditionalPointsNumbers();
		}

	    private class ScaleImageHelper
	    {
			public static byte[,,] _scalePixels(Pixel[][] pixels)
			{
				var res = new byte[3, pixels[0].Length, pixels.Length];

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

						res[0, j, i] = (byte)((double)(pixels[i][j].R - minR) * 255 / (maxR - minR));
						res[1, j, i] = (byte)((double)(pixels[i][j].G - minG) * 255 / (maxG - minG));
						res[2, j, i] = (byte)((double)(pixels[i][j].B - minB) * 255 / (maxB - minB));

						//res[0, i, j] = (byte)(pixels[i][j].R < 0 ? 0 : pixels[i][j].R > 255 ? 255 : pixels[i][j].R);
						//res[1, i, j] = (byte)(pixels[i][j].G < 0 ? 0 : pixels[i][j].G > 255 ? 255 : pixels[i][j].G);
						//res[2, i, j] = (byte)(pixels[i][j].B < 0 ? 0 : pixels[i][j].B > 255 ? 255 : pixels[i][j].B);
					}
				}

				return res;
			}

		    public static byte[,,] _scalePixelsWidthAbs(Pixel[][] pixels)
			{
				var res = new byte[3, pixels[0].Length, pixels.Length];

				var maxR = pixels.Max(row => row.Max(p => Math.Abs(p.R)));
				var maxG = pixels.Max(row => row.Max(p => Math.Abs(p.G)));
				var maxB = pixels.Max(row => row.Max(p => Math.Abs(p.B)));

				var minR = pixels.Min(row => row.Min(p => Math.Abs(p.R)));
				var minG = pixels.Min(row => row.Min(p => Math.Abs(p.G)));
				var minB = pixels.Min(row => row.Min(p => Math.Abs(p.B)));

				for (int i = 0; i < pixels.Length; i++)
				{
					for (int j = 0; j < pixels[0].Length; j++)
					{
						res[0, j, i] = (byte)((double)(pixels[i][j].R - minR) * 255 / (maxR - minR));
						res[1, j, i] = (byte)((double)(pixels[i][j].G - minG) * 255 / (maxG - minG));
						res[2, j, i] = (byte)((double)(pixels[i][j].B - minB) * 255 / (maxB - minB));
					}
				}

				return res;
			}

		    public static byte[,,] _cutPixels(Pixel[][] pixels)
			{
				var res = new byte[3, pixels[0].Length, pixels.Length];

				for (int i = 0; i < pixels.Length; i++)
				{
					for (int j = 0; j < pixels[0].Length; j++)
					{
						res[0, j, i] = (byte)(pixels[i][j].R < 0 ? 0 : pixels[i][j].R > 255 ? 255 : pixels[i][j].R);
						res[1, j, i] = (byte)(pixels[i][j].G < 0 ? 0 : pixels[i][j].G > 255 ? 255 : pixels[i][j].G);
						res[2, j, i] = (byte)(pixels[i][j].B < 0 ? 0 : pixels[i][j].B > 255 ? 255 : pixels[i][j].B);
					}
				}

				return res;
			}

		    public static byte[,,] _convertImplicitPixelsToByte(Pixel[][] pixels)
			{
				var res = new byte[3, pixels[0].Length, pixels.Length];

				for (int i = 0; i < pixels.Length; i++)
				{
					for (int j = 0; j < pixels[0].Length; j++)
					{
						res[0, j, i] = (byte)(pixels[i][j].R);
						res[1, j, i] = (byte)(pixels[i][j].G);
						res[2, j, i] = (byte)(pixels[i][j].B);
					}
				}

				return res;
			}
		}
    }
}
