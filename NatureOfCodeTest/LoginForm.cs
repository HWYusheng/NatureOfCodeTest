using System;
using System.Data.OleDb;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace NatureOfCodeTest
{
    public class LoginForm : Form
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnRegister;
        private Button btnGuest;
        private Label lblTitle;
        private Label lblUsername;
        private Label lblPassword;
        private Label lblStatus;

        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source = " + Environment.CurrentDirectory + @"\StellerWobble.accdb";

        public LoginForm()
        {
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "FitLine Game - Login";
            this.Size = new Size(400, 360);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(25, 25, 40);
            this.ForeColor = Color.White;

            lblTitle = new Label
            {
                Text = "FitLine Game Login",
                Location = new Point(0, 20),
                Size = new Size(380, 30),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.LightSkyBlue
            };
            this.Controls.Add(lblTitle);

            lblUsername = new Label { Text = "Username:", Location = new Point(60, 75), Size = new Size(80, 20) };
            this.Controls.Add(lblUsername);

            txtUsername = new TextBox
            {
                Location = new Point(150, 72),
                Size = new Size(185, 24),
                BackColor = Color.FromArgb(40, 40, 60),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(txtUsername);

            lblPassword = new Label { Text = "Password:", Location = new Point(60, 115), Size = new Size(80, 20) };
            this.Controls.Add(lblPassword);

            txtPassword = new TextBox
            {
                Location = new Point(150, 112),
                Size = new Size(185, 24),
                PasswordChar = '●',
                BackColor = Color.FromArgb(40, 40, 60),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(txtPassword);

            lblStatus = new Label
            {
                Location = new Point(60, 148),
                Size = new Size(280, 20),
                ForeColor = Color.OrangeRed,
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblStatus);

            btnLogin = new Button
            {
                Text = "Login",
                Location = new Point(60, 180),
                Size = new Size(120, 36),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;
            this.Controls.Add(btnLogin);

            btnRegister = new Button
            {
                Text = "Register",
                Location = new Point(215, 180),
                Size = new Size(120, 36),
                BackColor = Color.FromArgb(0, 160, 100),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Click += BtnRegister_Click;
            this.Controls.Add(btnRegister);

            btnGuest = new Button
            {
                Text = "Play as Guest",
                Location = new Point(100, 240),
                Size = new Size(190, 36),
                BackColor = Color.FromArgb(70, 70, 90),
                ForeColor = Color.LightGray,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnGuest.FlatAppearance.BorderSize = 0;
            btnGuest.Click += BtnGuest_Click;
            this.Controls.Add(btnGuest);

            Label lblGuestNote = new Label
            {
                Text = "Guest attempts are cleared on next startup.",
                Location = new Point(50, 285),
                Size = new Size(300, 20),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 8)
            };
            this.Controls.Add(lblGuestNote);
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblStatus.Text = "Please enter username and password.";
                return;
            }

            string hash = HashPassword(password);
            string sql = "SELECT UserID, Username FROM Users WHERE Username = ? AND PasswordHash = ?";

            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("?", username);
                    cmd.Parameters.AddWithValue("?", hash);
                    conn.Open();
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            UserSession.CurrentUserID = reader.GetInt32(0);
                            UserSession.CurrentUsername = reader.GetString(1);
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            lblStatus.Text = "Invalid username or password.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Login error: " + ex.Message;
            }
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblStatus.Text = "Please enter username and password.";
                return;
            }

            if (password.Length < 4)
            {
                lblStatus.Text = "Password must be at least 4 characters.";
                return;
            }

            string hash = HashPassword(password);
            string sql = "INSERT INTO Users (Username, PasswordHash) VALUES (?, ?)";

            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("?", username);
                    cmd.Parameters.AddWithValue("?", hash);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                // Auto-login after registration
                string selectSql = "SELECT UserID FROM Users WHERE Username = ?";
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                using (OleDbCommand cmd = new OleDbCommand(selectSql, conn))
                {
                    cmd.Parameters.AddWithValue("?", username);
                    conn.Open();
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            UserSession.CurrentUserID = reader.GetInt32(0);
                            UserSession.CurrentUsername = username;
                        }
                    }
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (OleDbException ex)
            {
                if (ex.Message.Contains("duplicate") || ex.Message.Contains("unique") || ex.Errors.Count > 0)
                    lblStatus.Text = "Username already exists.";
                else
                    lblStatus.Text = "Register error: " + ex.Message;
            }
        }

        private void BtnGuest_Click(object sender, EventArgs e)
        {
            UserSession.CurrentUserID = -1;
            UserSession.CurrentUsername = "Guest";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
