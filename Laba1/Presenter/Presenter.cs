using System;
using System.Collections.Generic;
using System.Linq;
using CommonModel;
using Laba1.View;

namespace Laba1.Presenter
{
	class Presenter
	{
		private readonly MainForm _view;

		private readonly Model _model;

		public Presenter(MainForm view, Model model)
		{
			_view = view;
			_model = model;

			_view.Settings = _model.Settings;
			_view.Init();
			_view.SettingChanged += _settingChangedHandler;
			//_view.RefreshGraphicNeeded += RefreshGraphicHandler;
			_view.BuildGraphicsNeeded += _buildGraphicsHandler;
			_view.ReBuildGraphicsNeeded += _rebuildGraphicsHandler;
			_view.StartUnevenSplines += _startUnevenSplinesHandler;
		}

		private void _startUnevenSplinesHandler(object sender, EventArgs e)
		{
			var len = (int) _model.Settings["PointCount"].Value;
			var X = _model.GetUnevenT(len);
			var Y = _model.GetP(len);
			var points = new List<PointD>();
			var regularX = new double[len];
			for (int i = 0; i < len; i++)
			{
				points.Add(new PointD(X[i], Y[i]));
				regularX[i] = i;
			}


			_model.P = X;
			var pointsRegularX = _model.CalculatePoints(_model.CalculateValueInPointS20, _model.GetInterval(true, 1, len), true);
			_model.P = Y;
			var pointsRegularY = _model.CalculatePoints(_model.CalculateValueInPointS20, _model.GetInterval(true, 1, len), true);

			var pointsX = new List<PointD>();
			var pointsY = new List<PointD>();
			var pointsRes = new List<PointD>();
			for (int i = 0; i < len; i++)
			{
				pointsX.Add(new PointD(regularX[i], X[i]));
				pointsY.Add(new PointD(regularX[i], Y[i]));
			}


			var bla = new double[pointsRegularX.Count];
			for (int i = 0; i < pointsRegularX.Count; i++)
			{
				bla[i] = pointsRegularX[i].Y * pointsRegularY[i].Y;
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
			Model.CalcValueInPoint func;
			Interval interval;
			bool isCenter;
			int len = (int)_model.Settings["PointCount"].Value;

			switch (graphicName.ToLower())
			{
				case "s20":
					func = _model.CalculateValueInPointS20;
					interval = _model.GetInterval(true, 1, len);
					isCenter = true;
					break;
				case "s21":
					func = _model.CalculateValueInPointS21;
					interval = _model.GetInterval(true, 2, len);
					isCenter = true;
					break;
				case "s22":
					func = _model.CalculateValueInPointS22;
					interval = _model.GetInterval(true, 3, len);
					isCenter = true;
					break;
				case "s30":
					func = _model.CalculateValueInPointS30;
					interval = _model.GetInterval(false, 1, len);
					isCenter = false;
					break;
				case "s31":
					func = _model.CalculateValueInPointS31;
					interval = _model.GetInterval(false, 2, len);
					isCenter = false;
					break;
				case "s32":
					func = _model.CalculateValueInPointS32;
					interval = _model.GetInterval(false, 3, len);
					isCenter = false;
					break;
				case "s40":
					func = _model.CalculateValueInPointS40;
					interval = _model.GetInterval(true, 2, len);
					isCenter = true;
					break;
				case "s41":
					func = _model.CalculateValueInPointS41;
					interval = _model.GetInterval(true, 3, len);
					isCenter = true;
					break;
				case "s42":
					func = _model.CalculateValueInPointS42;
					interval = _model.GetInterval(true, 4, len);
					isCenter = true;
					break;
				case "s50":
					func = _model.CalculateValueInPointS50;
					interval = _model.GetInterval(false, 2, len);
					isCenter = false;
					break;
				default:
					_view.ShowErrorMessage("Неможливо відтворити графік " + graphicName);
					return;
			}

			List<PointD> points = null;
			try
			{
				points = _model.CalculatePoints(func, interval, isCenter);
			}
			catch (NotImplementedException)
			{
				_view.ShowErrorMessage("Рішення для графіка " + graphicName + " поки не реалізовано");
				return;
			}
			//catch (Exception ex)
			//{
			//	_view.ShowErrorMessage(ex.Message);
			//	return;
			//}

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
						if (pCount <= _model.P.Length)
						{
							_model.P = _model.P.Take(pCount).ToArray();
						}
						else
						{
							_model.P = _model.P.Concat(_model.GetP(pCount - _model.P.Length)).ToArray();
						}
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

		private void RefreshGraphicHandler(object sender, GraphicEventArgs e)
		{
			List<double[]> points;

			//_model.P = _model.GetNewP((int)_model.Settings["PointCount"].Value);
			Model.CalcValueInPoint2 funCalcValueInPoint;

			switch (e.Graphic.ToLower())
			{
				case "s20":
					funCalcValueInPoint = _model.CalculateValueInPoint;
					points = _model.CalcPointsS20(_model.CalculateValueInPointS20);
					break;
				case "s21":
					funCalcValueInPoint = _model.CalculateValueInPointS21;
					points = _model.CalcPointsS21(_model.CalculateValueInPointS21);
					break;
				case "s22":
					funCalcValueInPoint = _model.CalculateValueInPointS22;
					points = _model.CalcPointsS22(_model.CalculateValueInPointS22);
					break;
				case "s30":
					funCalcValueInPoint = _model.CalculateValueInPointS30;
					points = _model.CalcPointsS30(_model.CalculateValueInPointS30);
					break;
				case "s31":
					funCalcValueInPoint = _model.CalculateValueInPointS31;
					points = _model.CalcPointsS31(_model.CalculateValueInPointS31);
					break;
				case "s32":
					funCalcValueInPoint = _model.CalculateValueInPointS32;
					points = _model.CalcPointsS32(_model.CalculateValueInPointS32);
					break;
				case "s40":
					funCalcValueInPoint = _model.CalculateValueInPointS40;
					points = _model.CalcPointsS40(_model.CalculateValueInPointS40);
					break;
				case "s41":
					funCalcValueInPoint = _model.CalculateValueInPointS41;
					points = _model.CalcPointsS41(_model.CalculateValueInPointS41);
					break;
				case "s42":
					funCalcValueInPoint = _model.CalculateValueInPointS42;
					points = _model.CalcPointsS42(_model.CalculateValueInPointS42);
					break;
				case "s50":
					funCalcValueInPoint = _model.CalculateValueInPointS50;
					points = _model.CalcPointsS50(_model.CalculateValueInPointS50);
					break;
				default:
					funCalcValueInPoint = _model.CalculateValueInPoint;
					points = _model.CalculatePoints(_model.CalculateValueInPoint);
					break;
			}



			if (points == null || !(points.Count > 1))
			{
				_view.ShowErrorMessage("Не вдалося відтворити графік. Недостатньо точок для відтворення графіку");
				return;
			}

			List<double[]> nodalPoints = new List<double[]>();
			for (int i = 0; i < _model.P.Length; i++)
			{
				nodalPoints.Add(new double[] { i, _model.P[i] });
			}
			_view.DrawGraphic(points, nodalPoints);

			var tmpShownPointX = _model.Settings["ShownPoint"].Value;
			if (tmpShownPointX != null)
			{
				try
				{
					double x = Convert.ToDouble(tmpShownPointX);
					double y = funCalcValueInPoint(x);
					_view.ClearAllSpecifiedPoint();
					_view.ShowSpecifiedPoint(x, y);
				}
				catch (InvalidCastException)
				{
					_view.ShowErrorMessage("Неправильно введені дані");
				}
				catch (Exception ex)
				{
					_view.ShowErrorMessage(ex.Message);
				}
			}
			else
			{
				_view.ClearAllSpecifiedPoint();
			}
		}
	}
}
