using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text.Json;
using static System.Windows.Forms.DataFormats;

namespace AO_Login
{
    public partial class Form2 : Form
    {

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        private readonly Panel titleBar; // Add readonly modifier
        private readonly Button btnClose; // Add readonly modifier
        private readonly Button btnMinimize; // Add readonly modifier
        private readonly Button btnMaximize; // Add readonly modifier

        // Draw the title text on the custom titleBar panel with a custom font size
        public string TitleBarText { get; set; } = "Multi-Box Teams";
        public float TitleBarFontSize { get; set; } = 12f; // Change this value for different font sizes

        // Add these properties to your Form1 class
        public bool TitleBarMinimizeVisible
        {
            get => btnMinimize.Visible;
            set => btnMinimize.Visible = value;
        }

        public bool TitleBarMaximizeVisible
        {
            get => btnMaximize.Visible;
            set => btnMaximize.Visible = value;
        }

        public bool TitleBarTextVisible
        {
            get => _titleBarTextVisible;
            set
            {
                _titleBarTextVisible = value;
                titleBar.Invalidate(); // Redraw title bar
            }
        }
        private bool _titleBarTextVisible = true;

        public Form2(bool tempdt, string? TextColorName)
        {
            InitializeComponent();
            // Initialize readonly fields in the constructor
            titleBar = new Panel
            {
                Height = 32,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(30, 30, 30)
            };

            btnClose = new Button
            {
                Text = "X",
                ForeColor = Color.White,
                BackColor = Color.Red,
                FlatStyle = FlatStyle.Flat,
                Width = 40,
                Height = 32,
                Dock = DockStyle.Right
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();

            btnMaximize = new Button
            {
                Text = "🗖",
                ForeColor = Color.White,
                BackColor = Color.Gray,
                FlatStyle = FlatStyle.Flat,
                Width = 40,
                Height = 32,
                Dock = DockStyle.Right
            };
            btnMaximize.FlatAppearance.BorderSize = 0;
            btnMaximize.Click += (s, e) =>
            {
                if (this.WindowState == FormWindowState.Maximized)
                    this.WindowState = FormWindowState.Normal;
                else
                    this.WindowState = FormWindowState.Maximized;
            };

            btnMinimize = new Button
            {
                Text = "—",
                ForeColor = Color.White,
                BackColor = Color.Gray,
                FlatStyle = FlatStyle.Flat,
                Width = 40,
                Height = 32,
                Dock = DockStyle.Right
            };
            btnMinimize.FlatAppearance.BorderSize = 0;
            btnMinimize.Click += (s, e) => this.WindowState = FormWindowState.Minimized;

            // Add the title bar buttons in this order (minimize, maximize, close)
            titleBar.Controls.Add(btnMinimize);
            titleBar.Controls.Add(btnMaximize);
            titleBar.Controls.Add(btnClose);

            titleBar.MouseDown += TitleBar_MouseDown;
            titleBar.MouseMove += TitleBar_MouseMove;
            titleBar.MouseUp += TitleBar_MouseUp;

            this.Controls.Add(titleBar);
            titleBar.BringToFront();

            Color color = Color.White;
            switch (TextColorName)
            {
                case "Black":
                    color = Color.Black;
                    break;
                case "White":
                    color = Color.White;
                    break;
                case "Yellow":
                    color = Color.Gold;
                    break;
                case "Orange":
                    color = Color.Orange;
                    break;
                case "Green":
                    color = Color.LimeGreen;
                    break;
                case "Blue":
                    color = Color.DeepSkyBlue;
                    break;
                case "Pink":
                    color = Color.HotPink;
                    break;
                case "Purple":
                    color = Color.MediumPurple;
                    break;
            }

            if (!tempdt) //Dark theme
            {
                this.BackColor = Color.Black;
                this.ForeColor = color;
                btnClose.BackColor = Color.Black;
                btnClose.ForeColor = color;
                foreach (Control c in this.Controls)
                {
                    c.BackColor = Color.Black;
                    c.ForeColor = color;
                    btnClose.BackColor = Color.Black;
                    btnClose.ForeColor = color;
                }
            }
            else //Light theme
            {
                this.BackColor = Color.White;
                this.ForeColor = Color.Black;
                btnClose.BackColor = Color.White;
                btnClose.ForeColor = Color.Black;
                foreach (Control c in this.Controls)
                {
                    c.BackColor = Color.White;
                    c.ForeColor = Color.Black;
                    btnClose.BackColor = Color.White;
                    btnClose.ForeColor = Color.Black;
                }
            }
            TitleBarMinimizeVisible = false;
            TitleBarMaximizeVisible = false;

            // Initialize custom title bar
            InitializeCustomTitleBar();

            LoadListBoxFromFile();
            LoadListBoxFromFile2();
            LoadListBoxFromFile3();
            LoadListBoxFromFile4();
        }


        private void TitleBar_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = true;
                dragCursorPoint = Cursor.Position;
                dragFormPoint = this.Location;
            }
        }

