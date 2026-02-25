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
            // Enable resizing redraw
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

            // Determine scales
            // We want to show a moving window or the whole history? 
            // Let's show the whole history for now, up to a limit, or auto-scale.
            // For educational purposes, seeing the whole sine wave develop is nice.
            
            // Determine scales
            // Use a fixed time window for scrolling
            double windowSize = 365 * 86400.0; // Show last 1 year of data
            double maxTime = Data[Data.Count - 1].Time;
            double minTime = Math.Max(Data[0].Time, maxTime - windowSize);
            
            // X-axis min label should follow scrolling
            float displayMinTime = (float)minTime;

            // Velocity range: find max absolute value to keep 0 in center
            // We'll scan the whole data for maxVel to keep the Y-scale stable, or just the window?
            // Stable Y-scale is usually better for comparison.
            double maxVel = Data.Select(d => Math.Abs(d.RadialVelocity)).Max();
            if (maxVel < 1) maxVel = 1; 
            maxVel *= 1.1; 

            // Coordinate mapping functions
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

                    // Smooth Color Transition Logic (Purple to Red)
                    float rvFactor = (float)Math.Max(-1, Math.Min(1, avgRV / 50.0));
                    Color segmentColor;
                    if (rvFactor > 0)
                    {
                        int r = (int)(211 + (255 - 211) * rvFactor);
                        int gValue = (int)(211 - 211 * rvFactor);
                        int b = (int)(211 - 211 * rvFactor);
                        segmentColor = Color.FromArgb(255, r, gValue, b);
                    }
                    else
                    {
                        float absFactor = Math.Abs(rvFactor);
                        int r = (int)(211 + (160 - 211) * absFactor);
                        int gValue = (int)(211 + (32 - 211) * absFactor);
                        int b = (int)(211 + (240 - 211) * absFactor);
                        segmentColor = Color.FromArgb(255, r, gValue, b);
                    }

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

                // Highlight current point
                PointF lastPoint = new PointF(MapX(Data[Data.Count - 1].Time), MapY(Data[Data.Count - 1].RadialVelocity));
                g.FillEllipse(Brushes.White, lastPoint.X - 3, lastPoint.Y - 3, 6, 6);
                
                // Doppler shift explanation
                string shiftText = "";
                Color shiftColor = Color.White;
                double currentRV = Data[Data.Count-1].RadialVelocity;
                
                if (currentRV > 1) { shiftText = "REDSHIFT (Receding)"; shiftColor = Color.LightCoral; }
                else if (currentRV < -1) { shiftText = "PURPLESHIFT (Approaching)"; shiftColor = Color.Plum; }
                else { shiftText = "STABLE"; shiftColor = Color.Gray; }

                g.DrawString(shiftText, new Font("Arial", 10, FontStyle.Bold), new SolidBrush(shiftColor), w - 250, marginTop);
            }

            // Draw Labels (Simple)
            Font font = new Font("Arial", 8); 
            Brush brush = Brushes.White;
            g.DrawString($"RV Max: {maxVel:F2} m/s", font, brush, 5, marginTop);
            g.DrawString($"RV Min: {-maxVel:F2} m/s", font, brush, 5, h - marginBottom - 15);
            g.DrawString($"Time: {(maxTime - minTime)/86400.0:F1} days", font, brush, w - 100, h - 20);
            
            // Explanation of detection
            g.DrawString("Star's wobble causes Doppler shift in light spectrum", font, Brushes.Gray, marginLeft, 5);
        }
    }
}
