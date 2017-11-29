using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

// ReSharper disable InconsistentNaming

namespace CommonModel
{
    public partial class Model
    {
        private readonly Random _rnd = new Random();
        public delegate double CalcValueInPoint(double d, int i);
        public delegate double CalcValueInPoint2(double d);

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


        #region USELESS
        public List<PointD> CalculatePoints(CalcValueInPoint calc, Interval interval, bool isCenter = true)
		{
			var points = new List<PointD>();

			for (int i = interval.Left; i < interval.Right; i++)
			{
				points.AddRange(
					X.Select(x => new PointD(
							isCenter ? 
								i + x / 2 : 
								i + x / 2 - 0.5, 
							calc(x, i)
						))
				);
			}

			return points;
		}


		public List<double[]> CalculatePoints(CalcValueInPoint calc)
		{
			var points = new List<double[]>();

			for (int i = 1; i < n - 1; i++)
			{
				for (int j = 1; j < pointsInInterval; j++)
				{
					points.Add(new double[]
					{
						i + X[j] / 2,
						calc(X[j], i)
					});
				}
			}

			return points;
		}

		public double CalculateValueInPoint(double x)
		{
			int i = (int)Math.Round(x);

			if (i < 1 || i > n - 2)
			{
				throw new Exception("Неможливо обчислити значення в даній точці");
			}

			x = (x - i) * 2;
			return (1.0 / 8) * ((1 - x) * (1 - x) * P[i - 1] + (6 - 2 * x * x) * P[i] + (1 + x) * (1 + x) * P[i + 1]);
		}

		public double CalculateValueInPoint(double x, int i)
		{
			if (i < 1 || i > n - 2)
			{
				throw new Exception("Неможливо обчислити значення в даній точці");
			}

			return (1.0 / 8) * ((1 - x) * (1 - x) * P[i - 1] + (6 - 2 * x * x) * P[i] + (1 + x) * (1 + x) * P[i + 1]);
		}

		public double CalculateValueInPoint(double x, double Pi_1, double Pi, double Pi1)
		{
			return (1.0 / 8) * ((1 - x) * (1 - x) * Pi_1 + (6 - 2 * x * x) * Pi + (1 + x) * (1 + x) * Pi1);
		}

		public List<double[]> CalcPointsS20(CalcValueInPoint calc)
		{
			var points = new List<double[]>();

			for (int i = 1; i < n - 1; i++)
			{
				for (int j = 1; j < pointsInInterval; j++)
				{
					points.Add(new double[]
					{
						i + X[j] / 2,
						calc(X[j], i)
					});
				}
			}

			return points;
		}

		public double CalculateValueInPointS20(double x, int i)
		{
			return (1.0 / 8) * (Math.Pow(1 - x, 2) * P[i - 1] + (6 - 2 * x * x) * P[i] + Math.Pow(1 + x, 2) * P[i + 1]);
		}

		public List<double[]> CalcPointsS21(CalcValueInPoint calc)
		{
			var points = new List<double[]>();

			for (int i = 2; i < n - 2; i++)
			{
				for (int j = 1; j < pointsInInterval; j++)
				{
					points.Add(new double[]
					{
						i + (1.0 / 2) * X[j],
						calc(X[j], i)
					});
				}
			}

			return points;
		}

		public double CalculateValueInPointS21(double x, int i)
		{
			return (1.0 / 48) * (-Math.Pow(1 - x, 2) * P[i - 2] +
								(2 - 16 * x + 10 * x * x) * P[i - 1] +
								(46 - 18 * x * x) * P[i] +
								(2 + 16 * x + 10 * x * x) * P[i + 1] -
								Math.Pow(1 + x, 2) * P[i + 2]);
		}
		public double CalculateValueInPointS21(double x)
		{
			int i = (int)Math.Round(x);

			if (i < 2 || i > n - 3)
			{
				throw new Exception("Неможливо обчислити значення в даній точці");
			}

			x = (x - i) * 2;
			return CalculateValueInPointS21(x, i);
		}

		public List<double[]> CalcPointsS22(CalcValueInPoint calc)
		{
			var points = new List<double[]>();

			for (int i = 3; i < n - 3; i++)
			{
				for (int j = 1; j < pointsInInterval; j++)
				{
					points.Add(new double[]
					{
						i + X[j] / 2,
						calc(X[j], i)
					});
				}
			}

			return points;
		}
		public double CalculateValueInPointS22(double x, int i)
		{
			return (1.0 / 288) * (Math.Pow(1 - x, 2) * P[i - 3] +
			                     (-4 + 20 * x - 12 * x * x) * P[i - 2] +
			                     (-5 - 106 * x + 75 * x * x) * P[i - 1] +
			                     (304 - 128 * x * x) * P[i] +
								 (-5 + 106 * x + 75 * x * x) * P[i + 1] +
								 (-4 - 20 * x - 12 * x * x) * P[i + 2] +
								 Math.Pow(1 + x, 2) * P[i + 3]);
		}
		public double CalculateValueInPointS22(double x)
		{
			int i = (int)Math.Round(x);

			if (i < 3 || i > n - 4)
			{
				throw new Exception("Неможливо обчислити значення в даній точці");
			}

			x = (x - i) * 2;
			return CalculateValueInPointS22(x, i);
		}

