using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NatureOfCodeTest.Model;

namespace NatureOfCodeTest
{
    public partial class Form1 : Form
    {
        private NatureOfCodeTest.Model.SimulationEngine engine;
        private Timer simulationTimer;
        private RadialVelocityForm rvForm;

        private float scaleAUToPixels = 300f; // 1 AU = 300 pixels (Increased from 150)

        // https://stackoverflow.com/questions/61261191/how-do-you-remove-the-flickering-in-the-paint-method
        public Form1()
        {
            InitializeComponent();
            EnableDoubleBuffering();
            this.lblData.Width = 50;
            this.lblData.Height = 100;
            this.lblData.ForeColor = Color.AntiqueWhite;

            this.pnlOrbitalMap.Paint += PnlOrbitalMap_Paint;

            this.pnlOrbitalMap.Resize += (s, e) => this.pnlOrbitalMap.Invalidate();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeSimulation();
            InitializeTimer();
        }

        private void InitializeSimulation()
        {
            // Setup objects
            NatureOfCodeTest.Model.Star sun = new NatureOfCodeTest.Model.Star
            {
                Name = "Sun",
                Mass = NatureOfCodeTest.Model.PhysicalConstants.SolarMass,
                Position = Vector2.Zero
            };

            NatureOfCodeTest.Model.Planet earth = new NatureOfCodeTest.Model.Planet
            {
                Name = "Earth",
                Mass = 5.972e24, // in kg
                Orbit = new NatureOfCodeTest.Model.OrbitalElements
                {
                    SemiMajorAxis = NatureOfCodeTest.Model.PhysicalConstants.AU,
                    Eccentricity = 0.0167, // Earth's eccentricity
                    Inclination = 0,
                    ArgumentOfPeriapsis = 0,
                    MeanAnomalyAtEpoch = 0,
                    EpochTime = 0
                }
            };
            earth.Position = new Vector2((float)NatureOfCodeTest.Model.PhysicalConstants.AU, 0);

            engine = new SimulationEngine
            {
                HostStar = sun,
                OrbitingPlanet = earth,
                TimeStep = 60*60*24 // 1 day per step
            };
        }

        private void InitializeTimer()
        {
            simulationTimer = new Timer();
            simulationTimer.Interval = 240; 
            simulationTimer.Tick += SimulationTimer_Tick;
            simulationTimer.Start();
        }

        private void SimulationTimer_Tick(object sender, EventArgs e)
        {

            engine.Step();

            pnlOrbitalMap.Invalidate();

            if (rvForm != null && !rvForm.IsDisposed && rvForm.Visible)
            {
                rvForm.Redraw();
            }
        }

