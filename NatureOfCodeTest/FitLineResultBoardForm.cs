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
        private Label lblPlayerInfo;
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
            this.Size = new Size(750, 520);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(25, 25, 40);
            this.ForeColor = Color.White;

            // Top banner — shows who is logged in
            lblPlayerInfo = new Label
            {
                Dock = DockStyle.Top,
                Height = 36,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 11, FontStyle.Bold),
                BackColor = UserSession.IsLoggedIn
                    ? Color.FromArgb(0, 100, 160)
                    : Color.FromArgb(70, 70, 90),
                ForeColor = Color.White,
                Text = UserSession.IsLoggedIn
                    ? $"Showing all attempts linked to account:  {UserSession.CurrentUsername}  (UserID #{UserSession.CurrentUserID})"
                    : "Guest Mode — showing this session's attempts only (not saved between sessions)"
            };

            // Average stats bar
            lblAverages = new Label
            {
                Dock = DockStyle.Top,
                Height = 30,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Arial", 9, FontStyle.Bold),
                Padding = new Padding(10, 0, 0, 0),
                BackColor = Color.FromArgb(35, 35, 55),
                ForeColor = Color.LightSkyBlue
            };

            // Results grid
            gridResults = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.FromArgb(30, 30, 50),
                GridColor = Color.FromArgb(60, 60, 90),
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                EnableHeadersVisualStyles = false
            };
            gridResults.DefaultCellStyle.BackColor          = Color.FromArgb(30, 30, 50);
            gridResults.DefaultCellStyle.ForeColor          = Color.White;
            gridResults.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 100, 180);
            gridResults.DefaultCellStyle.SelectionForeColor = Color.White;
            gridResults.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 20, 35);
            gridResults.ColumnHeadersDefaultCellStyle.ForeColor = Color.LightSkyBlue;
            gridResults.ColumnHeadersDefaultCellStyle.Font  = new Font("Arial", 9, FontStyle.Bold);
            gridResults.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(38, 38, 60);

            // Stack: banner at top, then stats bar, then grid
            this.Controls.Add(gridResults);
            this.Controls.Add(lblAverages);
            this.Controls.Add(lblPlayerInfo);
        }

        private void LoadData()
        {
            var results = repo.GetAllResults();

            // Display: Username from joined Users table, not raw PlayerID number
            var displayData = results.Select(r => new
            {
                Attempt_ID  = r.SimulationID,
                Player      = r.Username,                         // joined from Users
                Star_System = r.HostStarName,
                Planet      = r.PlanetName,
                Score       = r.FitScore.ToString("F1") + " / 100",
                Time_Taken  = r.TimeTakenSec + "s"
            }).ToList();

            gridResults.DataSource = displayData;

            var avgs = repo.GetAverages();
            lblAverages.Text =
                $"  Avg Score: {avgs.AvgScore:F1} / 100   |   " +
                $"Avg Time: {avgs.AvgTime:F1}s   |   " +
                $"Total Attempts: {results.Count}";
        }
    }
}
