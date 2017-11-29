using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonModel;

namespace CommonModel
{
    public class Spline
    {
        private readonly int _pointsInInterval = 10;

        private string _name;

        private int n;

        private Interval _interval;

        private double[] X;

        private Func<double, int, double> _calculateValueInPoint;

        private bool _isEven;

        private double[] Points;

        private double[] P => Points;

        public Interval AdditionalPointsCount => GetAdditionalPointsNumbers();
        

        public Spline(string type, double[] points)
        {
            _name = type;

            switch (type.ToLower())
            {
                case "s20":
                    _calculateValueInPoint = CalculateValueInPointS20;
                    break;
                case "s21":
                    _calculateValueInPoint = CalculateValueInPointS21;
                    break;
                case "s22":
                    _calculateValueInPoint = CalculateValueInPointS22;
                    break;
                case "s30":
                    _calculateValueInPoint = CalculateValueInPointS30;
                    break;
                case "s31":
                    _calculateValueInPoint = CalculateValueInPointS31;
                    break;
                case "s32":
                    _calculateValueInPoint = CalculateValueInPointS32;
                    break;
                case "s40":
                    _calculateValueInPoint = CalculateValueInPointS40;
                    break;
                case "s41":
                    _calculateValueInPoint = CalculateValueInPointS41;
                    break;
                case "s42":
                    _calculateValueInPoint = CalculateValueInPointS42;
                    break;
                case "s50":
                    _calculateValueInPoint = CalculateValueInPointS50;
                    break;
                default:
                    throw new Exception("Неможливо відтворити графік " + type);
            }

            _isEven = checkIsEven();


            if (points == null)
            {
                return;
            }
            n = points.Length;
            Points = points;
            X = GetNewX();
            _interval = GetInterval(AdditionalPointsCount, n);
        }

        private bool checkIsEven()
        {
            var splineRang = Convert.ToInt32(_name[_name.Length - 2]) - '0';
            return splineRang % 2 == 0;
        }

        /// <summary>
        /// returns count of additional points, needed to build current type of graphic
        /// </summary>
        /// <returns></returns>
        public Interval GetAdditionalPointsNumbers()
        {
            var splineRang = Convert.ToInt32(_name[_name.Length - 2]) - '0';
            var precise = Convert.ToInt32(_name[_name.Length - 1]) - '0';
            var _isEven = splineRang % 2 == 0;
            return new Interval(splineRang + precise - 1, splineRang + precise - 1 - (_isEven ? 0 : 1));
        }

        /// <summary>
        /// returns the interval where the graphic will be build
        /// </summary>
        /// <param name="additionalPoints"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public Interval GetInterval(Interval additionalPoints, int len)
        {
            return new Interval(additionalPoints.Left, len - additionalPoints.Right);
        }
        
        public List<PointD> CalculatePoints()
        {
            var points = new List<PointD>();

            for (int i = _interval.Left; i < _interval.Right; i++)
            {
                points.AddRange(
                    X.Select(x => new PointD(
                            _isEven ?
                                i + x / 2 :
                                i + x / 2 - 0.5,
                            _calculateValueInPoint(x, i)
                        ))
                );
            }

            return points;
        }

        /// <summary>
        /// Calculate X array from -1 to 1 with _pointsInInterval points
        /// </summary>
        /// <returns></returns>
        private double[] GetNewX()
        {
            var x = new double[_pointsInInterval];
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = -1 + (2.0 / (_pointsInInterval - 1)) * i;
            }
            return x;
        }

        #region CalculateValueInPoint functions
        private double CalculateValueInPointS20(double x, int i)
        {
            return (1.0 / 8) * (Math.Pow(1 - x, 2) * P[i - 1] + (6 - 2 * x * x) * P[i] + Math.Pow(1 + x, 2) * P[i + 1]);
        }

        private double CalculateValueInPointS21(double x, int i)
        {
            return (1.0 / 48) * (-Math.Pow(1 - x, 2) * P[i - 2] +
                                (2 - 16 * x + 10 * x * x) * P[i - 1] +
                                (46 - 18 * x * x) * P[i] +
                                (2 + 16 * x + 10 * x * x) * P[i + 1] -
                                Math.Pow(1 + x, 2) * P[i + 2]);
        }

        private double CalculateValueInPointS22(double x, int i)
        {
            return (1.0 / 288) * (Math.Pow(1 - x, 2) * P[i - 3] +
                                 (-4 + 20 * x - 12 * x * x) * P[i - 2] +
                                 (-5 - 106 * x + 75 * x * x) * P[i - 1] +
                                 (304 - 128 * x * x) * P[i] +
                                 (-5 + 106 * x + 75 * x * x) * P[i + 1] +
                                 (-4 - 20 * x - 12 * x * x) * P[i + 2] +
                                 Math.Pow(1 + x, 2) * P[i + 3]);
        }

        private double CalculateValueInPointS30(double x, int i)
        {
            return (1.0 / 48) * (
                P[i - 2] * (1 - 3 * x + 3 * x * x - 1 * x * x * x) +
                P[i - 1] * (23 - 15 * x - 3 * x * x + 3 * x * x * x) +
                P[i] * (23 + 15 * x - 3 * x * x - 3 * x * x * x) +
                P[i + 1] * (1 + 3 * x + 3 * x * x + 1 * x * x * x));
        }

        private double CalculateValueInPointS31(double x, int i)
        {
            return ((-5 + 15 * x - 15 * x * x + 5 * x * x * x) * P[i - 3] +
                    (-81 - 27 * x + 117 * x * x - 49 * x * x * x) * P[i - 2] +
                    (662 - 570 * x - 102 * x * x + 122 * x * x * x) * P[i - 1] +
                    (662 + 570 * x - 102 * x * x - 122 * x * x * x) * P[i] +
                    (-81 + 27 * x + 117 * x * x + 49 * x * x * x) * P[i + 1] +
                    (-5 - 15 * x - 15 * x * x - 5 * x * x * x) * P[i + 2]) / 1152;
        }

        private double CalculateValueInPointS32(double x, int i)
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

        private double CalculateValueInPointS40(double x, int i)
        {
            return ((1 - 4 * x + 6 * x * x - 4 * x * x * x + x * x * x * x) * P[i - 2] +
                    (76 - 88 * x + 24 * x * x + 8 * x * x * x - 4 * x * x * x * x) * P[i - 1] +
                    (230 - 60 * x * x + 6 * x * x * x * x) * P[i] +
                    (76 + 88 * x + 24 * x * x - 8 * x * x * x - 4 * x * x * x * x) * P[i + 1] +
                    (1 + 4 * x + 6 * x * x + 4 * x * x * x + x * x * x * x) * P[i + 2]) / 384;
        }

        private double CalculateValueInPointS41(double x, int i)
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

        private double CalculateValueInPointS42(double x, int i)
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

        private double CalculateValueInPointS50(double x, int i)
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
        #endregion
    }
}
