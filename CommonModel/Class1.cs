using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModel
{
	class Splain
	{
		public List<PointD> Points { get; private set; }

		public Model.CalcValueInPoint CalculatePointsFunc { get; set; }
		public Interval Interval { get; set; }
		public bool IsCenter { get; set; }

		private int _pointsInInterval;
		public int PointsInInterval {
			get { return _pointsInInterval; }
			set
			{
				_pointsInInterval = value;
				X = GetNewX(_pointsInInterval);
			}
		}

		private int n => Points.Count;
		private double[] X { get; set; }

		public Splain()
		{
			X = GetNewX(_pointsInInterval);
		}
		public Splain(Model.CalcValueInPoint func, Interval interval, bool isCenter = true)
		{

		}
		public Splain(List<PointD> points) : this()
		{
			Points = points;
		}

		public List<PointD> CalculatePoints(Interval interval, bool isCenter = true)
		{
			if (CalculatePointsFunc == null)
			{
				throw new Exception("Не задана функція для обчислення значення в точці");
			}

			var points = new List<PointD>();
			for (int i = interval.Left; i < interval.Right; i++)
			{
				points.AddRange(
					X.Select(x => new PointD(
						isCenter ?
							i + x / 2 :
							i + x / 2 - 0.5,
						CalculatePointsFunc(x, i)
					))
				);
			}

			return points;
		}

		private double[] GetNewX(int pointsInInterval)
		{
			var x = new double[pointsInInterval];
			for (int i = 0; i < x.Length; i++)
			{
				x[i] = -1 + (2.0 / (pointsInInterval - 1)) * i;
			}
			return x;
		}

	}
}
