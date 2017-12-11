using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using CommonModel;

namespace Laba1.View
{
    public partial class MainForm : Form
    {
        private const string SpecifiedPointIdentificator = "SpecifiedPoint";
        private readonly List<string> _unnecessarySettings = new List<string> { "IntervalLength", "ShownPoint" };


        public MainForm()
        {
            InitializeComponent();
        }

        public ScopeStates ScopeState;
        public Dictionary<string, Setting> Settings { private get; set; }

        public event EventHandler<SettingChangedEventArgs> SettingChanged;
        public event EventHandler<GraphicsEventArgs> BuildGraphicsNeeded;
        public event EventHandler<GraphicsEventArgs> ReBuildGraphicsNeeded;
        public event EventHandler<UnEvenTypesEventArgs> StartUnevenSplines;
        public event EventHandler LoadIamgeNeeded;
        public event EventHandler<EntityEventArgs> ApplyFilterToImageNeeded;
        public event EventHandler ScopeTogglerClicked;
        public event EventHandler<PicureBoxEventArgs> PictureBoxMouseEnter;
        public event EventHandler<PicureBoxEventArgs> PictureBoxMouseLeave;
        public event EventHandler<PicureBoxMouseEventArgs> PictureBoxMouseMove;
        public event EventHandler<PicureBoxFilterEventArgs> PictureBoxMouseDown;
        public event EventHandler<PicureBoxMouseEventArgs> PictureBoxMouseUp;

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

            _renderSettings();
            _renderSplineCheckBoxes();
        }

