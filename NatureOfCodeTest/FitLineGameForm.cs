using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;
using NatureOfCodeTest.Model;

namespace NatureOfCodeTest
{
    public class FitLineGameForm : Form
    {
        private Panel pnlGraph;
        private TrackBar trkAmplitude;
        private TrackBar trkPeriod;
        private TrackBar trkPhase;
        
        private Label lblScore;
        private Label lblAmp;
        private Label lblPeriod;
        private Label lblPhase;

        private Button btnNewGame;
        private Button btnSubmit;

        private SimulationEngine engine;
        private List<PointF> noisyDataPoints = new List<PointF>();
        
        private double trueMaxVel = 0;
        private double windowSizeDays = 1000;
        
        private Random random = new Random();
        
        public FitLineGameForm()
        {
            InitializeComponentsRunTime();
            StartNewGame();
        }

        private void InitializeComponentsRunTime()
        {
            this.Text = "Fit Line Game";
            this.Size = new Size(1000, 600);
            this.BackColor = Color.Black;
            this.ForeColor = Color.White;

            pnlGraph = new Panel
            {
                Location = new Point(10, 10),
                Size = new Size(1000 - 40, 400),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                BorderStyle = BorderStyle.FixedSingle
            };
            pnlGraph.Paint += PnlGraph_Paint;
            pnlGraph.Resize += (s, e) => pnlGraph.Invalidate();
            this.Controls.Add(pnlGraph);

            int currentY = 420;

            // Amplitude
            lblAmp = new Label { Location = new Point(10, currentY), Size = new Size(150, 20), Text = "Amplitude: 0%" };
            this.Controls.Add(lblAmp);
            
            trkAmplitude = new TrackBar { Location = new Point(160, currentY), Size = new Size(400, 45), Minimum = 10, Maximum = 200, Value = 100, TickFrequency = 10 };
            trkAmplitude.Scroll += (s, e) => { pnlGraph.Invalidate(); lblAmp.Text = $"Amplitude: {trkAmplitude.Value}%"; };
            this.Controls.Add(trkAmplitude);

            currentY += 45;

            // Period
            lblPeriod = new Label { Location = new Point(10, currentY), Size = new Size(150, 20), Text = "Period: 100 Days" };
            this.Controls.Add(lblPeriod);
            
            trkPeriod = new TrackBar { Location = new Point(160, currentY), Size = new Size(400, 45), Minimum = 50, Maximum = 1500, Value = 365, TickFrequency = 50 };
            trkPeriod.Scroll += (s, e) => { pnlGraph.Invalidate(); lblPeriod.Text = $"Period: {trkPeriod.Value} Days"; };
            this.Controls.Add(trkPeriod);

            currentY += 45;

            // Phase
            lblPhase = new Label { Location = new Point(10, currentY), Size = new Size(150, 20), Text = "Phase: 0°" };
            this.Controls.Add(lblPhase);
            
            trkPhase = new TrackBar { Location = new Point(160, currentY), Size = new Size(400, 45), Minimum = 0, Maximum = 360, Value = 180, TickFrequency = 15 };
            trkPhase.Scroll += (s, e) => { pnlGraph.Invalidate(); lblPhase.Text = $"Phase: {trkPhase.Value}°"; };
            this.Controls.Add(trkPhase);

            // Buttons
            btnNewGame = new Button { Location = new Point(580, 420), Size = new Size(150, 40), Text = "New System", ForeColor = Color.Black, Font = new Font("Arial", 10, FontStyle.Bold) };
            btnNewGame.Click += (s, e) => StartNewGame();
            this.Controls.Add(btnNewGame);

            btnSubmit = new Button { Location = new Point(740, 420), Size = new Size(150, 40), Text = "Submit Fit", ForeColor = Color.Black, Font = new Font("Arial", 10, FontStyle.Bold), BackColor = Color.LightGreen };
            btnSubmit.Click += BtnSubmit_Click;
            this.Controls.Add(btnSubmit);

            lblScore = new Label { Location = new Point(580, 480), Size = new Size(310, 80), Text = "Fit the line to the data points!", Font = new Font("Arial", 12, FontStyle.Bold), ForeColor = Color.LightSkyBlue };
            this.Controls.Add(lblScore);
        }

