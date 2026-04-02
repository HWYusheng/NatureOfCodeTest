using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace NatureOfCodeTest
{
    public class FitLineResultBoardForm : Form
    {
        private DataGridView gridResults;
        private Label lblAverages;
        private FitLineResultRepositary repo;

        public FitLineResultBoardForm()
        {
            repo = new FitLineResultRepositary();
            SetupUI();
            LoadData();
        }

        private void SetupUI()
        {
            this.Text = "Fit Line Result Board";
            this.Size = new Size(600, 450);
            this.StartPosition = FormStartPosition.CenterParent;

            lblAverages = new Label
            {
                Dock = DockStyle.Top,
                Height = 30,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font(this.Font, FontStyle.Bold),
                Padding = new Padding(10, 0, 0, 0)
            };

            gridResults = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White
            };

            this.Controls.Add(gridResults);
            this.Controls.Add(lblAverages);
        }

        private void LoadData()
        {
            var results = repo.GetAllResults();
            
            // Format for display
            var displayData = results.Select(r => new {
                AttemptID = r.SimulationID,
                PlanetName = r.PlanetName,
                Date = r.DatePlayed.ToString("yyyy-MM-dd HH:mm:ss"),
                Score = r.FitScore.ToString("F1") + " / 100",
                Time_Taken = r.TimeTakenSec + "s"
            }).ToList();

            gridResults.DataSource = displayData;
            
            var avgs = repo.GetAverages();
            lblAverages.Text = $"Average Score: {avgs.AvgScore:F1} | Average Time: {avgs.AvgTime:F1}s";
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FitLineResultBoardForm
            // 
            this.ClientSize = new System.Drawing.Size(274, 229);
            this.Name = "FitLineResultBoardForm";
            this.Load += new System.EventHandler(this.FitLineResultBoardForm_Load);
            this.ResumeLayout(false);

        }

        private void FitLineResultBoardForm_Load(object sender, EventArgs e)
        {

        }
    }
}