		public double CalculateValueInPointS30(double x)
		{
			throw new NotImplementedException();
		}

		public double CalculateValueInPointS32(double x)
		{
			throw new NotImplementedException();
		}


		

		public double CalculateValueInPointS31(double x)
		{
			throw new NotImplementedException();
		}

		

		public List<double[]> CalcPointsS30(CalcValueInPoint calc)
		{
			var points = new List<double[]>();

			for (int i = 2; i < n - 1; i++)
			{
				for (int j = 1; j < pointsInInterval; j++)
				{
					points.Add(new double[]
					{
						i +  1 * X[j] / 2 - 0.5,
						calc(X[j], i)
					});
				}
			}

			return points;
		}
		public double CalculateValueInPointS30(double x, int i)
		{
			return (1.0 / 48) * (
				P[i - 2] * (1 - 3 * x + 3 * x * x - 1 * x * x * x) + 
				P[i - 1] * (23 - 15 * x - 3 * x * x + 3 * x * x * x) + 
				P[i] * (23 + 15 * x - 3 * x * x - 3 * x * x * x) +
				P[i + 1] * (1 + 3 * x + 3 * x * x + 1 * x * x * x));
		}

		public List<double[]> CalcPointsS31(CalcValueInPoint calc)
		{
			var points = new List<double[]>();

			for (int i = 3; i < n - 2; i++)
			{
				for (int j = 1; j < pointsInInterval; j++)
				{
					points.Add(new double[]
					{
						i +  1 * X[j] / 2 - 0.5,
						calc(X[j], i)
					});
				}
			}

			return points;
		}
		public double CalculateValueInPointS31(double x, int i)
		{
			return ((-5 + 15 * x - 15 * x * x + 5 * x * x * x) * P[i - 3] +
			        (-81 - 27 * x + 117 * x * x - 49 * x * x * x) * P[i - 2] +
			        (662 - 570 * x - 102 * x * x + 122 * x * x * x) * P[i - 1] +
			        (662 + 570 * x - 102 * x * x - 122 * x * x * x) * P[i] +
			        (-81 + 27 * x + 117 * x * x + 49 * x * x * x) * P[i + 1] +
			        (-5 - 15 * x - 15 * x * x - 5 * x * x * x) * P[i + 2]) / 1152;
		}

		public List<double[]> CalcPointsS32(CalcValueInPoint calc)
		{
			var points = new List<double[]>();

			for (int i = 4; i < n - 3; i++)
			{
				for (int j = 1; j < pointsInInterval; j++)
				{
					points.Add(new double[]
					{
						i +  1 * X[j] / 2 - 0.5,
						calc(X[j], i)
					});
				}
			}

			return points;
		}
		public double CalculateValueInPointS32(double x, int i)
		{
			return ((47 - 141 * x + 141 * x * x - 47 * x * x * x) * P[i - 4] +
			        (653 + 579 * x - 1425 * x * x + 569 * x * x * x) * P[i - 3] +
			        (-6849 + 1383 * x + 6885 * x * x - 3339 * x * x * x) * P[i - 2] +
			        (33797 - 33705 * x - 5601 * x * x + 7501 * x * x * x) * P[i - 1] +
			        (33797 + 33705 * x - 5601 * x * x - 7501 * x * x * x) * P[i] +
			        (-6849 - 1383 * x + 6885 * x * x + 3335 * x * x * x) * P[i + 1] +
			        (653 - 579 * x - 1425 * x * x - 569 * x * x * x) * P[i + 2] +
			        (47 + 141 * x + 141 * x * x + 47 * x * x * x) * P[i + 3]) / 52296;
		}


		public List<double[]> CalcPointsS40(CalcValueInPoint calc)
		{
			var points = new List<double[]>();

			for (int i = 3; i < n - 3; i++)
			{
				for (int j = 1; j < pointsInInterval; j++)
				{
					points.Add(new double[]
					{
						i +  1 * X[j] / 2 - 0.5,
						calc(X[j], i)
					});
				}
			}

			return points;
		}
		public double CalculateValueInPointS40(double x, int i)
		{
			return ((1 - 4 * x + 6 * x * x - 4 * x * x * x + x * x * x * x) * P[i - 2] +
			        (76 - 88 * x + 24 * x * x + 8 * x * x * x - 4 * x * x * x * x) * P[i - 1] +
			        (230 - 60 * x * x + 6 * x * x * x * x) * P[i] +
			        (76 + 88 * x + 24 * x * x - 8 * x * x * x - 4 * x * x * x * x) * P[i + 1] +
			        (1 + 4 * x + 6 * x * x + 4 * x * x * x + x * x * x * x) * P[i + 2]) / 384;
		}
		public double CalculateValueInPointS40(double d)
		{
			throw new NotImplementedException();
		}