        private void StartNewGame()
        {
            lblScore.Text = "Fit the line to the data points!\nAdjust sliders and click Submit.";
            lblScore.ForeColor = Color.LightSkyBlue;

            // Randomize parameters
            double starMass = (0.5 + random.NextDouble() * 1.5) * NatureOfCodeTest.Model.PhysicalConstants.SolarMass;
            // Planet mass from Jupiter mass (1.898e27) to 20x Jupiter
            double jupiterMass = 1.898e27;
            double planetMass = (0.5 + random.NextDouble() * 19.5) * jupiterMass; 
            
            // Semi-major axis from 0.1 AU to 1.5 AU
            double semiMajorAU = 0.1 + random.NextDouble() * 1.4;
            double semiMajor = semiMajorAU * NatureOfCodeTest.Model.PhysicalConstants.AU;

            Star sun = new NatureOfCodeTest.Model.Star
            {
                Name = "Random Star",
                Mass = starMass,
                Position = Vector2.Zero
            };

            Planet planet = new NatureOfCodeTest.Model.Planet
            {
                Name = "Random Exoplanet",
                Mass = planetMass,
                Orbit = new NatureOfCodeTest.Model.OrbitalElements
                {
                    SemiMajorAxis = semiMajor,
                    Eccentricity = 0.0, // Keeping it circular for simple sine fitting
                    Inclination = 0,
                    ArgumentOfPeriapsis = 0,
                    MeanAnomalyAtEpoch = 0,
                    EpochTime = 0
                }
            };
            planet.Position = new Vector2((float)semiMajor, 0);

            engine = new SimulationEngine
            {
                HostStar = sun,
                OrbitingPlanet = planet,
                TimeStep = 86400 * 2 // 2 days
            };

            // Calculate true period to set reasonable window size
            double totalMass = starMass + planetMass;
            double n = Math.Sqrt(NatureOfCodeTest.Model.PhysicalConstants.G * totalMass / Math.Pow(semiMajor, 3));
            double periodSec = 2 * Math.PI / n;
            double periodDays = periodSec / 86400.0;
            
            windowSizeDays = periodDays * 2.5; // Show 2.5 periods

            // Run simulation 
            engine.Samples.Clear();
            noisyDataPoints.Clear();
            double timeEnd = windowSizeDays * 86400;
            
            while (engine.CurrentTime <= timeEnd)
            {
                engine.Step();
            }

            trueMaxVel = engine.Samples.Count > 0 ? engine.Samples.Select(s => Math.Abs(s.RadialVelocity)).Max() : 1;
            if (trueMaxVel == 0) trueMaxVel = 1e-5;

            // Generate noisy points
            // Take around 60 observation points
            int stepInterval = Math.Max(1, engine.Samples.Count / 60);
            for (int i = 0; i < engine.Samples.Count; i += stepInterval)
            {
                if (i >= engine.Samples.Count) break;
                var sample = engine.Samples[i];
                
                // Add noise (10% to 20% of true max velocity)
                double noiseLevel = trueMaxVel * 0.15;
                double noise = (random.NextDouble() - 0.5) * 2.0 * noiseLevel;
                
                // Add some random dropouts (clouds, telescope unavailable)
                if (random.NextDouble() > 0.1) // 90% chance to have data
                {
                   noisyDataPoints.Add(new PointF((float)(sample.Time / 86400.0), (float)(sample.RadialVelocity + noise)));
                }
            }

            // Set trackbar ranges dynamically, placing current value randomly to avoid giving hints
            trkAmplitude.Value = random.Next(10, 200);
            
            // Period from 0.5x to 2.5x the true period
            int pMin = Math.Max(10, (int)(periodDays * 0.5));
            int pMax = (int)(periodDays * 2.5);
            trkPeriod.Minimum = pMin;
            trkPeriod.Maximum = pMax;
            trkPeriod.Value = random.Next(pMin, pMax);

            trkPhase.Value = random.Next(0, 360);
            
            lblAmp.Text = $"Amplitude: {trkAmplitude.Value}%";
            lblPeriod.Text = $"Period: {trkPeriod.Value} Days";
            lblPhase.Text = $"Phase: {trkPhase.Value}°";

            pnlGraph.Invalidate();
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            if (noisyDataPoints.Count == 0 || trueMaxVel == 0) return;

            // Calculate actual error vs User curve
            // Score based on Mean Absolute Error
            double totalError = 0;
            
            double userAmp = trueMaxVel * (trkAmplitude.Value / 100.0);
            double userPeriodDays = trkPeriod.Value;
            double userPhaseRad = trkPhase.Value * Math.PI / 180.0;

            foreach (var pt in noisyDataPoints)
            {
                double timeSec = pt.X * 86400.0;
                double userPeriodSec = userPeriodDays * 86400.0;

                // Engine initial config uses vx = -sin(E), so the fundamental wave is a negative sine.
                // We'll let user's wave be evaluated standard way to allow them to fit it.
                // We use standard positive sine: v(t) = A * sin(2pi * t / P + phase)
                double userRV = userAmp * Math.Sin(2 * Math.PI * timeSec / userPeriodSec + userPhaseRad);
                
                double rvDiff = Math.Abs(userRV - pt.Y);
                totalError += rvDiff;
            }

            double meanError = totalError / noisyDataPoints.Count;
            // Max tolerable error is ~ amp
            double scorePercent = 100.0 * (1.0 - (meanError / (trueMaxVel * 1.5)));
            if (scorePercent < 0) scorePercent = 0;

            if (scorePercent >= 90)
            {
                lblScore.ForeColor = Color.Yellow;
                lblScore.Text = $"Excellent Fit!\nScore: {scorePercent:F1}/100\nData is modeled perfectly.";
            }
            else if (scorePercent >= 75)
            {
                lblScore.ForeColor = Color.LightGreen;
                lblScore.Text = $"Good Fit!\nScore: {scorePercent:F1}/100\nClose, but could be better.";
            }
            else
            {
                lblScore.ForeColor = Color.LightCoral;
                lblScore.Text = $"Poor Fit.\nScore: {scorePercent:F1}/100\nTry adjusting Amplitude or Period.";
            }
        }