        private void PnlOrbitalMap_Paint(object sender, PaintEventArgs e)
        {
            if (engine == null) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int w = pnlOrbitalMap.Width;
            int h = pnlOrbitalMap.Height;
            
            // Shift system slight toward the right
            float centerX = w * 0.75f; 
            float centerY = h / 2f;

            // Wobble Amplification for modeling purposes
            float wobbleAmplify = 12000f; // Increased for better visibility
            Vector2 starPos = engine.HostStar.Position;
            float starX = centerX + (starPos.X / (float)NatureOfCodeTest.Model.PhysicalConstants.AU * scaleAUToPixels * wobbleAmplify);
            float starY = centerY - (starPos.Y / (float)NatureOfCodeTest.Model.PhysicalConstants.AU * scaleAUToPixels * wobbleAmplify);

            // Draw Observer (Telescope) to the left
            float observerX = 60;
            float observerY = centerY;
            g.FillRectangle(Brushes.DimGray, observerX - 20, observerY - 5, 30, 10); // Telescope body
            g.FillEllipse(Brushes.Silver, observerX - 25, observerY - 8, 10, 16); // Lens
            g.DrawString("OBSERVER\n(Telescope)", this.Font, Brushes.LightSkyBlue, observerX - 30, observerY + 15);

            // Light Wave Visualization (Static Sine Wave Link)
            if (engine.Samples.Count > 0)
            {
                var latestSample = engine.Samples.Last();
                double currentRV = latestSample.RadialVelocity;

                // Find max RV for normalization (using historical max or fixed scale)
                float maxRV_Abs = (float)engine.Samples.Max(s => Math.Abs(s.RadialVelocity));
                if (maxRV_Abs < 1e-9) maxRV_Abs = 1.0f;

                float rvFactor = (float)(currentRV / maxRV_Abs);

                // Wave Properties
                float baseWavelength = 40f; 
                // Wavelength changes with RV (Doppler effect)
                // Positive RV (receding) -> Longer wavelength (Redshift)
                // Negative RV (approaching) -> Shorter wavelength (Blueshift)
                float currentWavelength = baseWavelength * (1.0f + rvFactor * 0.5f); 
                float amplitude = 40f;

                // Color mapping
                Color waveColor;
                if (rvFactor > 0)
                {
                    // Green to Red
                    int r = (int)(255 * rvFactor);
                    int gValue = (int)(255 * (1 - rvFactor));
                    waveColor = Color.FromArgb(200, r, gValue, 0);
                }
                else
                {
                    // Green to Purple
                    float absFactor = Math.Abs(rvFactor);
                    int r = (int)(160 * absFactor);
                    int gValue = (int)(255 * (1 - absFactor) + 32 * absFactor);
                    int bValue = (int)(240 * absFactor);
                    waveColor = Color.FromArgb(200, r, gValue, bValue);
                }

                // Draw the wave from star to observer
                float distTotal = starX - observerX;
                int segments = (int)distTotal / 2; // 2-pixel steps
                if (segments > 0)
                {
                    PointF[] wavePoints = new PointF[segments + 1];
                    for (int i = 0; i <= segments; i++)
                    {
                        float progress = (float)i / segments;
                        float x = starX - (progress * distTotal);
                        
                        // Sine wave math: y = sin(2*pi * x / wavelength)
                        // Using progress * distTotal as the local x-offset from the star
                        float localX = progress * distTotal;
                        float yOffset = (float)(amplitude * Math.Sin(2 * Math.PI * localX / currentWavelength));
                        
                        wavePoints[i] = new PointF(x, starY + yOffset);
                    }

                    using (Pen wavePen = new Pen(waveColor, 3))
                    {
                        g.DrawLines(wavePen, wavePoints);
                    }
                }
            }

            // Draw Star (Sun)
            float sunSize = 200;
            RectangleF sunRect = new RectangleF(starX - sunSize / 2, starY - sunSize / 2, sunSize, sunSize);
            g.FillEllipse(Brushes.Gold, sunRect);

            // CROSSHAIRS: Different for Star and Barycenter. For showing how the star fluctuate
            // 1. Star Crosshair (Yellow)
            using (Pen starCrossPen = new Pen(Color.Red, 1))
            {
                g.DrawLine(starCrossPen, starX - 10, starY, starX + 10, starY);
                g.DrawLine(starCrossPen, starX, starY - 10, starX, starY + 10);
            }
            
            // 2. Barycenter Crosshair (Cyan)
            using (Pen baryCrossPen = new Pen(Color.Cyan, 1))
            {
                g.DrawLine(baryCrossPen, centerX - 8, centerY, centerX + 8, centerY);
                g.DrawLine(baryCrossPen, centerX, centerY - 8, centerX, centerY + 8);
                g.DrawString("Barycenter", this.Font, Brushes.Cyan, centerX + 10, centerY + 10);
            }

            // Orbit Path (Dashed)
            float semiMajorPixels = (float)(engine.OrbitingPlanet.Orbit.SemiMajorAxis / NatureOfCodeTest.Model.PhysicalConstants.AU * scaleAUToPixels);
            float c = semiMajorPixels * (float)engine.OrbitingPlanet.Orbit.Eccentricity;
            float ellipseCenterX = centerX - c;
            float ellipseCenterY = centerY;
            float semiMinorPixels = semiMajorPixels * (float)Math.Sqrt(1 - engine.OrbitingPlanet.Orbit.Eccentricity * engine.OrbitingPlanet.Orbit.Eccentricity);
            
            RectangleF orbitRect = new RectangleF(ellipseCenterX - semiMajorPixels, ellipseCenterY - semiMinorPixels, semiMajorPixels * 2, semiMinorPixels * 2);
            using (Pen dashedPen = new Pen(Color.FromArgb(80, Color.White), 1))
            {
                dashedPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                g.DrawEllipse(dashedPen, orbitRect);
            }

            // Planet
            Vector2 pPos = engine.OrbitingPlanet.Position;
            float pX = centerX + (pPos.X / (float)NatureOfCodeTest.Model.PhysicalConstants.AU * scaleAUToPixels);
            float pY = centerY - (pPos.Y / (float)NatureOfCodeTest.Model.PhysicalConstants.AU * scaleAUToPixels);
            float planetSize = 15;
            g.FillEllipse(Brushes.CornflowerBlue, pX - planetSize / 2, pY - planetSize / 2, planetSize, planetSize);

            // Radial line
            using (Pen radialPen = new Pen(Color.FromArgb(40, Color.Lime), 1))
            {
                g.DrawLine(radialPen, starX, starY, pX, pY);
            }

            // ----------------------- Labels (this is where need to be fix, turn it into a label to prevent)
            double rv = engine.Samples.Count > 0 ? engine.Samples.Last().RadialVelocity : 0;
            string timeStr = $"Day: {engine.CurrentTime / 86400.0:F1}";
            string rvStr = $"Radial Velocity: {rv:F2} m/s";
            lblData.Text = timeStr + "\n" + rvStr;
        }

        private void EnableDoubleBuffering()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
        }

        private void btnShowRV_Click(object sender, EventArgs e)
        {
            if (rvForm == null || rvForm.IsDisposed)
            {
                rvForm = new RadialVelocityForm();
                rvForm.Data = engine.Samples;
                rvForm.Show();
            }
            else
            {
                rvForm.BringToFront();
            }
        }
    }
}
