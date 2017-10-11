using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using CommonModel;

namespace Laba1.View
{
    public partial class MainForm : Form
    {
	    private const string _specifiedPointIdentificator = "SpecifiedPoint";

	    public MainForm()
        {
            InitializeComponent();
        }
		
	    public Dictionary<string, Setting> Settings { private get; set; }

	    public event EventHandler<SettingChangedEventArgs> SettingChanged;
        public event EventHandler<GraphicEventArgs> RefreshGraphicNeeded;
	    public event EventHandler<GraphicsEventArgs> BuildGraphicsNeeded;
		public event EventHandler<GraphicsEventArgs> ReBuildGraphicsNeeded;
	    public event EventHandler StartUnevenSplines;

		public void DrawGraphic(IEnumerable<double[]> points, IEnumerable<double[]> nodalPoints)
        {
            chartMain.Series[0].Points.Clear();
            chartMain.Series[1].Points.Clear();
            foreach (var coor in points)
            {
                chartMain.Series[0].Points.AddXY(coor[0], coor[1]);
            }
            foreach (var coor in nodalPoints)
            {
                chartMain.Series[1].Points.AddXY(coor[0], coor[1]);
            }
        }

        public void Init()
        {
	        Width = 1000;

	        butSplainS2_0.Click += (sender, e) => RefreshGraphicNeeded?.Invoke(sender, new GraphicEventArgs("s20"));
	        butSplainS2_1.Click += (sender, e) => RefreshGraphicNeeded?.Invoke(sender, new GraphicEventArgs("s21"));
	        butSplainS2_2.Click += (sender, e) => RefreshGraphicNeeded?.Invoke(sender, new GraphicEventArgs("s22"));

	        butSplainS3_0.Click += (sender, e) => RefreshGraphicNeeded?.Invoke(sender, new GraphicEventArgs("s30"));
	        butSplainS3_1.Click += (sender, e) => RefreshGraphicNeeded?.Invoke(sender, new GraphicEventArgs("s31"));
	        butSplainS3_2.Click += (sender, e) => RefreshGraphicNeeded?.Invoke(sender, new GraphicEventArgs("s32"));

			_renderSettings();
	        _renderSplineCheckBoxes();
        }

	    private void _renderSettings()
	    {
		    foreach (var setting in Settings.Values)
		    {
			    var p = new Panel
			    {
				    Width = gbSettings.Width,
				    Height = 20,
				    Dock = DockStyle.Top
			    };
			    // p.BorderStyle = BorderStyle.FixedSingle;
			    gbSettings.Controls.Add(p);

			    var l = new Label
			    {
				    Text = setting.LabelText ?? "",
				    Location = new Point(10, 4)
			    };
			    p.Controls.Add(l);

			    var tb = new TextBox
			    {
				    Text = (setting.Value ?? "").ToString(),
				    Dock = DockStyle.Right,
					Enabled = (setting.Name != "IntervalLength")
				};
			    //tb.Location = new Point(10, 4);
			    tb.LostFocus += (sender, e) =>
			    {
				    if ((setting.Value?.ToString() ?? "") != tb.Text)
				    {
					    SettingChanged?.Invoke(setting, new SettingChangedEventArgs(setting.Name, tb.Text));
				    }
			    };

			    var pTextBoxWrapper = new Panel
			    {
				    Dock = DockStyle.Right,
				    Padding = new Padding(4),
				    Width = tb.Width + 10
			    };
			    p.AutoSize = true;
			    //pTextBoxWrapper.BorderStyle = BorderStyle.FixedSingle;
			    p.Controls.Add(pTextBoxWrapper);

			    pTextBoxWrapper.Controls.Add(tb);
		    }
	    }

