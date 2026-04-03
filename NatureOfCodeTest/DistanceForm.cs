using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NatureOfCodeTest.Model;

namespace NatureOfCodeTest
{
    public partial class DistanceForm : Form
    {
        public List<SimulationSample> Data { get; set; }
        
        private const double BaseDistance = 4.22 * 9.461e15;

        public DistanceForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.pnlDistanceGraph.Paint += PnlDistanceGraph_Paint;
            this.pnlDistanceGraph.Resize += (s, e) => this.pnlDistanceGraph.Invalidate();
        }

        public void Redraw()
        {
            if (this.Visible && !this.IsDisposed)
            {
                pnlDistanceGraph.Invalidate();
            }
        }

        private void PnlDistanceGraph_Paint(object sender, PaintEventArgs e)
        {
            if (Data == null || Data.Count < 2) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            float w = pnlDistanceGraph.Width;
            float h = pnlDistanceGraph.Height;

            float marginLeft = 70;
            float marginBottom = 50;
            float marginTop = 30;
            float marginRight = 30;

            // Draw Axes
            Pen axisPen = new Pen(Color.LimeGreen, 2);
            g.DrawLine(axisPen, marginLeft, marginTop, marginLeft, h - marginBottom);
            g.DrawLine(axisPen, marginLeft, h - marginBottom, w - marginRight, h - marginBottom);

            // Time window from TrackBar in designer (reusing scaling logic idea)
            double windowSize = trkXScale.Value * 86400.0;
            double maxTime = Data[Data.Count - 1].Time;
            double minTime = Math.Max(Data[0].Time, maxTime - windowSize);

            // Calculate min/max distance for Y-axis
            // D(t) = BaseDistance + StarPos.X
            // We only care about the delta because BaseDistance is HUGE
            double maxDelta = Data.Select(d => (double)d.StarPosition.X).Max();
            double minDelta = Data.Select(d => (double)d.StarPosition.X).Min();
            double spread = Math.Max(1.0, (maxDelta - minDelta) * 1.2);
            double midDelta = (maxDelta + minDelta) / 2.0;

            float MapX(double t)
            {
                if (t < minTime) return -1000;
                return marginLeft + (float)((t - minTime) / windowSize * (w - marginLeft - marginRight));
            }

            float MapY(double delta)
            {
                float graphHeight = h - marginBottom - marginTop;
                float centerY = marginTop + graphHeight / 2;
                return centerY - (float)((delta - midDelta) / spread * graphHeight);
            }

            // Draw Data
            using (Pen graphPen = new Pen(Color.Cyan, 2))
            {
                List<PointF> points = new List<PointF>();
                int startIndex = 0;
                for (int i = 0; i < Data.Count; i++) { if (Data[i].Time >= minTime) { startIndex = Math.Max(0, i - 1); break; } }

                for (int i = startIndex; i < Data.Count; i++)
                {
                    float x = MapX(Data[i].Time);
                    float y = MapY(Data[i].StarPosition.X);
                    if (x >= marginLeft)
                    {
                        points.Add(new PointF(x, y));
                    }
                }

                if (points.Count > 1)
                {
                    g.DrawLines(graphPen, points.ToArray());
                }
            }

            // Labels
            Font font = new Font("Arial", 9);
            Brush brush = Brushes.White;
            g.DrawString($"Base Distance: {BaseDistance:E3} m (4.22 LY)", font, Brushes.Gray, marginLeft, 5);
            g.DrawString($"Max: +{maxDelta:F0} m", font, brush, 5, marginTop);
            g.DrawString($"Min: {minDelta:F0} m", font, brush, 5, h - marginBottom - 15);
            g.DrawString("Relative Distance to Observer", font, Brushes.Cyan, w / 2 - 80, marginTop - 20);
            g.DrawString($"Time Window: {trkXScale.Value} days", font, brush, w - 150, h - 25);
        }

        private void trkXScale_Scroll(object sender, EventArgs e)
        {
            lblXScale.Text = $"X-Axis Scale: {trkXScale.Value} Days";
            pnlDistanceGraph.Invalidate();
        }

        private void DistanceForm_Load(object sender, EventArgs e)
        {

        }
    }
}
