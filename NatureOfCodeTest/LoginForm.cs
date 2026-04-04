using System;
using System.Drawing;
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

        private UserRepositary userRepo = new UserRepositary();

        public LoginForm()
        {
            InitializeComponentsRunTime();
        }

        private void InitializeComponentsRunTime()
        {
            this.Text = "FitLine Game - Login";
            this.Size = new Size(400, 360);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(25, 25, 40);
            this.ForeColor = Color.White;
            // Lables and Textboxes
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

            // Buttons
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




        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblStatus.Text = "Please enter username and password.";
                return;
            }

            var result = userRepo.Login(username, password);
            if (result.success)
            {
                UserSession.CurrentUserID   = result.userID;
                UserSession.CurrentUsername = result.username;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                lblStatus.Text = result.errorMessage;
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

            var result = userRepo.Register(username, password);
            if (result.success)
            {
                UserSession.CurrentUserID   = result.userID;
                UserSession.CurrentUsername = username;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                lblStatus.Text = result.errorMessage;
            }
        }

        private void BtnGuest_Click(object sender, EventArgs e)
        {
            UserSession.CurrentUserID = -1;
            UserSession.CurrentUsername = "Guest";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        private void LoginForm_Load(object sender, EventArgs e)
        {

        }


        private void LoginForm_Load_1(object sender, EventArgs e)
        {

        }


        private void LoginForm_Load_2(object sender, EventArgs e)
        {

        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // LoginForm
            // 
            this.ClientSize = new System.Drawing.Size(274, 229);
            this.Name = "LoginForm";
            this.Load += new System.EventHandler(this.LoginForm_Load_3);
            this.ResumeLayout(false);

        }

        private void LoginForm_Load_3(object sender, EventArgs e)
        {

        }
    }
}
