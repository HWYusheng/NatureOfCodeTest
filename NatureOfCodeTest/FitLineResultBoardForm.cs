using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace NatureOfCodeTest
{
    public class FitLineResultBoardForm : Form
    {
        private DataGridView gridResults;
        private FitLineResultRepositary repo;

        public FitLineResultBoardForm()
        {
            repo = new FitLineResultRepositary();
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Text = "Fit Line Result Board";
            this.Size = new Size(600, 450);
            this.StartPosition = FormStartPosition.CenterParent;

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
        }

        private void LoadData()
        {
            var results = repo.GetAllResults();
            
            // Format for display
            var displayData = results.Select(r => new {
                AttemptID = r.SimulationID,
                Date = r.DatePlayed.ToString("yyyy-MM-dd HH:mm:ss"),
                Score = r.FitScore.ToString("F1") + " / 100",
                Time_Taken = r.TimeTakenSec + "s"
            }).ToList();

            gridResults.DataSource = displayData;
        }
    }
}
