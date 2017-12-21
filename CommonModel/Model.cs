using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using CommonModel.Classes;
using CommonModel.OneDimSplines;

// ReSharper disable InconsistentNaming

namespace CommonModel
{
    public partial class Model
    {
        private readonly Random _rnd = new Random();
        public delegate double CalcValueInPoint(double d, int i);
        public delegate double CalcValueInPoint2(double d);

	    public Bitmap Image { get; set; }

	    public Bitmap FilteredImage { get; set; }

	    public int ScopeRadius { get; set; }
		
		private int pointsInInterval => (int)Settings["PointsInInterval"].Value;
        private int n => (int)Settings["PointCount"].Value;
        private double intervalLength => (double)Settings["IntervalLength"].Value;
        private double[] X { get; set; }

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
            X = GetNewX(pointsInInterval);
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
            X = GetNewX(pointsInInterval);

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
		    return matrix.ApplyFilter(filter).ToBitmap();
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
    }
}