        private void PnlGraph_Paint(object sender, PaintEventArgs e)
        {
            if (trueMaxVel == 0) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            float w = pnlGraph.Width;
            float h = pnlGraph.Height;

            float marginLeft = 50;
            float marginBottom = 30;
            float marginTop = 20;
            float marginRight = 20;

            float drawW = w - marginLeft - marginRight;
            float drawH = h - marginTop - marginBottom;

            // Draw Axes
            Pen axisPen = new Pen(Color.White, 2);
            g.DrawLine(axisPen, marginLeft, marginTop, marginLeft, h - marginBottom); 
            g.DrawLine(axisPen, marginLeft, h - marginBottom, w - marginRight, h - marginBottom); 

            float displayMaxTimeDays = (float)windowSizeDays;
            double maxVelRange = trueMaxVel * 1.5;

            float MapX(double tDays) 
            {
                return marginLeft + (float)((tDays / displayMaxTimeDays) * drawW);
            }

            float MapY(double v)
            {
                float centerY = marginTop + drawH / 2;
                return centerY - (float)(v / maxVelRange * (drawH / 2));
            }

            // Zero Line
            float y0 = MapY(0);
            using (Pen zeroPen = new Pen(Color.Gray, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
            {
                g.DrawLine(zeroPen, marginLeft, y0, w - marginRight, y0);
            }

            // Draw Noisy Data Points
            foreach (var pt in noisyDataPoints)
            {
                float x = MapX(pt.X);
                float y = MapY(pt.Y);
                g.FillEllipse(Brushes.White, x - 3, y - 3, 6, 6);
            }

            // Draw User Fit Line
            double userAmp = trueMaxVel * (trkAmplitude.Value / 100.0);
            double userPeriodDays = trkPeriod.Value;
            double userPhaseRad = trkPhase.Value * Math.PI / 180.0;

            int lineSegments = 200;
            if (lineSegments > 0)
            {
                PointF[] fitPoints = new PointF[lineSegments + 1];
                for (int i = 0; i <= lineSegments; i++)
                {
                    double tDays = displayMaxTimeDays * ((double)i / lineSegments);
                    double timeSec = tDays * 86400.0;
                    double userPeriodSec = userPeriodDays * 86400.0;

                    double v = userAmp * Math.Sin(2 * Math.PI * timeSec / userPeriodSec + userPhaseRad);
                    
                    fitPoints[i] = new PointF(MapX(tDays), MapY(v));
                }

                using (Pen fitPen = new Pen(Color.Magenta, 2))
                {
                    g.DrawLines(fitPen, fitPoints);
                }
            }

            Font font = new Font("Arial", 8); 
            Brush brush = Brushes.White;
            g.DrawString($"RV Max: {maxVelRange:F2} m/s", font, brush, 5, marginTop);
            g.DrawString($"RV Min: {-maxVelRange:F2} m/s", font, brush, 5, h - marginBottom - 15);
            g.DrawString($"Window: {displayMaxTimeDays:F1} days", font, brush, w - 150, h - 20);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FitLineGameForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "FitLineGameForm";
            this.Load += new System.EventHandler(this.FitLineGameForm_Load);
            this.ResumeLayout(false);

        }

        private void FitLineGameForm_Load(object sender, EventArgs e)
        {

        }
    }
}
