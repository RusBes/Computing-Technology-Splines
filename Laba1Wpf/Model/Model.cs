using System;
using System.Collections.Generic;

namespace Laba1Wpf.Model
{
    class Model
    {
        private Random _rnd;

        private int _pointsInInterval;
        public double[] P { get; set; }

        private int _pointCount;
        public int n
        {
            get { return (int)Settings.Find(sett => sett.Name == "n").Value; }
            set { Settings.Find(sett => sett.Name == "n").Value = value; }
        }

        public double Interval
        {
            get
            {
                return Convert.ToDouble(Settings.Find(sett => sett.Name == "Interval").Value);
            }
            set
            {
                Settings.Find(sett => sett.Name == "Interval").Value = value;
            }
        }

        private List<Setting> _settings;
        public List<Setting> Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }


        public Model()
        {
            _rnd = new Random();
            _pointsInInterval = 11;

            Settings = new List<Setting>()
            {
                new Setting("Interval", "Інтервал", 1),
                new Setting("n", "n", 20, true),
                new Setting("ShownPoint", "Показати значення в точці", null, true)
            };

            P = new double[n];
	        for (int i = 0; i < P.Length - 1; i++)
			{
                if (i == 1)
                {
                    P[i] = P[i - 1];
                }
                else if (i == P.Length - 2)
                {
                    P[i + 1] = P[i];
                }
                else
                {
                    P[i] = _rnd.NextDouble();
                }
                
            }
        }


        public List<double[]> CalculatePoints(out List<double[]> nodalPoints)
        {
            var points = new List<double[]>();
            nodalPoints = new List<double[]>();
            var x = new double[_pointsInInterval];
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = -1 + (2.0 / (_pointsInInterval - 1)) * i;
            }

            for (int i = 0; i < P.Length; i++)
            {
                nodalPoints.Add(new double[] { i, P[i] });
            }
            
            for (int i = 1; i < n - 1; i++)
            {
                for (int j = 1; j < _pointsInInterval; j++)
                {
                    points.Add(new double[]
                    {
                        i + x[j] / 2,
                        CalculateValueInPoint(i + x[j] / 2)
                        //(1.0 / 8) * (Math.Pow(1 - x[j], 2) * P[i - 1] + (6 - 2 * Math.Pow(x[j], 2)) * P[i] + Math.Pow((1 + x[j]), 2) * P[i + 1])
                    });
                }
            }
            
            return points;
        }

        public double CalculateValueInPoint(double x)
        {
            double y = 0;

            // TODO round is not working, need to write round that could change 0.5 to 1, not to 0
            int i = (int)Math.Round(x);


            if (i < 1 || i > n - 2)
            {
                return -1;
            }

            x = (x - i) * 2;
            //i--;
            y = (1.0 / 8) * (Math.Pow(1 - x, 2) * P[i - 1] + (6 - 2 * Math.Pow(x, 2)) * P[i] + Math.Pow(1 + x, 2) * P[i + 1]);

            return y;
        }

        private double _calcValueInPoint_S20(double t, int i, double h)
        {
            double y = 0;

            double x = (2.0 / h) * (t - (i + 0.5) * h);

            y = (1.0 / 8) * (1);

            return y;
        }
    }
}
