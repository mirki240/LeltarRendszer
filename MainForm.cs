using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace LeltarRendszer
{
    public class MainForm : Form
    {
        string username;
        string role;
        
        TextBox inventoryTextBox = new TextBox { PlaceholderText = "Új tétel" };
        Button addInventoryButton = new Button { Text = "Leltárhoz ad" };
        ListBox inventoryListBox = new ListBox();
        Button refreshButton = new Button { Text = "Frissítés" };
        Button deleteButton = new Button { Text = "Kijelölt törlése" };
        
        TextBox newUserTextBox = new TextBox { PlaceholderText = "Új felhasználó" };
        TextBox newPassTextBox = new TextBox { PlaceholderText = "Jelszó" };
        ComboBox roleComboBox = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        Button addUserButton = new Button { Text = "Felhasználó hozzáadása" };

        Label devLabel = new Label { Text = "Developer: @Mirki_240", AutoSize = true };
        Label userRoleLabel = new Label { AutoSize = true };

        public MainForm(string username, string role)
        {
            this.username = username;
            this.role = role;
            Text = $"Leltár ({username})";
            Width = 650;
            Height = 850;
            BackColor = Color.FromArgb(17, 17, 17);
            ForeColor = Color.Aqua;
            StartPosition = FormStartPosition.CenterScreen;
            
            this.Resize += MainForm_Resize;
            
            SetupControls();
            ApplyModernStyling();
            LoadInventory();
            SetupUserRoleDisplay();
        }

        private void SetupControls()
        {
            int controlWidth = 300;
            inventoryTextBox.Width = controlWidth;
            addInventoryButton.Width = controlWidth;
            addInventoryButton.Height = 45;
            inventoryListBox.Width = controlWidth;
            inventoryListBox.Height = 200;
            refreshButton.Width = controlWidth;
            refreshButton.Height = 45;
            deleteButton.Width = controlWidth;
            deleteButton.Height = 45;
            
            newUserTextBox.Width = controlWidth;
            newPassTextBox.Width = controlWidth;
            roleComboBox.Width = controlWidth;
            addUserButton.Width = controlWidth;
            addUserButton.Height = 45;

            PositionControls();

            Controls.AddRange(new Control[] {
                inventoryTextBox, addInventoryButton, inventoryListBox, refreshButton, deleteButton,
                newUserTextBox, newPassTextBox, roleComboBox, addUserButton,
                devLabel, userRoleLabel
            });

            roleComboBox.Items.AddRange(new string[] { "admin", "user" });
            
            addInventoryButton.Click += addInventoryButton_Click;
            addUserButton.Click += addUserButton_Click;
            refreshButton.Click += refreshButton_Click;
            deleteButton.Click += deleteButton_Click;

            if (role != "admin")
            {
                newUserTextBox.Visible = false;
                newPassTextBox.Visible = false;
                roleComboBox.Visible = false;
                addUserButton.Visible = false;
            }
        }

        private void PositionControls()
        {
            int centerX = ClientSize.Width / 2;
            int startY = 60;
            int spacing = 55;

            userRoleLabel.Location = new Point(20, 20);

            inventoryTextBox.Location = new Point(centerX - inventoryTextBox.Width / 2, startY);
            addInventoryButton.Location = new Point(centerX - addInventoryButton.Width / 2, startY + spacing);
            inventoryListBox.Location = new Point(centerX - inventoryListBox.Width / 2, startY + spacing * 2);
            refreshButton.Location = new Point(centerX - refreshButton.Width / 2, startY + spacing * 2 + inventoryListBox.Height + 15);
            deleteButton.Location = new Point(centerX - deleteButton.Width / 2, startY + spacing * 2 + inventoryListBox.Height + 70);

            if (role == "admin")
            {
                int adminStartY = startY + spacing * 2 + inventoryListBox.Height + 180;
                newUserTextBox.Location = new Point(centerX - newUserTextBox.Width / 2, adminStartY);
                newPassTextBox.Location = new Point(centerX - newPassTextBox.Width / 2, adminStartY + spacing);
                roleComboBox.Location = new Point(centerX - roleComboBox.Width / 2, adminStartY + spacing * 2);
                addUserButton.Location = new Point(centerX - addUserButton.Width / 2, adminStartY + spacing * 3);
            }

            devLabel.Location = new Point(ClientSize.Width - 150, ClientSize.Height - 30);
            devLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        }

        private void ApplyModernStyling()
        {
            Color darkGray = Color.FromArgb(30, 30, 30);
            Color aqua = Color.Aqua;
            Color lightGray = Color.FromArgb(70, 70, 70);
            Color redColor = Color.Red;
            Color redDark = Color.FromArgb(80, 20, 20);

            foreach (Control control in Controls)
            {
                if (control is TextBox textBox)
                {
                    textBox.BackColor = darkGray;
                    textBox.ForeColor = Color.White;
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    textBox.Font = new Font("Segoe UI", 10F);
                    textBox.Height = 30;

                    if (textBox == newUserTextBox || textBox == newPassTextBox)
                    {
                        textBox.BackColor = Color.FromArgb(40, 20, 20);
                        textBox.ForeColor = Color.LightPink;
                    }
                }
                else if (control is Button button)
                {
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderSize = 2;
                    button.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
                    button.Cursor = Cursors.Hand;
                    button.TextAlign = ContentAlignment.MiddleCenter;

                    if (button == addUserButton)
                    {
                        button.BackColor = redDark;
                        button.ForeColor = redColor;
                        button.FlatAppearance.BorderColor = redColor;
                    }
                    else if (button == deleteButton)
                    {
                        button.BackColor = Color.FromArgb(60, 20, 20);
                        button.ForeColor = Color.LightCoral;
                        button.FlatAppearance.BorderColor = Color.LightCoral;
                    }
                    else
                    {
                        button.BackColor = lightGray;
                        button.ForeColor = aqua;
                        button.FlatAppearance.BorderColor = aqua;
                    }
                }
                else if (control is ComboBox comboBox)
                {
                    comboBox.BackColor = Color.FromArgb(40, 20, 20);
                    comboBox.ForeColor = Color.LightPink;
                    comboBox.FlatStyle = FlatStyle.Flat;
                    comboBox.Font = new Font("Segoe UI", 10F);
                    comboBox.Height = 30;
                }
                else if (control is ListBox listBox)
                {
                    listBox.BackColor = darkGray;
                    listBox.ForeColor = Color.White;
                    listBox.BorderStyle = BorderStyle.FixedSingle;
                    listBox.Font = new Font("Segoe UI", 10F);
                    listBox.SelectionMode = SelectionMode.One;
                }
            }

            devLabel.ForeColor = aqua;
            devLabel.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            PositionControls();
        }

        private void SetupUserRoleDisplay()
        {
            if (role == "admin")
            {
                userRoleLabel.Text = "RANG: ADMIN";
                userRoleLabel.ForeColor = Color.Red;
                userRoleLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            }
            else
            {
                userRoleLabel.Text = "RANG: USER";
                userRoleLabel.ForeColor = Color.Aqua;
                userRoleLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            }
        }

        private void LoadInventory()
        {
            inventoryListBox.Items.Clear();
            if (File.Exists("inventory.txt"))
            {
                string[] items = File.ReadAllLines("inventory.txt");
                foreach (string item in items)
                {
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        inventoryListBox.Items.Add(item.Trim());
                    }
                }
            }
        }

        private void addInventoryButton_Click(object sender, EventArgs e)
        {
            string item = inventoryTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(item))
            {
                MessageBox.Show("Nem adhatsz hozzá üres tételt!", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            File.AppendAllText("inventory.txt", item + Environment.NewLine);
            File.AppendAllText("log.txt", $"{DateTime.Now}: {username} hozzáadta: {item}\n");
            inventoryTextBox.Clear();
            LoadInventory();
            MessageBox.Show("Tétel hozzáadva.", "Siker", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (inventoryListBox.SelectedItem == null)
            {
                MessageBox.Show("Válassz ki egy tételt a törléshez!", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedItem = inventoryListBox.SelectedItem.ToString();
            DialogResult result = MessageBox.Show($"Biztosan törölni szeretnéd: {selectedItem}?", "Törlés megerősítése", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                var lines = File.ReadAllLines("inventory.txt").Where(line => line.Trim() != selectedItem).ToArray();
                File.WriteAllLines("inventory.txt", lines);
                File.AppendAllText("log.txt", $"{DateTime.Now}: {username} törölte: {selectedItem}\n");
                LoadInventory();
                MessageBox.Show("Tétel törölve.", "Siker", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            LoadInventory();
        }

        private void addUserButton_Click(object sender, EventArgs e)
        {
            string user = newUserTextBox.Text.Trim();
            string pass = newPassTextBox.Text.Trim();
            string newRole = roleComboBox.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass) || newRole == null)
            {
                MessageBox.Show("Hiányzó adatok!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            File.AppendAllText("users.txt", $"{user};{pass};{newRole}\n");
            File.AppendAllText("log.txt", $"{DateTime.Now}: {username} létrehozta: {user} ({newRole})\n");
            newUserTextBox.Clear();
            newPassTextBox.Clear();
            roleComboBox.SelectedIndex = -1;
            MessageBox.Show("Felhasználó hozzáadva.", "Siker", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}