		private void _renderSplineCheckBoxes()
	    {
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					if (j == 3 && i > 0)
					{
						break;
					}

					var chb = new CheckBox
					{
						Name = "chb" + (j + 2) + i,
						Text = "S" + (j + 2) + i,
						Location = new Point(5 + j * 60, 20 + i * 28),
						AutoSize = true
					};
					groupBox1.Controls.Add(chb);
				}
			}
		}

	    private int _specifiedPointsCounter;
	    public void ShowSpecifiedPoint(double x, double y)
        {
            var xMin = chartMain.ChartAreas[0].AxisX.Minimum;
            var yMin = chartMain.ChartAreas[0].AxisY.Minimum;
	        //chartMain.Series[2].Points.AddXY(xMin, y);
	        //chartMain.Series[2].Points.AddXY(x, yMin);

			var series = new Series(_specifiedPointIdentificator + _specifiedPointsCounter)
			{
				ChartType = SeriesChartType.Point
			};
	        _specifiedPointsCounter++;
	        series.Points.AddXY(x, y);
			chartMain.Series.Add(series);
	        
        }

	    public void ClearAllSpecifiedPoint()
	    {
		    for (int i = 0; i < chartMain.Series.Count; i++)
		    {
			    if (chartMain.Series[i].Name.Contains(_specifiedPointIdentificator))
			    {
				    chartMain.Series.RemoveAt(i);

				}
		    }
		}

	    public void ShowErrorMessage(string mes)
	    {
		    MessageBox.Show(mes);
	    }

	    public void ShowInfo(string mes)
	    {
		    lb.Items.Add(mes);
	    }

	    public void ClearInfo(string mes)
	    {
			lb.Items.Clear();
	    }

		private void butBuildGraphics_Click(object sender, EventArgs e)
		{
			//var graphics = from CheckBox chb 
			//			   in groupBox1.Controls
			//			   select chb.Name;

			var graphics = new List<string>();
			foreach (var chb in groupBox1.Controls)
			{
				if (chb is CheckBox && (chb as CheckBox).Checked)
				{
					graphics.Add((chb as CheckBox).Name.Replace("chb", "S"));
				}
			}

			BuildGraphicsNeeded?.Invoke(sender, new GraphicsEventArgs(graphics.ToArray()));
		}

		private void butRebuildGraphics_Click(object sender, EventArgs e)
		{
			//var graphics = from CheckBox chb 
			//			   in groupBox1.Controls
			//			   select chb.Name;

			var graphics = new List<string>();
			foreach (var chb in groupBox1.Controls)
			{
				if (chb is CheckBox && (chb as CheckBox).Checked)
				{
					graphics.Add((chb as CheckBox).Name.Replace("chb", "S"));
				}
			}

			ReBuildGraphicsNeeded?.Invoke(sender, new GraphicsEventArgs(graphics.ToArray()));
		}

		private void butClearGraphics_Click(object sender, EventArgs e)
		{
			ClearGraphics();
		}

	    public void ClearGraphics(Chart chart = null)
	    {
		    if (chart == null)
		    {
			    chart = chartMain;
		    }
		    chart.Series.Clear();
		}

	    public void AddGraphic(double[][] points, double[][] nodalPoints = null, string name = "")
	    {
		    Series s = new Series(name)
		    {
			    ChartType = SeriesChartType.Line,
			    Legend = "Legend1"
		    };
		    foreach (double[] p in points)
		    {
			    s.Points.AddXY(p[0], p[1]);
		    }
		    chartMain.Series.Add(s);

			if (nodalPoints != null)
		    {
			    Series sNodal = new Series(name + "NodalPoints")
			    {
				    ChartType = SeriesChartType.Point,
				    Legend = "Legend1"
			    };
			    foreach (double[] p in nodalPoints)
			    {
				    sNodal.Points.AddXY(p[0], p[1]);
			    }
				chartMain.Series.Add(sNodal);
		    }
	    }

	    public void AddGraphic(IEnumerable<PointD> points, IEnumerable<PointD> nodalPoints = null, string name = "", Chart chart = null)
	    {
		    if (chart == null)
		    {
			    chart = chartMain;
		    }

		    if (points != null)
		    {
			    Series s = new Series(name)
			    {
				    ChartType = SeriesChartType.Line,
				    Legend = "Legend1"
			    };
			    foreach (PointD p in points)
			    {
				    s.Points.AddXY(p.X, p.Y);
			    }
			    chart.Series.Add(s);
		    }

		    if (nodalPoints != null)
		    {
			    Series sNodal = new Series(name + "NodalPoints")
			    {
				    ChartType = SeriesChartType.Point,
				    Legend = "Legend1"
			    };
			    foreach (PointD p in nodalPoints)
			    {
				    sNodal.Points.AddXY(p.X, p.Y);
			    }
			    chart.Series.Add(sNodal);
		    }
	    }

		private void butStartUneven_Click(object sender, EventArgs e)
		{
			StartUnevenSplines?.Invoke(sender, new EventArgs());
		}

		//Timer timer = new Timer();
		//private void button7_Click(object sender, EventArgs e)
		//{
		//	timer.Interval = 1;
		//	timer.Tick += Tick_Tick;
		//	timer.Start();
		//}

		//   private int counter = 0;
		//private void Tick_Tick(object sender, EventArgs e)
		//{
		//	for (int i = 0; i < 100; i++)
		//	{
		//		chartMain.Series[0].Points.AddXY(5 * i, 5);
		//		counter += 100;
		//		if (counter > 100000)
		//		{
		//			timer.Stop();
		//		}
		//	}
		//}
	}
}