        private void _renderSettings()
        {
            foreach (var setting in Settings.Values)
            {
                // don't show unnecessary settings
                if (_unnecessarySettings.Contains(setting.Name))
                {
                    continue;
                }

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
                    Dock = DockStyle.Right
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
                    gbSplineTypes.Controls.Add(chb);
                }
            }
        }

        private int _specifiedPointsCounter;
        public void ShowSpecifiedPoint(double x, double y)
        {
            //var xMin = chartMain.ChartAreas[0].AxisX.Minimum;
            //var yMin = chartMain.ChartAreas[0].AxisY.Minimum;
            //chartMain.Series[2].Points.AddXY(xMin, y);
            //chartMain.Series[2].Points.AddXY(x, yMin);

            var series = new Series(SpecifiedPointIdentificator + _specifiedPointsCounter)
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
                if (chartMain.Series[i].Name.Contains(SpecifiedPointIdentificator))
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
            BuildGraphicsNeeded?.Invoke(sender, new GraphicsEventArgs(GetCheckedGraphics()));
        }

        private void butRebuildGraphics_Click(object sender, EventArgs e)
        {
            ReBuildGraphicsNeeded?.Invoke(sender, new GraphicsEventArgs(GetCheckedGraphics()));
        }

        private string[] GetCheckedGraphics()
        {
            //var graphics = new List<string>();
            //foreach (var chb in gbSplineTypes.Controls)
            //{
            //    if (chb is CheckBox && (chb as CheckBox).Checked)
            //    {
            //        graphics.Add((chb as CheckBox).Name.Replace("chb", "S"));
            //    }
            //}
            var graphics = FindAllControls<CheckBox>(gbSplineTypes).Where(chb => chb.Checked).Select(chb => chb.Name.Replace("chb", "S"));
            return graphics.ToArray();
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

        public void AddGraphic(List<PointD> points, List<PointD> nodalPoints = null, string name = "", Chart chart = null)
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

	        if (points != null && nodalPoints != null)
	        {
		        var minX = Math.Min(points.Any() ? points.Min(p => p.X) : 0, nodalPoints.Any() ? nodalPoints.Min(p => p.X) : 0);
		        var maxX = Math.Max(points.Any() ? points.Max(p => p.X) : 0, nodalPoints.Any() ? nodalPoints.Max(p => p.X) : 0);
		        var minY = Math.Min(points.Any() ? points.Min(p => p.Y) : 0, nodalPoints.Any() ? nodalPoints.Min(p => p.Y) : 0);
		        var maxY = Math.Max(points.Any() ? points.Max(p => p.Y) : 0, nodalPoints.Any() ? nodalPoints.Max(p => p.Y) : 0);

		        chart.ChartAreas[0].AxisX.Minimum = minX > 0 ? minX / 1.2 : minX * 1.2;
		        chart.ChartAreas[0].AxisX.Maximum = maxX > 0 ? maxX * 1.2 : maxX / 1.2;
		        chart.ChartAreas[0].AxisY.Minimum = minY > 0 ? minY / 1.2 : minY * 1.2;
		        chart.ChartAreas[0].AxisY.Maximum = maxY > 0 ? maxY * 1.2 : maxY / 1.2;
	        }
        }

        private void butStartUneven_Click(object sender, EventArgs e)
        {
            UnevenTypes type;
            if (rbStar.Checked)
            {
                type = UnevenTypes.Star;
            }
            else if (rbRectangle.Checked)
            {
                type = UnevenTypes.Rectangle;
            }
            else if (rbPetla.Checked)
            {
                type = UnevenTypes.Petla;
            }
            else
            {
                type = UnevenTypes.Normal;
            }
            var checkedGraphics = GetCheckedGraphics();
            var graphic = checkedGraphics.Length > 0 ? checkedGraphics[0] : "S20";
            StartUnevenSplines?.Invoke(sender, new UnEvenTypesEventArgs(graphic, type));
        }

        private void butLoadImage_Click(object sender, EventArgs e)
        {
            LoadIamgeNeeded?.Invoke(sender, new EventArgs());
        }

        public void ShowImageBefore(Bitmap img)
        {
            pbBefor.Image = img;
            pbBefor.Location = new Point(0, 0);
            pbBefor.Width = img.Width;
            pbBefor.Height = img.Height;
            panelImageBefore.Width = Math.Min(img.Width, (tabPage3.Width - 50) / 2);

            pbAfter.Location = new Point(0, 0);
            panelImageAfter.Location = new Point(panelImageBefore.Location.X + panelImageBefore.Width + 5, panelImageBefore.Location.Y);
        }

        public void ShowImageAfter(Bitmap img)
        {
            pbAfter.Image = img;
            pbAfter.Width = img.Width;
            pbAfter.Height = img.Height;
        }

        private void butApplyFilter_Click(object sender, EventArgs e)
        {
            var radioButtons = FindAllControls<RadioButton>(gbFilters);
            var checkedRb = radioButtons.FirstOrDefault(rb => rb.Checked);
            if (checkedRb == null)
            {
                ShowErrorMessage("Не вибрано тип фільтру!");
                return;
            }

            ApplyFilterToImageNeeded?.Invoke(sender, new EntityEventArgs(checkedRb.Name.Replace("rb", "")));
        }

        private List<T> FindAllControls<T>(Control parentControl) where T : class
        {
            if (!typeof(T).IsSubclassOf(typeof(Control)))
            {
                throw new ArgumentException("Шуканий тип повинен наслідуватися від \"Control\"");
            }

            List<T> res = new List<T>();
            foreach (Control control in parentControl.Controls)
            {
                if (control.GetType() == typeof(T))
                {
                    res.Add(control as T);
                }
                FindAllControls<T>(control);
            }
            return res;
        }


        public void DrawContur(List<Point> points, Color color, Graphics g)
        {
            pbBefor.Invalidate();
            for (int i = 0; i < points.Count - 1; i++)
            {
                g.DrawLine(new Pen(color), points[i], points[i + 1]);
            }
        }


        private void butScope_Click(object sender, EventArgs e)
        {
            ScopeTogglerClicked?.Invoke(sender, e);
        }

        private void pbImages_MouseEnter(object sender, EventArgs e)
        {
            PictureBoxMouseEnter?.Invoke(sender, new PicureBoxEventArgs(pbBefor));
        }

        private void pbImages_MouseLeave(object sender, EventArgs e)
        {
            PictureBoxMouseLeave?.Invoke(sender, new PicureBoxEventArgs(pbBefor));
        }

        private void pbImages_MouseMove(object sender, MouseEventArgs e)
        {
            PictureBoxMouseMove?.Invoke(sender, new PicureBoxMouseEventArgs(pbBefor, e.Location));
        }

        private void pbAImages_MouseDown(object sender, MouseEventArgs e)
        {
            var radioButtons = FindAllControls<RadioButton>(gbFilters);
            var checkedRb = radioButtons.FirstOrDefault(rb => rb.Checked);
            if (checkedRb == null)
            {
                ShowErrorMessage("Не вибрано тип фільтру!");
                return;
            }

            PictureBoxMouseDown?.Invoke(sender, new PicureBoxFilterEventArgs(checkedRb.Name.Replace("rb", "")));
        }

        private void pbImages_MouseUp(object sender, MouseEventArgs e)
        {
            PictureBoxMouseUp?.Invoke(sender, new PicureBoxMouseEventArgs(pbBefor, e.Location));
        }
    }



    public class PicureBoxEventArgs : EventArgs
    {
        public PictureBox PictureBox { get; }

        public PicureBoxEventArgs(PictureBox pb)
        {
            PictureBox = pb;
        }
    }

    public class PicureBoxMouseEventArgs : EventArgs
    {
        public PictureBox PictureBox { get; }

        public Point Location { get; }

        public PicureBoxMouseEventArgs(PictureBox pb, Point location)
        {
            PictureBox = pb;
            Location = location;
        }
    }
}