		public List<double[]> CalcPointsS41(CalcValueInPoint calc)
		{
			var points = new List<double[]>();

			for (int i = 3; i < n - 3; i++)
			{
				for (int j = 1; j < pointsInInterval; j++)
				{
					points.Add(new double[]
					{
						i +  1 * X[j] / 2 - 0.5,
						calc(X[j], i)
					});
				}
			}

			return points;
		}
		public double CalculateValueInPointS41(double x, int i)
		{
			double x2 = x * x;
			double x3 = x * x * x;
			double x4 = x * x * x * x;
			return ((-1 + 4 * x - 6 * x2 + 4 * x3 - x4) * P[i - 3] + 
					(-70 + 64 * x + 12 * x2 - 32 * x3 + 10 * x4) * P[i - 2] +
			        (225 - 524 * x + 198 * x2 + 52 * x3 - 31 * x4) * P[i - 1] + 
					(1228 - 408 * x2 + 44 * x4) * P[i] +
			        (225 + 524 * x + 198 * x2 - 52 * x3 - 31 * x4) * P[i + 1] +
			        (-70 - 64 * x + 12 * x2 + 32 * x3 + 10 * x4) * P[i + 2] + 
					(-1 - 4 * x - 6 * x2 - 4 * x3 - x4) * P[i + 3]) / 1536;
		}
		public double CalculateValueInPointS41(double d)
		{
			throw new NotImplementedException();
		}

		public List<double[]> CalcPointsS42(CalcValueInPoint calc)
		{
			var points = new List<double[]>();

			for (int i = 3; i < n - 3; i++)
			{
				for (int j = 1; j < pointsInInterval; j++)
				{
					points.Add(new double[]
					{
						i +  1 * X[j] / 2 - 0.5,
						calc(X[j], i)
					});
				}
			}

			return points;
		}
		public double CalculateValueInPointS42(double x, int i)
		{
			double x2 = x * x;
			double x3 = x * x * x;
			double x4 = x * x * x * x;
			return ((13 - 52 * x + 78 * x2 - 52 * x3 + 13 * x4) * P[i - 4] +
			        (876 - 696 * x - 360 * x2 + 552 * x3 - 164 * x4) * P[i - 3] +
			        (-5084 + 8104 * x - 840 * x2 - 2648 * x3 + 964 * x4) * P[i - 2] +
			        (8404 - 36952 * x + 16872 * x2 + 3848 * x3 - 2588 * x4) * P[i - 1] + 
					(83742 - 31500 * x2 + 3550 * x4) * P[i] +
			        (8404 + 36952 * x + 16872 * x2 - 3848 * x3 - 2588 * x4) * P[i + 1] +
			        (-5084 - 8104 * x - 840 * x2 + 2648 * x3 + 964 * x4) * P[i + 2] +
			        (876 + 696 * x - 360 * x2 - 552 * x3 - 164 * x4) * P[i + 3] +
			        (13 + 52 * x + 78 * x2 + 52 * x3 + 13 * x4) * P[i + 4]) / 92160;
		}
		public double CalculateValueInPointS42(double d)
		{
			throw new NotImplementedException();
		}


		public List<double[]> CalcPointsS50(CalcValueInPoint calc)
		{
			var points = new List<double[]>();

			for (int i = 3; i < n - 3; i++)
			{
				for (int j = 1; j < pointsInInterval; j++)
				{
					points.Add(new double[]
					{
						i +  1 * X[j] / 2 - 0.5,
						calc(X[j], i)
					});
				}
			}

			return points;
		}
		public double CalculateValueInPointS50(double x, int i)
		{
			double x2 = x * x;
			double x3 = x * x * x;
			double x4 = x * x * x * x;
			double x5 = x * x * x * x * x;
			return (Math.Pow((1 - x), 5) * P[i - 3] + 
				(237 - 375 * x + 210 * x2 - 30 * x3 - 15 * x4 + 3 * x5) * P[i - 2] +
			        (1682 - 770 * x - 220 * x2 + 140 * x3 + 10 * x4 - 10 * x5) * P[i - 1] +
			        (1682 + 770 * x - 220 * x2 - 140 * x3 + 10 * x4 + 10 * x5) * P[i] +
			        (237 + 375 * x + 210 * x2 + 30 * x3 - 15 * x4 - 5 * x5) * P[i + 1] + 
					(Math.Pow(1 + x, 5)) * P[i + 2]) / 3840;
		}
		public double CalculateValueInPointS50(double d)
		{
			throw new NotImplementedException();
		}

#endregion
    }
}
