using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AO_Login
{
    public partial class Form3 : Form
    {
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        private readonly Panel titleBar; // Add readonly modifier
        private readonly Button btnClose; // Add readonly modifier
        private readonly Button btnMinimize; // Add readonly modifier
        private readonly Button btnMaximize; // Add readonly modifier
        public string PassedText;

        // Draw the title text on the custom titleBar panel with a custom font size
        public string TitleBarTextForm3 { get; set; } = "";
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
        public Form3(bool tempdt, string? TextColorName)
        {
            InitializeComponent();
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
            e.Graphics.DrawString(TitleBarTextForm3, font, brush, textRect, sf);
        }

        public void InitializeCustomTitleBar()
        {
            titleBar.Paint += TitleBar_Paint;
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


        private void Form3_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(PassedText))
            {
                string notesDir = Path.Combine("data", "notes");
                string saveFilePath1 = Path.Combine(notesDir, $"{PassedText}.txt"); // Change path if needed
                if (File.Exists(saveFilePath1))
                {
                    RichTextBox1.Text = File.ReadAllText(saveFilePath1);
                }
                else
                {
                    RichTextBox1.Clear();
                }

            }
        }

        private void SaveButtonForm3_Click(object sender, EventArgs e)
        {
            string notesDir = Path.Combine("data", "notes");
            string saveFilePath1 = Path.Combine(notesDir, $"{PassedText}.txt"); // Change path if needed
                File.WriteAllText(saveFilePath1, RichTextBox1.Text);
            MessageBox.Show($"Note for {PassedText} was saved.", "OK", MessageBoxButtons.OK, MessageBoxIcon.None);
        }
    }
}
