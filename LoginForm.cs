using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace LeltarRendszer
{
    public class LoginForm : Form
    {
        TextBox usernameTextBox = new TextBox { PlaceholderText = "Felhasználónév" };
        TextBox passwordTextBox = new TextBox { PlaceholderText = "Jelszó", UseSystemPasswordChar = true };
        Button loginButton = new Button { Text = "Bejelentkezés" };
        Label titleLabel = new Label { Text = "LELTÁR RENDSZER", AutoSize = false, TextAlign = ContentAlignment.MiddleCenter };

        public LoginForm()
        {
            Text = "Bejelentkezés - Leltár Rendszer";
            Width = 400;
            Height = 350;
            BackColor = Color.FromArgb(17, 17, 17);
            ForeColor = Color.Aqua;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            SetupControls();
            ApplyModernStyling();
        }

        private void SetupControls()
        {
            int controlWidth = 250;
            int centerX = ClientSize.Width / 2;

            titleLabel.Width = controlWidth;
            titleLabel.Height = 40;
            titleLabel.Location = new Point(centerX - titleLabel.Width / 2, 40);

            usernameTextBox.Width = controlWidth;
            passwordTextBox.Width = controlWidth;
            loginButton.Width = controlWidth;

            usernameTextBox.Location = new Point(centerX - usernameTextBox.Width / 2, 120);
            passwordTextBox.Location = new Point(centerX - passwordTextBox.Width / 2, 170);
            loginButton.Location = new Point(centerX - loginButton.Width / 2, 220);

            Controls.AddRange(new Control[] { titleLabel, usernameTextBox, passwordTextBox, loginButton });

            loginButton.Click += loginButton_Click;
            
            this.AcceptButton = loginButton;
        }

        private void ApplyModernStyling()
        {
            Color darkGray = Color.FromArgb(30, 30, 30);
            Color aqua = Color.Aqua;
            Color lightGray = Color.FromArgb(70, 70, 70);

            titleLabel.ForeColor = aqua;
            titleLabel.Font = new Font("Segoe UI", 16F, FontStyle.Bold);

            usernameTextBox.BackColor = darkGray;
            usernameTextBox.ForeColor = Color.White;
            usernameTextBox.BorderStyle = BorderStyle.FixedSingle;
            usernameTextBox.Height = 30;

            passwordTextBox.BackColor = darkGray;
            passwordTextBox.ForeColor = Color.White;
            passwordTextBox.BorderStyle = BorderStyle.FixedSingle;
            passwordTextBox.Font = new Font("Segoe UI", 11F);
            passwordTextBox.Height = 30;

            loginButton.BackColor = lightGray;
            loginButton.ForeColor = aqua;
            loginButton.FlatStyle = FlatStyle.Flat;
            loginButton.FlatAppearance.BorderColor = aqua;
            loginButton.FlatAppearance.BorderSize = 2;
            loginButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            loginButton.Height = 40;
            loginButton.Cursor = Cursors.Hand;

            loginButton.MouseEnter += (s, e) => {
                loginButton.BackColor = Color.FromArgb(90, 90, 90);
            };
            loginButton.MouseLeave += (s, e) => {
                loginButton.BackColor = lightGray;
            };
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            string enteredUsername = usernameTextBox.Text.Trim();
            string enteredPassword = passwordTextBox.Text;

            if (string.IsNullOrWhiteSpace(enteredUsername) || string.IsNullOrWhiteSpace(enteredPassword))
            {
                MessageBox.Show("Kérlek töltsd ki az összes mezőt!", "Hiányzó adatok", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!File.Exists("users.txt")) 
            {
                File.WriteAllText("users.txt", "admin;admin123;admin\n");
            }

            var lines = File.ReadAllLines("users.txt");
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                
                var parts = line.Split(';');
                if (parts.Length >= 3 && parts[0] == enteredUsername && parts[1] == enteredPassword)
                {
                    File.AppendAllText("log.txt", $"{DateTime.Now}: {enteredUsername} bejelentkezett\n");
                    
                    Hide();
                    new MainForm(parts[0], parts[2]).Show();
                    return;
                }
            }
            
            MessageBox.Show("Hibás felhasználónév vagy jelszó!", "Bejelentkezési hiba", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            

            passwordTextBox.Clear();
        }
    }
}