        private void TitleBar_MouseMove(object? sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point diff = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(diff));
            }
        }

        private void TitleBar_MouseUp(object? sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private async void Login3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < MBListBox3.Items.Count; i++)
            {
                MBListBox3.SetSelected(i, true);
            }

            var selectedNames = MBListBox3.SelectedItems.Cast<string>().ToList();
            var duplicateAccounts = selectedNames
                .Select(name =>
                {
                    var baseName = name.EndsWith("-rk19") ? name[..^5] : name;
                    var parts = baseName.Split('-', 2); // split only once
                    return parts.Length == 2 ? parts[0] : ""; // account name
                })
                .GroupBy(account => account)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicateAccounts.Count > 0)
            {
                MessageBox.Show(
                    $"You have selected multiple characters with the same account name: {string.Join(", ", duplicateAccounts)}. Please select only one per account.",
                    "Duplicate Account Selection",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            string dataDir = "data";

            foreach (var charName in MBListBox3.Items)
            {
                string filePath = Path.Combine(dataDir, $"{charName}.bat");
                if (!File.Exists(filePath))
                {
                    MessageBox.Show($"[{charName}] Launcher batch file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    continue;
                }

                try
                {
                    await Task.Run(() =>
                    {
                        var process = new System.Diagnostics.Process();
                        process.StartInfo.FileName = "cmd.exe";
                        process.StartInfo.Arguments = $"/c \"{filePath}\"";
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.CreateNoWindow = true;
                        process.Start();
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"[{charName}] Exception: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ListBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // Modify the TitleBar_Paint method to respect TitleBarTextVisible
        private void TitleBar_Paint(object? sender, PaintEventArgs e)
        {
            if (!TitleBarTextVisible)
                return;

            using var sf = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near };
            Rectangle textRect = new(10, 0, titleBar.Width - 120, titleBar.Height);
            using var font = new Font("Segoe UI", TitleBarFontSize, FontStyle.Bold);
            using var brush = new SolidBrush(titleBar.ForeColor);
            e.Graphics.DrawString(TitleBarText, font, brush, textRect, sf);
        }

        public void InitializeCustomTitleBar()
        {
            titleBar.Paint += TitleBar_Paint;
        }

        private void MBListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void LoadListBoxFromFile()
        {
            MBListBox1.Items.Clear();
            string settingsDir = Path.Combine("data", "settings");
            string saveFilePath1 = Path.Combine(settingsDir, "SavedListItems1.txt"); // Change path if needed

            if (File.Exists(saveFilePath1))
            {
                string[] lines = File.ReadAllLines(saveFilePath1);
                MBListBox1.Items.AddRange(lines);
            }
        }

        public void LoadListBoxFromFile2()
        {
            MBListBox2.Items.Clear();
            string settingsDir = Path.Combine("data", "settings");
            string saveFilePath1 = Path.Combine(settingsDir, "SavedListItems2.txt"); // Change path if needed

            if (File.Exists(saveFilePath1))
            {
                string[] lines = File.ReadAllLines(saveFilePath1);
                MBListBox2.Items.AddRange(lines);
            }
        }

        public void LoadListBoxFromFile3()
        {
            MBListBox3.Items.Clear();
            string settingsDir = Path.Combine("data", "settings");
            string saveFilePath1 = Path.Combine(settingsDir, "SavedListItems3.txt"); // Change path if needed

            if (File.Exists(saveFilePath1))
            {
                string[] lines = File.ReadAllLines(saveFilePath1);
                MBListBox3.Items.AddRange(lines);
            }
        }

        public void LoadListBoxFromFile4()
        {
            MBListBox4.Items.Clear();
            string settingsDir = Path.Combine("data", "settings");
            string saveFilePath1 = Path.Combine(settingsDir, "SavedListItems4.txt"); // Change path if needed

            if (File.Exists(saveFilePath1))
            {
                string[] lines = File.ReadAllLines(saveFilePath1);
                MBListBox4.Items.AddRange(lines);
            }
        }


        private void DeleteButton1_Click(object sender, EventArgs e)
        {

            if (MBListBox1.SelectedItems.Count >= 1)
            {

                var selectedItems = MBListBox1.SelectedItems.Cast<string>().ToList();
                string selectedText = string.Join(Environment.NewLine, MBListBox1.SelectedItems.Cast<string>());

                var confirm = MessageBox.Show($"Are you sure you want to delete \n{selectedText}?",
                                              "Confirm Delete",
                                              MessageBoxButtons.YesNo,
                                              MessageBoxIcon.Warning);


                if (confirm == DialogResult.Yes)
                {
                    for (int i = MBListBox1.SelectedIndices.Count - 1; i >= 0; i--)
                    {
                        MBListBox1.Items.RemoveAt(MBListBox1.SelectedIndices[i]);
                    }
                }
            }

            string settingsDir = Path.Combine("data", "settings");
            string saveFilePath = Path.Combine(settingsDir, "SavedListItems1.txt"); // Change path if needed

            using StreamWriter writer = new(saveFilePath);
            foreach (var item in MBListBox1.Items)
            {
                writer.WriteLine(item.ToString());
            }


        }

        private async void LoginButton1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < MBListBox1.Items.Count; i++)
            {
                MBListBox1.SetSelected(i, true);
            }

            var selectedNames = MBListBox1.SelectedItems.Cast<string>().ToList();
            var duplicateAccounts = selectedNames
                .Select(name =>
                {
                    var baseName = name.EndsWith("-rk19") ? name[..^5] : name;
                    var parts = baseName.Split('-', 2); // split only once
                    return parts.Length == 2 ? parts[0] : ""; // account name
                })
                .GroupBy(account => account)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicateAccounts.Count > 0)
            {
                MessageBox.Show(
                    $"You have selected multiple characters with the same account name: {string.Join(", ", duplicateAccounts)}. Please select only one per account.",
                    "Duplicate Account Selection",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            string dataDir = "data";

            foreach (var charName in MBListBox1.Items)
            {
                string filePath = Path.Combine(dataDir, $"{charName}.bat");
                if (!File.Exists(filePath))
                {
                    MessageBox.Show($"[{charName}] Launcher batch file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    continue;
                }

                try
                {
                    await Task.Run(() =>
                    {
                        var process = new System.Diagnostics.Process();
                        process.StartInfo.FileName = "cmd.exe";
                        process.StartInfo.Arguments = $"/c \"{filePath}\"";
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.CreateNoWindow = true;
                        process.Start();
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"[{charName}] Exception: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public void Skuly(string colors)
        {

            Color color = Color.White;
            switch (colors)
            {
                case "Black":
                    color = Color.Black;
                    break;
                case "White":
                    color = Color.White;
                    break;
                case "Yellow":
                    color = Color.Gold;
                    break;
                case "Orange":
                    color = Color.Orange;
                    break;
                case "Green":
                    color = Color.LimeGreen;
                    break;
                case "Blue":
                    color = Color.DeepSkyBlue;
                    break;
                case "Pink":
                    color = Color.HotPink;
                    break;
                case "Purple":
                    color = Color.MediumPurple;
                    break;
            }


            foreach (Control c in this.Controls)
            {
                c.ForeColor = color;
                this.ForeColor = color;
            }




        }

        private async void LoginButton2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < MBListBox2.Items.Count; i++)
            {
                MBListBox2.SetSelected(i, true);
            }

            var selectedNames = MBListBox2.SelectedItems.Cast<string>().ToList();
            var duplicateAccounts = selectedNames
                .Select(name =>
                {
                    var baseName = name.EndsWith("-rk19") ? name[..^5] : name;
                    var parts = baseName.Split('-', 2); // split only once
                    return parts.Length == 2 ? parts[0] : ""; // account name
                })
                .GroupBy(account => account)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicateAccounts.Count > 0)
            {
                MessageBox.Show(
                    $"You have selected multiple characters with the same account name: {string.Join(", ", duplicateAccounts)}. Please select only one per account.",
                    "Duplicate Account Selection",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            string dataDir = "data";

            foreach (var charName in MBListBox2.Items)
            {
                string filePath = Path.Combine(dataDir, $"{charName}.bat");
                if (!File.Exists(filePath))
                {
                    MessageBox.Show($"[{charName}] Launcher batch file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    continue;
                }

                try
                {
                    await Task.Run(() =>
                    {
                        var process = new System.Diagnostics.Process();
                        process.StartInfo.FileName = "cmd.exe";
                        process.StartInfo.Arguments = $"/c \"{filePath}\"";
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.CreateNoWindow = true;
                        process.Start();
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"[{charName}] Exception: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DeleteButton2_Click(object sender, EventArgs e)
        {
            if (MBListBox2.SelectedItems.Count >= 1)
            {
                string? selectedItem = MBListBox2.SelectedItem?.ToString();

                var confirm = MessageBox.Show($"Are you sure you want to delete {selectedItem}?",
                                              "Confirm Delete",
                                              MessageBoxButtons.YesNo,
                                              MessageBoxIcon.Warning);


                if (confirm == DialogResult.Yes)
                {
                    for (int i = MBListBox2.SelectedIndices.Count - 1; i >= 0; i--)
                    {
                        MBListBox2.Items.RemoveAt(MBListBox2.SelectedIndices[i]);
                    }
                }
            }

            string settingsDir = Path.Combine("data", "settings");
            string saveFilePath = Path.Combine(settingsDir, "SavedListItems2.txt"); // Change path if needed

            using StreamWriter writer = new(saveFilePath);
            foreach (var item in MBListBox2.Items)
            {
                writer.WriteLine(item.ToString());
            }
        }

        private void DeleteButton3_Click(object sender, EventArgs e)
        {
            if (MBListBox3.SelectedItems.Count >= 1)
            {
                string? selectedItem = MBListBox3.SelectedItem?.ToString();

                var confirm = MessageBox.Show($"Are you sure you want to delete {selectedItem}?",
                                              "Confirm Delete",
                                              MessageBoxButtons.YesNo,
                                              MessageBoxIcon.Warning);


                if (confirm == DialogResult.Yes)
                {
                    for (int i = MBListBox3.SelectedIndices.Count - 1; i >= 0; i--)
                    {
                        MBListBox3.Items.RemoveAt(MBListBox3.SelectedIndices[i]);
                    }
                }
            }

            string settingsDir = Path.Combine("data", "settings");
            string saveFilePath = Path.Combine(settingsDir, "SavedListItems3.txt"); // Change path if needed

            using StreamWriter writer = new(saveFilePath);
            foreach (var item in MBListBox3.Items)
            {
                writer.WriteLine(item.ToString());
            }
        }

        private async void LoginButton4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < MBListBox4.Items.Count; i++)
            {
                MBListBox4.SetSelected(i, true);
            }

            var selectedNames = MBListBox4.SelectedItems.Cast<string>().ToList();
            var duplicateAccounts = selectedNames
                .Select(name =>
                {
                    var baseName = name.EndsWith("-rk19") ? name[..^5] : name;
                    var parts = baseName.Split('-', 2); // split only once
                    return parts.Length == 2 ? parts[0] : ""; // account name
                })
                .GroupBy(account => account)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            string dataDir = "data";

            if (duplicateAccounts.Count > 0)
            {
                MessageBox.Show(
                    $"You have selected multiple characters with the same account name: {string.Join(", ", duplicateAccounts)}. Please select only one per account.",
                    "Duplicate Account Selection",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            foreach (var charName in MBListBox4.Items)
            {
                string filePath = Path.Combine(dataDir, $"{charName}.bat");
                if (!File.Exists(filePath))
                {
                    MessageBox.Show($"[{charName}] Launcher batch file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    continue;
                }

                try
                {
                    await Task.Run(() =>
                    {
                        var process = new System.Diagnostics.Process();
                        process.StartInfo.FileName = "cmd.exe";
                        process.StartInfo.Arguments = $"/c \"{filePath}\"";
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.CreateNoWindow = true;
                        process.Start();
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"[{charName}] Exception: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DeleteButton4_Click(object sender, EventArgs e)
        {
            if (MBListBox4.SelectedItems.Count >= 1)
            {
                string? selectedItem = MBListBox4.SelectedItem?.ToString();

                var confirm = MessageBox.Show($"Are you sure you want to delete {selectedItem}?",
                                              "Confirm Delete",
                                              MessageBoxButtons.YesNo,
                                              MessageBoxIcon.Warning);


                if (confirm == DialogResult.Yes)
                {
                    for (int i = MBListBox4.SelectedIndices.Count - 1; i >= 0; i--)
                    {
                        MBListBox4.Items.RemoveAt(MBListBox4.SelectedIndices[i]);
                    }
                }
            }

            string settingsDir = Path.Combine("data", "settings");
            string saveFilePath = Path.Combine(settingsDir, "SavedListItems4.txt"); // Change path if needed

            using StreamWriter writer = new(saveFilePath);
            foreach (var item in MBListBox4.Items)
            {
                writer.WriteLine(item.ToString());
            }
        }

        private void MBListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void MBListBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
