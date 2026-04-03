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
        private Button btnRetry;
        private Button btnSubmit;
        private Button btnResultBoard;
        private Button btnLogin;
        private Label lblTimer;
        private Label lblUserStatus;
        private System.Windows.Forms.Timer gameTimer;
        private int timeTakenSeconds;
        private FitLineResultRepositary repo;
        private CelesBodyRepositary systemRepo;
        private int currentPlanetID = -1;

        private SimulationEngine engine;
        private List<PointF> noisyDataPoints = new List<PointF>();
        
        private double trueMaxVel = 0;
        private double windowSizeDays = 1000;
        
        private Random random = new Random();
        
        public FitLineGameForm()
        {
            EnableDoubleBuffering();
            InitializeComponentsRunTime();
            StartNewGame();
        }

        private void InitializeComponentsRunTime()
        {
            this.Text = "Fit Line Game";
            this.Size = new Size(1000, 600);
            this.BackColor = Color.Black;
            this.ForeColor = Color.White;

            repo = new FitLineResultRepositary();
            systemRepo = new CelesBodyRepositary();
            
            gameTimer = new System.Windows.Forms.Timer();
            gameTimer.Interval = 1000;
            gameTimer.Tick += (s, e) => { timeTakenSeconds++; lblTimer.Text = $"Time: {timeTakenSeconds}s"; };

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
            lblAmp = new Label { Location = new Point(10, currentY), Size = new Size(150, 20), Text = "Amplitude: 0%", Anchor = AnchorStyles.Bottom | AnchorStyles.Left };
            this.Controls.Add(lblAmp);
            
            trkAmplitude = new TrackBar { Location = new Point(160, currentY), Size = new Size(400, 45), Minimum = 10, Maximum = 200, Value = 100, TickFrequency = 10, Anchor = AnchorStyles.Bottom | AnchorStyles.Left };
            trkAmplitude.Scroll += (s, e) => { pnlGraph.Invalidate(); lblAmp.Text = $"Amplitude: {trkAmplitude.Value}%"; };
            this.Controls.Add(trkAmplitude);

            currentY += 45;

            // Period
            lblPeriod = new Label { Location = new Point(10, currentY), Size = new Size(150, 20), Text = "Period: 100 Days", Anchor = AnchorStyles.Bottom | AnchorStyles.Left };
            this.Controls.Add(lblPeriod);
            
            trkPeriod = new TrackBar { Location = new Point(160, currentY), Size = new Size(400, 45), Minimum = 50, Maximum = 1500, Value = 365, TickFrequency = 50, Anchor = AnchorStyles.Bottom | AnchorStyles.Left };
            trkPeriod.Scroll += (s, e) => { pnlGraph.Invalidate(); lblPeriod.Text = $"Period: {trkPeriod.Value} Days"; };
            this.Controls.Add(trkPeriod);

            currentY += 45;

            // Phase
            lblPhase = new Label { Location = new Point(10, currentY), Size = new Size(150, 20), Text = "Phase: 0°", Anchor = AnchorStyles.Bottom | AnchorStyles.Left };
            this.Controls.Add(lblPhase);
            
            trkPhase = new TrackBar { Location = new Point(160, currentY), Size = new Size(400, 45), Minimum = 0, Maximum = 360, Value = 180, TickFrequency = 15, Anchor = AnchorStyles.Bottom | AnchorStyles.Left };
            trkPhase.Scroll += (s, e) => { pnlGraph.Invalidate(); lblPhase.Text = $"Phase: {trkPhase.Value}°"; };
            this.Controls.Add(trkPhase);

            // Buttons
            btnNewGame = new Button { Location = new Point(520, 420), Size = new Size(140, 40), Text = "New System", ForeColor = Color.YellowGreen, Font = new Font("Arial", 10, FontStyle.Bold), Anchor = AnchorStyles.Bottom | AnchorStyles.Right };
            btnNewGame.Click += (s, e) => StartNewGame();
            this.Controls.Add(btnNewGame);

            btnRetry = new Button { Location = new Point(670, 420), Size = new Size(140, 40), Text = "Retry System", ForeColor = Color.Orange, Font = new Font("Arial", 10, FontStyle.Bold), Anchor = AnchorStyles.Bottom | AnchorStyles.Right };
            btnRetry.Click += (s, e) => RetryCurrentGame();
            this.Controls.Add(btnRetry);

            btnSubmit = new Button { Location = new Point(820, 420), Size = new Size(140, 40), Text = "Submit Fit", ForeColor = Color.Black, Font = new Font("Arial", 10, FontStyle.Bold), BackColor = Color.LightGreen, Anchor = AnchorStyles.Bottom | AnchorStyles.Right };
            btnSubmit.Click += BtnSubmit_Click;
            this.Controls.Add(btnSubmit);
            
            btnResultBoard = new Button { Location = new Point(800, 480), Size = new Size(130, 40), Text = "Result Board", ForeColor = Color.White, BackColor = Color.DimGray, Font = new Font("Arial", 10, FontStyle.Bold), Anchor = AnchorStyles.Bottom | AnchorStyles.Right };
            btnResultBoard.Click += (s, e) => { new FitLineResultBoardForm().ShowDialog(); };
            this.Controls.Add(btnResultBoard);

            // Optional Login / Logout button
            btnLogin = new Button
            {
                Location = new Point(600, 480),
                Size = new Size(150, 36),
                Text = "Login / Register",
                ForeColor = Color.White,
                BackColor = Color.FromArgb(0, 100, 160),
                Font = new Font("Arial", 9, FontStyle.Bold),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };
            btnLogin.Click += BtnLoginToggle_Click;
            this.Controls.Add(btnLogin);

            lblUserStatus = new Label
            {
                Location = new Point(170, 487),
                Size = new Size(240, 22),
                Text = "Playing as: Guest",
                ForeColor = Color.Gray,
                Font = new Font("Arial", 9, FontStyle.Italic),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };
            this.Controls.Add(lblUserStatus);

            lblScore = new Label { Location = new Point(520, 490), Size = new Size(270, 80), Text = "Fit the line to the data points!", Font = new Font("Arial", 12, FontStyle.Bold), ForeColor = Color.LightSkyBlue, Anchor = AnchorStyles.Bottom | AnchorStyles.Right };
            this.Controls.Add(lblScore);

            lblTimer = new Label { Location = new Point(520, 470), Size = new Size(150, 30), Text = "Time: 0s", ForeColor = Color.White, Font = new Font("Arial", 10, FontStyle.Bold), Anchor = AnchorStyles.Bottom | AnchorStyles.Right };
            this.Controls.Add(lblTimer);
        }

        private void StartNewGame()
        {
            btnSubmit.Enabled = true;
            lblScore.Text = "Fit the line to the data points!\nAdjust sliders and click Submit.";
            lblScore.ForeColor = Color.LightSkyBlue;

            timeTakenSeconds = 0;
            lblTimer.Text = "Time: 0s";
            gameTimer.Start();

            var systemData = systemRepo.GetRandomSystem();
            if (systemData == null)
            {
                currentPlanetID = -1;
                double tempStarMass = (0.5 + random.NextDouble() * 1.5) * NatureOfCodeTest.Model.PhysicalConstants.SolarMass;
                double tempPlanetMass = (0.5 + random.NextDouble() * 19.5) * 1.898e27;
                double tempSemiMajor = (0.1 + random.NextDouble() * 1.4) * NatureOfCodeTest.Model.PhysicalConstants.AU;
                
                systemData = new SystemFetchData
                {
                    PlanetID = -1,
                    HostStar = new Star { Name = "Fallback Star", Mass = tempStarMass, Position = Vector2.Zero },
                    OrbitingPlanet = new Planet 
                    { 
                        Name = "Fallback Planet", Mass = tempPlanetMass, 
                        Orbit = new OrbitalElements { SemiMajorAxis = tempSemiMajor, Eccentricity = 0, Inclination = 0, ArgumentOfPeriapsis = 0 }
                    }
                };
            }
            
            currentPlanetID = systemData.PlanetID;
            Star sun = systemData.HostStar;
            Planet planet = systemData.OrbitingPlanet;
            
            double earthMass = 5.972e24;
            double solarMass = NatureOfCodeTest.Model.PhysicalConstants.SolarMass; 
            
            double starMass = (sun.Mass <= 0 ? 1 : sun.Mass) * solarMass;
            sun.Mass = starMass;

            double planetMass = (planet.Mass <= 0 ? 1 : planet.Mass) * earthMass;
            planet.Mass = planetMass;

            double semiMajorAU = planet.Orbit.SemiMajorAxis;
            if (semiMajorAU <= 0) semiMajorAU = 0.1 + random.NextDouble() * 1.4;
            double semiMajor = semiMajorAU * NatureOfCodeTest.Model.PhysicalConstants.AU;
            planet.Orbit.SemiMajorAxis = semiMajor;
            planet.Orbit.Eccentricity = 0.0; // Keeping it circular for simple sine fitting

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

        private void RetryCurrentGame()
        {
            if (noisyDataPoints.Count == 0) return;

            btnSubmit.Enabled = true;
            lblScore.Text = "Fit the line to the data points!\nAdjust sliders and click Submit.";
            lblScore.ForeColor = Color.LightSkyBlue;

            timeTakenSeconds = 0;
            lblTimer.Text = "Time: 0s";
            gameTimer.Start();

            // Setup new random sliders values to let them try again
            trkAmplitude.Value = random.Next(10, 200);
            
            double periodDays = windowSizeDays / 2.5;
            int pMin = Math.Max(10, (int)(periodDays * 0.5));
            int pMax = (int)(periodDays * 2.5);
            trkPeriod.Value = random.Next(pMin, pMax);

            trkPhase.Value = random.Next(0, 360);
            
            lblAmp.Text = $"Amplitude: {trkAmplitude.Value}%";
            lblPeriod.Text = $"Period: {trkPeriod.Value} Days";
            lblPhase.Text = $"Phase: {trkPhase.Value}°";

            pnlGraph.Invalidate();
        }

        private void BtnLoginToggle_Click(object sender, EventArgs e)
        {
            if (UserSession.IsLoggedIn)
            {
                // Logout
                UserSession.CurrentUserID = -1;
                UserSession.CurrentUsername = "Guest";
                btnLogin.Text = "Login / Register";
                btnLogin.BackColor = Color.FromArgb(0, 100, 160);
                lblUserStatus.Text = "Playing as: Guest";
                lblUserStatus.ForeColor = Color.Gray;
            }
            else
            {
                using (var loginForm = new LoginForm())
                {
                    loginForm.ShowDialog(this);
                }
                UpdateLoginUI();
            }
        }

        private void UpdateLoginUI()
        {
            if (UserSession.IsLoggedIn)
            {
                btnLogin.Text = "Logout";
                btnLogin.BackColor = Color.FromArgb(160, 60, 60);
                lblUserStatus.Text = $"Playing as: {UserSession.CurrentUsername}";
                lblUserStatus.ForeColor = Color.LightGreen;
            }
            else
            {
                btnLogin.Text = "Login / Register";
                btnLogin.BackColor = Color.FromArgb(0, 100, 160);
                lblUserStatus.Text = "Playing as: Guest";
                lblUserStatus.ForeColor = Color.Gray;
            }
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            if (noisyDataPoints.Count == 0 || trueMaxVel == 0) return;

            btnSubmit.Enabled = false;
            gameTimer.Stop();

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

                // Engine initial config uses vx = -sin(E), so the base wave is a negative sine.
                double userRV = userAmp * -Math.Sin(2 * Math.PI * timeSec / userPeriodSec + userPhaseRad);
                
                double rvDiff = Math.Abs(userRV - pt.Y);
                totalError += rvDiff;
            }

            double meanError = totalError / noisyDataPoints.Count;
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

            repo.AddResult(currentPlanetID, scorePercent, timeTakenSeconds);
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

        private void EnableDoubleBuffering()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
        }
        private void FitLineGameForm_Load(object sender, EventArgs e)
        {

        }


    }
}
