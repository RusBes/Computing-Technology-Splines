using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using CommonModel;
using Laba1.View;

namespace Laba1.Presenter
{
	class Matrix : IEnumerable<IEnumerable<double>>
	{
		public double[,] Arr;

		public void Add(params double[] a)
		{
			
		}

		public double this[int i, int j]
		{
			get { return 1; }
		}

		public IEnumerator<IEnumerable<double>> GetEnumerator()
		{
			throw new NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	class Worker
	{
		public void Do()
		{
			var a = new Matrix() { {1, 2, 3}, {1, 2, 3} };
		}
	}

	class Presenter
    {
        private readonly MainForm _view;

        private readonly Model _model;

        public Presenter(MainForm view)
        {
            _view = view;
            _model = new Model();

            _view.Settings = _model.Settings;
            _view.Init();
            _view.SettingChanged += _settingChangedHandler;
            _view.BuildGraphicsNeeded += _buildGraphicsHandler;
            _view.ReBuildGraphicsNeeded += _rebuildGraphicsHandler;
            _view.StartUnevenSplines += _startUnevenSplinesHandler;
            _view.LoadIamgeNeeded += _loadIamgeHandler;
            _view.ApplyFilterToImageNeeded += _applyFilterToImageHandler;
            _view.ScopeTogglerClicked += _scopeTogglerClickedHandler;

            // events for image scoping
            _view.PictureBoxMouseDown += _pictureBoxMouseDownHandler;
            _view.PictureBoxMouseUp+= _pictureBoxMouseUpHandler;
            _view.PictureBoxMouseEnter += _pictureBoxMousenterHandler;
            _view.PictureBoxMouseLeave+= _pictureBoxMouseLeaveHandler;
            _view.PictureBoxMouseMove += _pictureBoxMouseMoveHandler;
        }

        private void _pictureBoxMouseDownHandler(object sender, PicureBoxFilterEventArgs e)
        {
            if(_view.ScopeState == ScopeStates.On)
            {
                _view.ScopeState = ScopeStates.Applied;

                var startTime = DateTime.Now;

                var points = new List<Point>();
                var radius = _model.ScopeRadius;
                var topLeft = (new Point(_center.X - radius, _center.Y + radius));
                var botRight = (new Point(_center.X + radius, _center.Y - radius));
                for (int i = topLeft.X; i < botRight.X; i++)
                {
                    for (int j = topLeft.Y; j > botRight.Y; j--)
                    {
                        points.Add(new Point(j, i));
                    }
                }

                var endCalcPoints = (DateTime.Now - startTime).TotalMilliseconds;
                var startFilteringTime = DateTime.Now;

                var newImage = _model.ApplyFilter(_model.Image, e.Filter, points);

                var endFiltering = (DateTime.Now - startFilteringTime).TotalMilliseconds;

                _view.ShowImageBefore(newImage);


                var endTime = (DateTime.Now - startTime).TotalMilliseconds;
            }
        }

        private void _pictureBoxMouseUpHandler(object sender, PicureBoxMouseEventArgs e)
        {
            if (_view.ScopeState == ScopeStates.Applied)
            {
                _view.ScopeState = ScopeStates.On;
                _view.ShowImageBefore(_model.Image);
            }
        }

        private void _pictureBoxMousenterHandler(object sender, PicureBoxEventArgs e)
        {
            if(_view.ScopeState == ScopeStates.On)
            {
                e.PictureBox.Paint += _pictureBox_PaintHandler;
                _drawCounter++;
                //DrawContur(Cursor.Position, Color.Red, e.PictureBox.CreateGraphics(), e.PictureBox);
            }
        }

        int _drawCounter = 0;
        private void _pictureBoxMouseLeaveHandler(object sender, PicureBoxEventArgs e)
        {
            if (_view.ScopeState == ScopeStates.On)
            {
                e.PictureBox.Paint -= _pictureBox_PaintHandler;
                _drawCounter--;
                //DrawContur(Cursor.Position, Color.Red, e.PictureBox.CreateGraphics(), e.PictureBox);
            }
            //_view.ScopeState = ScopeStates.Off;
        }

        private void _pictureBoxMouseMoveHandler(object sender, PicureBoxMouseEventArgs e)
        {
            _center = e.Location;
            e.PictureBox.Invalidate();
            //if (_view.ScopeState == ScopeStates.On)
            //{
                //DrawContur(e.Location, Color.Red, e.PictureBox.CreateGraphics(), e.PictureBox);
            //}
        }

	    private Point _center;

        private void _pictureBox_PaintHandler(object sender, PaintEventArgs e)
        {
            DrawContur(_center, Color.Red, e.Graphics);
        }

        private void DrawContur(Point center, Color color, Graphics g)
        {
            var points = new List<Point>();
            var radius = _model.ScopeRadius;
            points.Add(new Point(center.X - radius, center.Y - radius));
            points.Add(new Point(center.X - radius, center.Y + radius));
            points.Add(new Point(center.X + radius, center.Y + radius));
            points.Add(new Point(center.X + radius, center.Y - radius));
            points.Add(new Point(center.X - radius, center.Y - radius));

            _view.DrawContur(points, color, g);
        }

        private void _scopeTogglerClickedHandler(object sender, EventArgs e)
        {
            if (_view.ScopeState == ScopeStates.Off)
            {
                _view.ScopeState = ScopeStates.On;
                _model.ScopeRadius = Convert.ToInt16(_view.nudScopeRadius.Value);
            }
            else
            {
                _view.ScopeState = ScopeStates.Off;
            }
        }

        private void _applyFilterToImageHandler(object sender, EntityEventArgs e)
        {
            if (_model.Image == null)
            {
                _view.ShowErrorMessage("Немає завантажених фото");
                return;
            }
            try
            {
                var modifying = _model.ApplyFilter(_model.Image, e.Name);
                //_model.Image = modifying;
                _view.ShowImageAfter(modifying);
            }
            catch(Exception ex)
            {
                _view.ShowErrorMessage(ex.Message);
            }
        }

        private void _loadIamgeHandler(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                _model.LoadBitmap(fileDialog.FileName);
            }

            _view.ShowImageBefore(_model.Image);
        }

        private void _startUnevenSplinesHandler(object sender, UnEvenTypesEventArgs e)
        {
            int len;
            double[] x;
            double[] y;
            List<PointD> tmpPoints;
            switch (e.Type)
            {
                case UnevenTypes.Star:
                    tmpPoints = _model.GetStarPoints(_model.GetAdditionalPointsCount(e.Graphic));
                    x = tmpPoints.Select(p => p.X).ToArray();
                    y = tmpPoints.Select(p => p.Y).ToArray();
                    len = x.Length;
                    break;
                case UnevenTypes.Rectangle:
                    tmpPoints = _model.GetRectantlePoints(_model.GetAdditionalPointsCount(e.Graphic));
                    x = tmpPoints.Select(p => p.X).ToArray();
                    y = tmpPoints.Select(p => p.Y).ToArray();
                    len = x.Length;
                    break;
                case UnevenTypes.Petla:
                    tmpPoints = _model.GetPetlyaPoints(_model.GetAdditionalPointsCount(e.Graphic));
                    x = tmpPoints.Select(p => p.X).ToArray();
                    y = tmpPoints.Select(p => p.Y).ToArray();
                    len = x.Length;
                    break;
                default:
                    len = (int)_model.Settings["PointCount"].Value;
                    x = _model.GetUnevenT(len);
                    y = _model.GetP(len);
                    break;
            }
            //var X = _model.GetUnevenT(len);
            //var Y = _model.GetT(len);
            var points = new List<PointD>();
            var regularX = new double[len];
            for (int i = 0; i < len; i++)
            {
                points.Add(new PointD(x[i], y[i]));
                regularX[i] = i;
            }


            var pointsRegularX = _model.CreateSpline(e.Graphic, x).CalculatePoints();
            var pointsRegularY = _model.CreateSpline(e.Graphic, y).CalculatePoints();

            var pointsX = new List<PointD>();
            var pointsY = new List<PointD>();
            var pointsRes = new List<PointD>();
            for (int i = 0; i < len; i++)
            {
                pointsX.Add(new PointD(regularX[i], x[i]));
                pointsY.Add(new PointD(regularX[i], y[i]));
            }

            for (int i = 0; i < pointsRegularX.Count; i++)
            {
                pointsRes.Add(new PointD(pointsRegularX[i].Y, pointsRegularY[i].Y));
            }

            _view.ClearGraphics(_view.chartUnevenPoints);
            _view.ClearGraphics(_view.chartRegularX);
            _view.ClearGraphics(_view.chartRegularY);
            _view.AddGraphic(pointsRes, points, chart: _view.chartUnevenPoints);
            _view.AddGraphic(pointsRegularX, pointsX, chart: _view.chartRegularX);
            _view.AddGraphic(pointsRegularY, pointsY, chart: _view.chartRegularY);
        }

        private void _rebuildGraphicsHandler(object sender, GraphicsEventArgs e)
        {
            _view.ClearGraphics();
            _model.P = _model.GetP(_model.P.Length);
            _buildGraphics(e.Graphics);
        }

        private void _buildGraphicsHandler(object sender, GraphicsEventArgs e)
        {
            _view.ClearGraphics();
            _buildGraphics(e.Graphics);
        }

        private void _buildGraphics(string[] graphics)
        {
            foreach (var graphicName in graphics)
            {
                _buildGraphic(graphicName);
            }
        }

        private void _buildGraphic(string graphicName)
        {
            var spline = _model.CreateSpline(graphicName);

            List<PointD> points;
            try
            {
                points = spline.CalculatePoints();
                //points = _model.CalculatePoints(func, interval, isCenter);
            }
            catch (NotImplementedException)
            {
                _view.ShowErrorMessage("Рішення для графіка " + graphicName + " поки не реалізовано");
                return;
            }

            if (points == null || !(points.Count > 1))
            {
                _view.ShowErrorMessage("Не вдалося відтворити графік" + graphicName + ". Недостатньо точок для відтворення графіку");
                return;
            }

            List<PointD> nodalPoints = _model.P.Select((t, i) => new PointD(i, t)).ToList();

            _view.AddGraphic(points, nodalPoints, graphicName);
        }

        private void _settingChangedHandler(object sender, SettingChangedEventArgs e)
        {
            try
            {
                double d;
                switch (e.Name)
                {
                    case "Interval":
                        _model.Settings[e.Name].Value = e.NewValue;
                        break;
                    case "ShownPoint":
                        if (Double.TryParse(e.NewValue, out d))
                        {
                            _model.Settings[e.Name].Value = d;
                        }
                        else
                        {
                            _model.Settings[e.Name].Value = null;
                        }
                        break;
                    case "PointsInInterval":
                        int pointsInInterval = int.Parse(e.NewValue);
                        if (pointsInInterval > 1)
                        {
                            _model.Settings[e.Name].Value = pointsInInterval;
                            _model.RefreshX();
                        }
                        else
                        {
                            _view.ShowErrorMessage("Кількість точок має бути більше 1");
                        }
                        break;
                    case "PointCount":
                        int pCount = int.Parse(e.NewValue);
                        _model.Settings[e.Name].Value = pCount;
                        _model.P = pCount <= _model.P.Length ? 
							_model.P.Take(pCount).ToArray() : 
							_model.P.Concat(_model.GetP(pCount - _model.P.Length)).ToArray();
                        break;
                    default:
                        _view.ShowErrorMessage("Виникла проблема зі зміною налаштувань, зміни було відхилено");
                        return;
                }
            }
            catch (Exception)
            {
                _view.ShowErrorMessage("Виникла проблема зі зміною налаштувань, зміни було відхилено");
            }
        }
    }
}
