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
        public List<NatureOfCodeTest.Model.SimulationSample> Data { get; set; }

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

            // Determine scales using TrackBars
            double windowSize = trkXScale.Value * 86400.0; // Show window size from TrackBar
            double maxTime = Data[Data.Count - 1].Time;
            double minTime = Math.Max(Data[0].Time, maxTime - windowSize);
            
            // X-axis min label should follow scrolling
            float displayMinTime = (float)minTime;

            // Velocity range: use TrackBar for scaling (1 to 1000)
            // trkYScale value 100 = 100% of auto-calculated maxVel
            // We'll use it to multiply the base auto-scale or just use it as a divisor
            double baseMaxVel = Data.Select(d => Math.Abs(d.RadialVelocity)).Max();
            if (baseMaxVel < 1e-3) baseMaxVel = 1e-3; // Avoid div by zero
            
            // Map trkYScale (1-1000) to a range (e.g. 0.01 to 10.0 x baseMaxVel)
            // But user might want absolute control. Let's just make it a multiplier for a "standard" range.
            double maxVel = (baseMaxVel * 1.5) * (100.0 / trkYScale.Value); 

            float MapX(double t)
            {
                if (t < minTime) return -1000; // Off screen
                return marginLeft + (float)((t - minTime) / windowSize * (w - marginLeft - marginRight));
            }

            float MapY(double v)
            {
                float graphHeight = h - marginBottom - marginTop;
                float halfHeight = graphHeight / 2;
                float centerY = marginTop + halfHeight;
                return centerY - (float)(v / maxVel * halfHeight);
            }

            // Draw Zero Line
            Pen zeroPen = new Pen(Color.Gray, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };
            float y0 = MapY(0);
            g.DrawLine(zeroPen, marginLeft, y0, w - marginRight, y0);

            // Draw Data
            if (Data.Count > 1)
            {
                // Find index to start drawing from for performance
                int startIndex = 0;
                for (int i = 0; i < Data.Count; i++) { if (Data[i].Time >= minTime) { startIndex = Math.Max(0, i - 1); break; } }

                for (int i = startIndex + 1; i < Data.Count; i++)
                {
                    double rvStart = Data[i - 1].RadialVelocity;
                    double rvEnd = Data[i].RadialVelocity;
                    double avgRV = (rvStart + rvEnd) / 2.0;

                    // Same color occillation as Doppler shift visulisation 
                    double wavelength = ColorUtils.GetShiftedWavelength(avgRV);
                    Color segmentColor = ColorUtils.WavelengthToRGB(wavelength, 255);

                    using (Pen segmentPen = new Pen(segmentColor, 2))
                    {
                        float x1 = MapX(Data[i - 1].Time);
                        float x2 = MapX(Data[i].Time);
                        if (x2 >= marginLeft)
                        {
                            g.DrawLine(segmentPen, Math.Max(marginLeft, x1), MapY(rvStart), x2, MapY(rvEnd));
                        }
                    }
                    
                    if (i % 5 == 0)
                    {
                        float x = MapX(Data[i].Time);
                        if (x >= marginLeft)
                        {
                            float y = MapY(Data[i].RadialVelocity);
                            float spread = (float)(5 + Math.Abs(avgRV) / maxVel * 5); 
                            g.DrawLine(new Pen(Color.FromArgb(80, segmentColor), 1), x, y - spread, x, y + spread);
                        }
                    }
                }

                // Highlight current drawing point
                PointF lastPoint = new PointF(MapX(Data[Data.Count - 1].Time), MapY(Data[Data.Count - 1].RadialVelocity));
                g.FillEllipse(Brushes.White, lastPoint.X - 3, lastPoint.Y - 3, 6, 6);
                
                double currentRV = Data[Data.Count - 1].RadialVelocity;
                double textWavelength = ColorUtils.GetShiftedWavelength(currentRV);
                string shiftText = "";
                Color shiftColor = ColorUtils.WavelengthToRGB(textWavelength, 255);
                
                if (currentRV > 1) { shiftText = "REDSHIFT (Receding)"; }
                else if (currentRV < -1) { shiftText = "BLUE/VIOLET SHIFT (Approaching)"; }
                else { shiftText = "STABLE"; shiftColor = Color.Gray; }

                g.DrawString(shiftText, new Font("Arial", 10, FontStyle.Bold), new SolidBrush(shiftColor), w - 250, marginTop);
            }

            Font font = new Font("Arial", 8); 
            Brush brush = Brushes.White;
            g.DrawString($"RV Max (visible): {maxVel:F2} m/s", font, brush, 5, marginTop);
            g.DrawString($"RV Min (visible): {-maxVel:F2} m/s", font, brush, 5, h - marginBottom - 15);
            g.DrawString($"Time Window: {trkXScale.Value} days", font, brush, w - 150, h - 20);
            
            g.DrawString("Star's wobble causes Doppler shift in light spectrum. Adjust sliders below to scale.", font, Brushes.Gray, marginLeft, 5);
        }

        private void trkScale_Scroll(object sender, EventArgs e)
        {
            lblXScale.Text = $"X-Axis Scale: {trkXScale.Value} Days";
            lblYScale.Text = $"Y-Axis Zoom: {trkYScale.Value}%";
            pnlRVGraph.Invalidate();
        }
    }
}
