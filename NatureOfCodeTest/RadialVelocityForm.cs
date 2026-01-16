using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NatureOfCodeTest.Model;

namespace NatureOfCodeTest
{
    public partial class RadialVelocityForm : Form
    {
        public List<SimulationSample> Data { get; set; }

        public RadialVelocityForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.pnlRVGraph.Paint += PnlRVGraph_Paint;
            this.pnlRVGraph.Resize += (s, e) => this.pnlRVGraph.Invalidate();
        }

        private void RadialVelocityForm_Load(object sender, EventArgs e)
        {

        }

        public void Redraw()
        {
            if (this.Visible && !this.IsDisposed)
            {
                pnlRVGraph.Invalidate();
            }
        }

        private void PnlRVGraph_Paint(object sender, PaintEventArgs e)
        {
            if (Data == null || Data.Count < 2) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            float w = pnlRVGraph.Width;
            float h = pnlRVGraph.Height;

            // Margins
            float marginLeft = 50;
            float marginBottom = 30;
            float marginTop = 20;
            float marginRight = 20;

            // Draw Axes
            Pen axisPen = new Pen(Color.White, 2);
            g.DrawLine(axisPen, marginLeft, marginTop, marginLeft, h - marginBottom); // Y Axis
            g.DrawLine(axisPen, marginLeft, h - marginBottom, w - marginRight, h - marginBottom); // X Axis

            
            double minTime = Data[0].Time;
            double maxTime = Data[Data.Count - 1].Time;
            if (maxTime == minTime) maxTime += 1; // Avoid div by zero

            // Velocity range: find max absolute value to keep 0 in center
            double maxVel = Data.Select(d => Math.Abs(d.RadialVelocity)).Max();
            if (maxVel < 1) maxVel = 1; // Default min scale
            maxVel *= 1.1; // Add padding

            // Coordinate mapping functions
            float MapX(double t)
            {
                return marginLeft + (float)((t - minTime) / (maxTime - minTime) * (w - marginLeft - marginRight));
            }

            float MapY(double v)
            {
                float graphHeight = h - marginBottom - marginTop;
                float halfHeight = graphHeight / 2;
                float centerY = marginTop + halfHeight;
                
                // v positive -> up (smaller Y), v negative -> down (larger Y)
                return centerY - (float)(v / maxVel * halfHeight);
            }

            // Draw Zero Line
            Pen zeroPen = new Pen(Color.Gray, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };
            float y0 = MapY(0);
            g.DrawLine(zeroPen, marginLeft, y0, w - marginRight, y0);

            // Draw Data
            if (Data.Count > 1)
            {
                PointF[] points = new PointF[Data.Count];
                for(int i=0; i<Data.Count; i++)
                {
                    points[i] = new PointF(MapX(Data[i].Time), MapY(Data[i].RadialVelocity));
                }
                
                g.DrawLines(new Pen(Color.Lime, 2), points);
            }

            // Draw Labels (Simple)
            Font font = new Font("Arial", 8);
            Brush brush = Brushes.White;
            g.DrawString($"RV Max: {maxVel:F2} m/s", font, brush, 5, marginTop);
            g.DrawString($"RV Min: {-maxVel:F2} m/s", font, brush, 5, h - marginBottom - 15);
            g.DrawString($"Time: {(maxTime - minTime)/86400.0:F1} days", font, brush, w - 100, h - 20);
        }
    }
}
