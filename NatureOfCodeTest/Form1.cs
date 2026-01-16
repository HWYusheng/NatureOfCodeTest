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
        private SimulationEngine engine;
        private Timer simulationTimer;
        private RadialVelocityForm rvForm;

        private float scaleAUToPixels = 150f; // 1 AU = 150 pixels

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;            
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
            Star sun = new Star
            {
                Name = "Sun",
                Mass = PhysicalConstants.SolarMass,
                Position = Vector2.Zero
            };

            Planet earth = new Planet
            {
                Name = "Earth",
                Mass = 5.972e24, // kg
                Orbit = new OrbitalElements
                {
                    SemiMajorAxis = PhysicalConstants.AU,
                    Eccentricity = 0.0167, // Earth's eccentricity
                    Inclination = 0,
                    ArgumentOfPeriapsis = 0, // Simplified
                    MeanAnomalyAtEpoch = 0,
                    EpochTime = 0
                }
            };
            earth.Position = new Vector2((float)PhysicalConstants.AU, 0);

            engine = new SimulationEngine
            {
                HostStar = sun,
                OrbitingPlanet = earth,
                TimeStep = 86400 // 1 day per step
            };
        }

        private void InitializeTimer()
        {
            simulationTimer = new Timer();
            simulationTimer.Interval = 16; 
            simulationTimer.Tick += SimulationTimer_Tick;
            simulationTimer.Start();
        }

        private void SimulationTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < 5; i++) // Speed 5x
            {
                engine.Step();
            }

            // Redraw orbital map
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
            float centerX = w / 2f;
            float centerY = h / 2f;

            // Draw Sun
            float sunSize = 40;
            RectangleF sunRect = new RectangleF(centerX - sunSize / 2, centerY - sunSize / 2, sunSize, sunSize);
            g.FillEllipse(Brushes.Gold, sunRect);
            g.DrawString(engine.HostStar.Name, this.Font, Brushes.White, sunRect.Location);

            // Draw Orbit Path (Estimate ellipse)
            // Only simplification. True Keplerian orbit drawing would need to transform 
            
            float semiMajorPixels = (float)(engine.OrbitingPlanet.Orbit.SemiMajorAxis / PhysicalConstants.AU * scaleAUToPixels);
            
            float c = semiMajorPixels * (float)engine.OrbitingPlanet.Orbit.Eccentricity;
            
            
            // Center of ellipse relative to Star (0,0) is at (-c, 0) if periapsis is at 0 degrees.
            float ellipseCenterX = centerX - c;
            float ellipseCenterY = centerY;
            float semiMinorPixels = semiMajorPixels * (float)Math.Sqrt(1 - engine.OrbitingPlanet.Orbit.Eccentricity * engine.OrbitingPlanet.Orbit.Eccentricity);
            
            RectangleF orbitRect = new RectangleF(ellipseCenterX - semiMajorPixels, ellipseCenterY - semiMinorPixels, semiMajorPixels * 2, semiMinorPixels * 2);
            g.DrawEllipse(new Pen(Color.FromArgb(50, Color.White)), orbitRect);


            // Draw Planet
            Vector2 pPos = engine.OrbitingPlanet.Position;
            float pX = centerX + (pPos.X / (float)PhysicalConstants.AU * scaleAUToPixels);
            float pY = centerY - (pPos.Y / (float)PhysicalConstants.AU * scaleAUToPixels);

            float planetSize = 15;
            g.FillEllipse(Brushes.CornflowerBlue, pX - planetSize / 2, pY - planetSize / 2, planetSize, planetSize);
            g.DrawString(engine.OrbitingPlanet.Name, this.Font, Brushes.White, pX + 10, pY);

            // Time
            string timeStr = $"Day: {engine.CurrentTime / 86400.0:F1}";
            g.DrawString(timeStr, new Font("Arial", 12, FontStyle.Bold), Brushes.White, 10, 10);
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
