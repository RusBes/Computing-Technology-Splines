namespace Laba1.View
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
			this.chartMain = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.butSplainS2_0 = new System.Windows.Forms.Button();
			this.butSplainS2_1 = new System.Windows.Forms.Button();
			this.butSplainS2_2 = new System.Windows.Forms.Button();
			this.gbSettings = new System.Windows.Forms.GroupBox();
			this.butSplainS3_2 = new System.Windows.Forms.Button();
			this.butSplainS3_1 = new System.Windows.Forms.Button();
			this.butSplainS3_0 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.button5 = new System.Windows.Forms.Button();
			this.button6 = new System.Windows.Forms.Button();
			this.lb = new System.Windows.Forms.ListBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.butClearGraphics = new System.Windows.Forms.Button();
			this.butRebuildGraphics = new System.Windows.Forms.Button();
			this.butBuildGraphics = new System.Windows.Forms.Button();
			this.button7 = new System.Windows.Forms.Button();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.butStartUneven = new System.Windows.Forms.Button();
			this.chartRegularX = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.chartRegularY = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.chartUnevenPoints = new System.Windows.Forms.DataVisualization.Charting.Chart();
			((System.ComponentModel.ISupportInitialize)(this.chartMain)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.chartRegularX)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.chartRegularY)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.chartUnevenPoints)).BeginInit();
			this.SuspendLayout();
			// 
			// chartMain
			// 
			chartArea1.Name = "ChartArea1";
			this.chartMain.ChartAreas.Add(chartArea1);
			legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
			legend1.Name = "Legend1";
			this.chartMain.Legends.Add(legend1);
			this.chartMain.Location = new System.Drawing.Point(6, 134);
			this.chartMain.Name = "chartMain";
			series1.ChartArea = "ChartArea1";
			series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
			series1.Legend = "Legend1";
			series1.Name = "Series1";
			series2.ChartArea = "ChartArea1";
			series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
			series2.Legend = "Legend1";
			series2.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
			series2.Name = "Series2";
			series3.ChartArea = "ChartArea1";
			series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
			series3.Legend = "Legend1";
			series3.MarkerSize = 8;
			series3.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Star5;
			series3.Name = "Series3";
			this.chartMain.Series.Add(series1);
			this.chartMain.Series.Add(series2);
			this.chartMain.Series.Add(series3);
			this.chartMain.Size = new System.Drawing.Size(590, 315);
			this.chartMain.TabIndex = 1;
			this.chartMain.Text = "chart1";
			// 
			// butSplainS2_0
			// 
			this.butSplainS2_0.Location = new System.Drawing.Point(410, 259);
			this.butSplainS2_0.Name = "butSplainS2_0";
			this.butSplainS2_0.Size = new System.Drawing.Size(83, 28);
			this.butSplainS2_0.TabIndex = 2;
			this.butSplainS2_0.Text = "Сплайн S2,0";
			this.butSplainS2_0.UseVisualStyleBackColor = true;
			this.butSplainS2_0.Visible = false;
			// 
			// butSplainS2_1
			// 
			this.butSplainS2_1.Location = new System.Drawing.Point(410, 293);
			this.butSplainS2_1.Name = "butSplainS2_1";
			this.butSplainS2_1.Size = new System.Drawing.Size(83, 28);
			this.butSplainS2_1.TabIndex = 2;
			this.butSplainS2_1.Text = "Сплайн S2,1";
			this.butSplainS2_1.UseVisualStyleBackColor = true;
			this.butSplainS2_1.Visible = false;
			// 
			// butSplainS2_2
			// 
			this.butSplainS2_2.Location = new System.Drawing.Point(410, 327);
			this.butSplainS2_2.Name = "butSplainS2_2";
			this.butSplainS2_2.Size = new System.Drawing.Size(83, 28);
			this.butSplainS2_2.TabIndex = 2;
			this.butSplainS2_2.Text = "Сплайн S2,2";
			this.butSplainS2_2.UseVisualStyleBackColor = true;
			this.butSplainS2_2.Visible = false;
			// 
			// gbSettings
			// 
			this.gbSettings.Location = new System.Drawing.Point(6, 6);
			this.gbSettings.Name = "gbSettings";
			this.gbSettings.Size = new System.Drawing.Size(270, 122);
			this.gbSettings.TabIndex = 3;
			this.gbSettings.TabStop = false;
			this.gbSettings.Text = "Налаштування";
			// 
			// butSplainS3_2
			// 
			this.butSplainS3_2.Location = new System.Drawing.Point(499, 327);
			this.butSplainS3_2.Name = "butSplainS3_2";
			this.butSplainS3_2.Size = new System.Drawing.Size(83, 28);
			this.butSplainS3_2.TabIndex = 4;
			this.butSplainS3_2.Text = "Сплайн S3,2";
			this.butSplainS3_2.UseVisualStyleBackColor = true;
			this.butSplainS3_2.Visible = false;
			// 
			// butSplainS3_1
			// 
			this.butSplainS3_1.Location = new System.Drawing.Point(499, 293);
			this.butSplainS3_1.Name = "butSplainS3_1";
			this.butSplainS3_1.Size = new System.Drawing.Size(83, 28);
			this.butSplainS3_1.TabIndex = 5;
			this.butSplainS3_1.Text = "Сплайн S3,1";
			this.butSplainS3_1.UseVisualStyleBackColor = true;
			this.butSplainS3_1.Visible = false;
			// 
			// butSplainS3_0
			// 
			this.butSplainS3_0.Location = new System.Drawing.Point(499, 259);
			this.butSplainS3_0.Name = "butSplainS3_0";
			this.butSplainS3_0.Size = new System.Drawing.Size(83, 28);
			this.butSplainS3_0.TabIndex = 6;
			this.butSplainS3_0.Text = "Сплайн S3,0";
			this.butSplainS3_0.UseVisualStyleBackColor = true;
			this.butSplainS3_0.Visible = false;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(588, 327);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(83, 28);
			this.button1.TabIndex = 7;
			this.button1.Text = "Сплайн S4,2";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Visible = false;
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(588, 293);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(83, 28);
			this.button2.TabIndex = 8;
			this.button2.Text = "Сплайн S4,1";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Visible = false;
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(588, 259);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(83, 28);
			this.button3.TabIndex = 9;
			this.button3.Text = "Сплайн S4,0";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Visible = false;
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(677, 327);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(83, 28);
			this.button4.TabIndex = 10;
			this.button4.Text = "Сплайн S5,2";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Visible = false;
			// 
			// button5
			// 
			this.button5.Location = new System.Drawing.Point(677, 293);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(83, 28);
			this.button5.TabIndex = 11;
			this.button5.Text = "Сплайн S5,1";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Visible = false;
			// 
			// button6
			// 
			this.button6.Location = new System.Drawing.Point(677, 259);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(83, 28);
			this.button6.TabIndex = 12;
			this.button6.Text = "Сплайн S5,0";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Visible = false;
			// 
			// lb
			// 
			this.lb.FormattingEnabled = true;
			this.lb.Location = new System.Drawing.Point(602, 134);
			this.lb.Name = "lb";
			this.lb.Size = new System.Drawing.Size(205, 316);
			this.lb.TabIndex = 13;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.butClearGraphics);
			this.groupBox1.Controls.Add(this.butRebuildGraphics);
			this.groupBox1.Controls.Add(this.butBuildGraphics);
			this.groupBox1.Location = new System.Drawing.Point(282, 6);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(394, 122);
			this.groupBox1.TabIndex = 15;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Побудова сплайнів";
			// 
			// butClearGraphics
			// 
			this.butClearGraphics.Location = new System.Drawing.Point(253, 77);
			this.butClearGraphics.Name = "butClearGraphics";
			this.butClearGraphics.Size = new System.Drawing.Size(135, 39);
			this.butClearGraphics.TabIndex = 0;
			this.butClearGraphics.Text = "Очистити графіки";
			this.butClearGraphics.UseVisualStyleBackColor = true;
			this.butClearGraphics.Click += new System.EventHandler(this.butClearGraphics_Click);
			// 
			// butRebuildGraphics
			// 
			this.butRebuildGraphics.Location = new System.Drawing.Point(253, 48);
			this.butRebuildGraphics.Name = "butRebuildGraphics";
			this.butRebuildGraphics.Size = new System.Drawing.Size(135, 23);
			this.butRebuildGraphics.TabIndex = 0;
			this.butRebuildGraphics.Text = "Перебудувати графіки";
			this.butRebuildGraphics.UseVisualStyleBackColor = true;
			this.butRebuildGraphics.Click += new System.EventHandler(this.butRebuildGraphics_Click);
			// 
			// butBuildGraphics
			// 
			this.butBuildGraphics.Location = new System.Drawing.Point(253, 19);
			this.butBuildGraphics.Name = "butBuildGraphics";
			this.butBuildGraphics.Size = new System.Drawing.Size(135, 23);
			this.butBuildGraphics.TabIndex = 0;
			this.butBuildGraphics.Text = "Побудувати графіки";
			this.butBuildGraphics.UseVisualStyleBackColor = true;
			this.butBuildGraphics.Click += new System.EventHandler(this.butBuildGraphics_Click);
			// 
			// button7
			// 
			this.button7.Location = new System.Drawing.Point(410, 88);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size(75, 23);
			this.button7.TabIndex = 16;
			this.button7.Text = "button7";
			this.button7.UseVisualStyleBackColor = true;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(917, 498);
			this.tabControl1.TabIndex = 17;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.gbSettings);
			this.tabPage1.Controls.Add(this.chartMain);
			this.tabPage1.Controls.Add(this.groupBox1);
			this.tabPage1.Controls.Add(this.lb);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(909, 472);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Рівномірні сплайни";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.butStartUneven);
			this.tabPage2.Controls.Add(this.chartRegularX);
			this.tabPage2.Controls.Add(this.chartRegularY);
			this.tabPage2.Controls.Add(this.chartUnevenPoints);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(909, 472);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Нерівномірні сплайни";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// butStartUneven
			// 
			this.butStartUneven.Location = new System.Drawing.Point(156, 297);
			this.butStartUneven.Name = "butStartUneven";
			this.butStartUneven.Size = new System.Drawing.Size(119, 45);
			this.butStartUneven.TabIndex = 1;
			this.butStartUneven.Text = "Розпочати";
			this.butStartUneven.UseVisualStyleBackColor = true;
			this.butStartUneven.Click += new System.EventHandler(this.butStartUneven_Click);
			// 
			// chartRegularX
			// 
			chartArea2.Name = "ChartArea1";
			this.chartRegularX.ChartAreas.Add(chartArea2);
			legend2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
			legend2.Name = "Legend1";
			this.chartRegularX.Legends.Add(legend2);
			this.chartRegularX.Location = new System.Drawing.Point(490, 242);
			this.chartRegularX.Name = "chartRegularX";
			series4.ChartArea = "ChartArea1";
			series4.Legend = "Legend1";
			series4.Name = "Series1";
			this.chartRegularX.Series.Add(series4);
			this.chartRegularX.Size = new System.Drawing.Size(411, 227);
			this.chartRegularX.TabIndex = 0;
			this.chartRegularX.Text = "chart1";
			// 
			// chartRegularY
			// 
			chartArea3.Name = "ChartArea1";
			this.chartRegularY.ChartAreas.Add(chartArea3);
			legend3.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
			legend3.Name = "Legend1";
			this.chartRegularY.Legends.Add(legend3);
			this.chartRegularY.Location = new System.Drawing.Point(490, 6);
			this.chartRegularY.Name = "chartRegularY";
			series5.ChartArea = "ChartArea1";
			series5.Legend = "Legend1";
			series5.Name = "Series1";
			this.chartRegularY.Series.Add(series5);
			this.chartRegularY.Size = new System.Drawing.Size(413, 228);
			this.chartRegularY.TabIndex = 0;
			this.chartRegularY.Text = "chart1";
			// 
			// chartUnevenPoints
			// 
			chartArea4.Name = "ChartArea1";
			this.chartUnevenPoints.ChartAreas.Add(chartArea4);
			legend4.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
			legend4.Name = "Legend1";
			this.chartUnevenPoints.Legends.Add(legend4);
			this.chartUnevenPoints.Location = new System.Drawing.Point(8, 6);
			this.chartUnevenPoints.Name = "chartUnevenPoints";
			series6.ChartArea = "ChartArea1";
			series6.Legend = "Legend1";
			series6.Name = "Series1";
			this.chartUnevenPoints.Series.Add(series6);
			this.chartUnevenPoints.Size = new System.Drawing.Size(473, 225);
			this.chartUnevenPoints.TabIndex = 0;
			this.chartUnevenPoints.Text = "chart1";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(917, 498);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.button7);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button5);
			this.Controls.Add(this.button6);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.butSplainS3_2);
			this.Controls.Add(this.butSplainS3_1);
			this.Controls.Add(this.butSplainS3_0);
			this.Controls.Add(this.butSplainS2_2);
			this.Controls.Add(this.butSplainS2_1);
			this.Controls.Add(this.butSplainS2_0);
			this.Name = "MainForm";
			this.Text = "Лабораторна робота №1";
			((System.ComponentModel.ISupportInitialize)(this.chartMain)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.chartRegularX)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.chartRegularY)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.chartUnevenPoints)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataVisualization.Charting.Chart chartMain;
        private System.Windows.Forms.Button butSplainS2_0;
        private System.Windows.Forms.Button butSplainS2_1;
        private System.Windows.Forms.Button butSplainS2_2;
		private System.Windows.Forms.GroupBox gbSettings;
		private System.Windows.Forms.Button butSplainS3_2;
		private System.Windows.Forms.Button butSplainS3_1;
		private System.Windows.Forms.Button butSplainS3_0;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.ListBox lb;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button butClearGraphics;
		private System.Windows.Forms.Button butRebuildGraphics;
		private System.Windows.Forms.Button butBuildGraphics;
		private System.Windows.Forms.Button button7;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.Button butStartUneven;
		public System.Windows.Forms.DataVisualization.Charting.Chart chartUnevenPoints;
		public System.Windows.Forms.DataVisualization.Charting.Chart chartRegularY;
		public System.Windows.Forms.DataVisualization.Charting.Chart chartRegularX;
	}
}

