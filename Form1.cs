using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;



namespace AO_Login
{

    public partial class Form1 : Form
    {
        private readonly Panel panelProfession; // Add readonly modifier
        private static readonly string[] ProfessionItems =
        [
            "Adventurer",
            "Agent",
            "Bureaucrat",
            "Doctor",
            "Enforcer",
            "Engineer",
            "Fixer",
            "Keeper",
            "Martial Artist",
            "Meta-Physicist",
            "Nano Technician",
            "Shade",
            "Soldier",
            "Trader"
        ];

        private static readonly string SettingsDir = Path.Combine("data", "settings");
        private static readonly string NotesDir = Path.Combine("data", "notes");
        private static readonly string OptionsFile = Path.Combine(SettingsDir, "options.json");
        private readonly AppOptions options = new(); // Add readonly modifier
        private static readonly JsonSerializerOptions CachedJsonSerializerOptions = new() { WriteIndented = true }; // Add readonly modifier

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        private readonly Panel titleBar; // Add readonly modifier
        private readonly Button btnClose; // Add readonly modifier
        private readonly Button btnMinimize; // Add readonly modifier
        private readonly Button btnMaximize; // Add readonly modifier

        // Draw the title text on the custom titleBar panel with a custom font size
        public string TitleBarText { get; set; } = "AO Login";
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

        public Form1()
        {
            Directory.CreateDirectory(SettingsDir); // Ensure settings directory exists
            Directory.CreateDirectory(NotesDir); // Ensure notes directory exists


            InitializeComponent();
            this.LocationChanged += Form1_LocationOrSizeChanged;
            this.SizeChanged += Form1_LocationOrSizeChanged;
            this.Icon = Properties.Resources.output_onlinepngtools;
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

            // Initialize panelProfession if it is not already initialized in InitializeComponent
            panelProfession = new Panel
            {
                Name = "panelProfession",
                Size = new Size(200, 100), // Example size, adjust as needed
                Location = new Point(10, 10) // Example location, adjust as needed
            };

            // Add panelProfession to the form if it is not already added
            this.Controls.Add(panelProfession);

            // Remove the panelProfession overlay and restore comboBoxProfession to the form
            if (panelProfession.Controls.Contains(ComboBoxProfession))
            {
                panelProfession.Controls.Remove(ComboBoxProfession);
                ComboBoxProfession.Location = new Point(panelProfession.Left + 1, panelProfession.Top + 1);
                this.Controls.Add(ComboBoxProfession);
                this.Controls.Remove(panelProfession);
            }

            // Load options and apply theme
            LoadOptions();
            CheckBoxTheme_CheckedChanged(this, EventArgs.Empty); // Apply theme on startup

            // Set profession combo box items
            ComboBoxProfession.Items.Clear();
            ComboBoxProfession.Items.AddRange(ProfessionItems);

            // Customize ComboBoxProfession
            ComboBoxProfession.DrawMode = DrawMode.OwnerDrawFixed;
            ComboBoxProfession.DropDownStyle = ComboBoxStyle.DropDownList; // or DropDown

            ComboBoxProfession.DrawItem += (s, e) =>
            {
                e.DrawBackground();
                // Use the current ForeColor for text, which is set in ApplyTheme
                Color textColor = ComboBoxProfession.ForeColor;
                using (var brush = new SolidBrush(textColor))
                {
                    if (e.Index >= 0)
                        if (e.Graphics != null && e.Font != null && brush != null && ComboBoxProfession.Items[e.Index] != null)
                        {
                            e.Graphics.DrawString(
                                ComboBoxProfession.Items[e.Index]!.ToString(),
                                e.Font,
                                brush,
                                e.Bounds
                            );
                        }
                }
                e.DrawFocusRectangle();
            };

            // Always show Save, Delete, and Login buttons when initialized
            ButtonSave.Visible = true;
            ButtonDelete.Visible = true;
            LoginButton.Visible = true;
            ButtonNew.Visible = true;

            // Initially disable input fields
            TextBoxCharName.Enabled = false;
            TextBoxCharID.Enabled = false;
            TextBoxAccount.Enabled = false;
            TextBoxPass.Enabled = false;

            // Attach event handler for TextBoxCharID KeyPress
            TextBoxCharID.KeyPress += TextBoxCharID_KeyPress;

            // Attach event handler for ListBox1 SelectedIndexChanged
            ListBox1.SelectedIndexChanged += ListBox1_SelectedIndexChanged;

            // Check if AOPath.txt and AOQLPath.txt exist
            string settingsDir = Path.Combine("data", "settings");
            string aoPathFile = Path.Combine(settingsDir, "AOPath.txt");
            string aoqlPathFile = Path.Combine(settingsDir, "AOQLPath.txt");
            bool aoPathExists = File.Exists(aoPathFile);
            bool aoqlPathExists = File.Exists(aoqlPathFile);

            if (!aoPathExists || !aoqlPathExists)
            {

                MaximizeBox = false;
                MdiChildrenMinimizedAnchorBottom = false;
                MinimizeBox = false;
                foreach (Control c in this.Controls)
                {
                    c.Visible = false;
                }
                titleBar.Visible = true;
                ButtonAOPath.Visible = true;
                btnMinimize.Visible = true;
                btnClose.Visible = true;


            }
            else if (aoPathExists && aoqlPathExists)
            {
                // Always show the main controls if both files exist
                ButtonSave.Visible = true;
                ButtonNew.Visible = true;
                ButtonDelete.Visible = true;
                ListBox1.Visible = true;
                LoginButton.Visible = true;
                TextBoxAccount.Visible = true;
                TextBoxCharID.Visible = true;
                TextBoxCharName.Visible = true;
                LabelAccountName.Visible = true;
                LabelCharName.Visible = true;
                LabelCharID.Visible = true;
                LabelPass.Visible = true;
                TextBoxPass.Visible = true;
                // Hide minimize and maximize buttons, and hide title text
                TitleBarMinimizeVisible = true;
                TitleBarTextVisible = true;

                LoadCharacterNames();
                MaximizeBox = true;
                MdiChildrenMinimizedAnchorBottom = true;
                MinimizeBox = true;
            }
            TitleBarMaximizeVisible = false;

            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;

            // Initialize custom title bar
            InitializeCustomTitleBar();

            // Customize ComboBoxTextColor
            ComboBoxTextColor.DrawMode = DrawMode.OwnerDrawFixed;
            ComboBoxTextColor.DropDownStyle = ComboBoxStyle.DropDownList; // or DropDown

            ComboBoxTextColor.DrawItem += (s, e) =>
            {
                e.DrawBackground();
                // Use the ComboBoxTextColor.ForeColor (which is set to the selected color)
                Color textColor = ComboBoxTextColor.ForeColor;
                if (e.Index >= 0)
                {
                    using var brush = new SolidBrush(textColor);
                    if (e.Font != null && brush != null && e.Graphics != null && ComboBoxTextColor.Items[e.Index] != null)
                    {
                        e.Graphics.DrawString(
                            ComboBoxTextColor.Items[e.Index]!.ToString(),
                            e.Font,
                            brush,
                            e.Bounds
                        );
                    }
                }
                e.DrawFocusRectangle();
            };
            ComboBoxTextColor.BackColor = Color.Black; // This will affect the dropdown, not the text area
            this.Refresh();
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

        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x84;
            const int HTCLIENT = 1;
            const int HTCAPTION = 2;
            const int HTLEFT = 10;
            const int HTRIGHT = 11;
            const int HTTOP = 12;
            const int HTTOPLEFT = 13;
            const int HTTOPRIGHT = 14;
            const int HTBOTTOM = 15;
            const int HTBOTTOMLEFT = 16;
            const int HTBOTTOMRIGHT = 17;

            if (m.Msg == WM_NCHITTEST)
            {
                base.WndProc(ref m);
                Point pos = PointToClient(new Point(m.LParam.ToInt32()));
                int grip = 8;
                if (pos.Y < titleBar.Height)
                {
                    m.Result = (IntPtr)HTCAPTION;
                    return;
                }
                if (pos.X <= grip && pos.Y <= grip)
                {
                    m.Result = (IntPtr)HTTOPLEFT;
                    return;
                }
                if (pos.X >= this.ClientSize.Width - grip && pos.Y <= grip)
                {
                    m.Result = (IntPtr)HTTOPRIGHT;
                    return;
                }
                if (pos.X <= grip && pos.Y >= this.ClientSize.Height - grip)
                {
                    m.Result = (IntPtr)HTBOTTOMLEFT;
                    return;
                }
                if (pos.X >= this.ClientSize.Width - grip && pos.Y >= this.ClientSize.Height - grip)
                {
                    m.Result = (IntPtr)HTBOTTOMRIGHT;
                    return;
                }
                if (pos.X <= grip)
                {
                    m.Result = (IntPtr)HTLEFT;
                    return;
                }
                if (pos.X >= this.ClientSize.Width - grip)
                {
                    m.Result = (IntPtr)HTRIGHT;
                    return;
                }
                if (pos.Y <= grip)
                {
                    m.Result = (IntPtr)HTTOP;
                    return;
                }
                if (pos.Y >= this.ClientSize.Height - grip)
                {
                    m.Result = (IntPtr)HTBOTTOM;
                    return;
                }
                m.Result = (IntPtr)HTCLIENT;
                return;
            }
            base.WndProc(ref m);
        }

        private void LoadOptions()
        {
            AppOptions tempOptions = new();
            if (File.Exists(OptionsFile))
            {
                try
                {
                    string json = File.ReadAllText(OptionsFile);
                    tempOptions = JsonSerializer.Deserialize<AppOptions>(json) ?? new AppOptions();
                }
                catch
                {
                    tempOptions = new AppOptions();
                }
            }
            options.DarkTheme = tempOptions.DarkTheme;
            options.TextColorName = tempOptions.TextColorName;
            CheckBoxTheme.Checked = options.DarkTheme;
            if (!options.DarkTheme && !string.IsNullOrEmpty(options.TextColorName))
            {
                ComboBoxTextColor.SelectedItem = options.TextColorName;
                ApplyTextColor(options.TextColorName);
            }
        }

        private void SaveOptions()
        {
            options.DarkTheme = CheckBoxTheme.Checked;
            if (!CheckBoxTheme.Checked)
            {
                options.TextColorName = ComboBoxTextColor.SelectedItem?.ToString();
            }
            string json = JsonSerializer.Serialize(options, CachedJsonSerializerOptions);
            File.WriteAllText(OptionsFile, json);

        }

        private void ButtonNew_Click(object? sender, EventArgs e)
        {

            ListBox1.SelectedItems.Clear();
            // Enable input fields
            TextBoxCharName.Enabled = true;
            TextBoxCharID.Enabled = true;
            TextBoxAccount.Enabled = true;
            TextBoxPass.Enabled = true;

            // Clear the input fields
            TextBoxCharName.Clear();
            TextBoxCharID.Clear();
            TextBoxAccount.Clear();
            TextBoxPass.Clear();

            // Clear ListBox selection
            //ListBox1.ClearSelected();

            ComboBoxProfession.Enabled = true;
            ComboBoxProfession.SelectedIndex = -1;
        }

        private void ButtonSave_Click(object? sender, EventArgs e)
        {
            static bool ContainsWhitespace(string input) => input.Any(char.IsWhiteSpace);

            // Get values from text boxes
            string charName = TextBoxCharName.Text;
            string charID = TextBoxCharID.Text;
            string account = TextBoxAccount.Text;
            string password = TextBoxPass.Text;

            string profession = ComboBoxProfession.SelectedItem as string ?? "";
            // ... validation: ensure a profession is selected if you want
            if (string.IsNullOrWhiteSpace(profession))
            {
                MessageBox.Show("Please select a profession.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validate all fields
            if (string.IsNullOrWhiteSpace(charName) || ContainsWhitespace(charName) ||
                string.IsNullOrWhiteSpace(charID) || ContainsWhitespace(charID) ||
                string.IsNullOrWhiteSpace(account) || ContainsWhitespace(account) ||
                string.IsNullOrWhiteSpace(password) || ContainsWhitespace(password))
            {
                MessageBox.Show("All fields (Account, Character Name, Password, Character ID) must be filled and cannot contain whitespace.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Trim values for use
            charName = charName.Trim();
            charID = charID.Trim();
            account = account.Trim();
            password = password.Trim();

            // Ensure the "data" directory exists
            string dataDir = "data";
            Directory.CreateDirectory(dataDir);

            // Read AOQuickLauncher path from AOQLPath.txt
            string settingsDir = Path.Combine("data", "settings");
            string aoqlPathFile = Path.Combine(settingsDir, "AOQLPath.txt");
            string aoqlPath = "";
            try
            {
                if (File.Exists(aoqlPathFile))
                {
                    aoqlPath = File.ReadAllText(aoqlPathFile).Trim();
                }
                else
                {
                    MessageBox.Show("AOQuickLauncher path not found. Please set it first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading AOQuickLauncher path: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string content = "";

            // Determine filename based on RK19 checkbox
            bool isRk19 = checkBoxRK19 != null && checkBoxRK19.Checked;
            string fileName = isRk19
                ? $"{account}-{charName}-rk19.bat"
                : $"{account}-{charName}.bat";
            string filePath = Path.Combine(dataDir, fileName);

            string professionLine = $":: Profession: {profession}";

            if (isRk19)
            {
                content = $@"@ECHO OFF
set AOPath=D:\SteamLibrary\steamapps\common\Anarchy Online
cd ""{aoqlPath}""
{professionLine}
::dotnet AOQuickLauncher.dll <AccName> <Password> <CharId>
dotnet AOQuickLauncher.dll --rk19 {account} {password} {charID}
";
            }
            else
            {
                content = $@"@ECHO OFF
set AOPath=D:\SteamLibrary\steamapps\common\Anarchy Online
cd ""{aoqlPath}""
{professionLine}
::dotnet AOQuickLauncher.dll <AccName> <Password> <CharId>
dotnet AOQuickLauncher.dll {account} {password} {charID}
";
            }

            // If editing, remove the old batch file if the name has changed
            if (ListBox1.SelectedItems.Count == 1)
            {
                string oldEntry = ListBox1.SelectedItem?.ToString() ?? "";
                string oldFileName = oldEntry + ".bat";
                string oldFilePath = Path.Combine(dataDir, oldFileName);
                if (!string.Equals(oldFileName, fileName, StringComparison.OrdinalIgnoreCase) && File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                    ListBox1.Items.Remove(oldEntry);
                }
            }

            try
            {
                TextBoxCharName.Enabled = true;
                File.WriteAllText(filePath, content);
                MessageBox.Show("Character launcher batch file saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.None);

                // Refresh the ListBox to ensure it always shows the latest data
                LoadCharacterNames();

                // Select the new/updated item in the ListBox
                string listBoxEntry = isRk19
                    ? $"{account}-{charName}-rk19"
                    : $"{account}-{charName}";
                int idx = ListBox1.Items.IndexOf(listBoxEntry);
                if (idx >= 0)
                {
                    ListBox1.SelectedIndex = idx;
                }

                // Clear the textboxes after save
                TextBoxCharName.Clear();
                TextBoxCharID.Clear();
                TextBoxAccount.Clear();
                TextBoxPass.Clear();

                // Disable the textboxes after save
                TextBoxCharName.Enabled = false;
                TextBoxCharID.Enabled = false;
                TextBoxAccount.Enabled = false;
                TextBoxPass.Enabled = false;

                // Clear and lock the profession combobox after save
                ComboBoxProfession.SelectedIndex = -1;
                ComboBoxProfession.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonAOPath_Click_1(object? sender, EventArgs e)
        {
            using var folderDialog = new FolderBrowserDialog
            {
                Description = "Select AO Installation Folder"
            };
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedPath = folderDialog.SelectedPath;
                string settingsDir = Path.Combine("data", "settings");
                Directory.CreateDirectory(settingsDir); // Ensure directory exists

                try
                {
                    // Check for AnarchyOnline.exe or Anarchy.exe
                    string anarchyOnlineExe = Path.Combine(selectedPath, "AnarchyOnline.exe");
                    string anarchyExe = Path.Combine(selectedPath, "Anarchy.exe");

                    if (!File.Exists(anarchyOnlineExe) && !File.Exists(anarchyExe))
                    {
                        MessageBox.Show("The selected folder does not contain AnarchyOnline.exe or Anarchy.exe. Please select the correct AO installation folder.",
                            "Invalid AO Folder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // Save AO path
                    File.WriteAllText(Path.Combine(settingsDir, "AOPath.txt"), selectedPath);
                    MessageBox.Show("AO installation path saved successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Look for AOQuickLauncher.exe automatically
                    string exeDir = AppDomain.CurrentDomain.BaseDirectory;
                    string aoqlExe = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AOQuickLauncher.exe");
                    if (File.Exists(aoqlExe))
                    {
                        File.WriteAllText(Path.Combine(settingsDir, "AOQLPath.txt"), exeDir);
                    }
                    else
                    {
                        // Ask user to locate AOQuickLauncher.exe manually
                        MessageBox.Show("Choose AOQuickLauncher.exe file.", "Next Step", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        bool validExeChosen = false;
                        string aoqlDir = "";

                        while (!validExeChosen)
                        {
                            using var openFileDialog = new OpenFileDialog
                            {
                                Title = "Select AOQuickLauncher.exe",
                                Filter = "Executable Files (*.exe)|*.exe",
                                FileName = "AOQuickLauncher.exe"
                            };
                            if (openFileDialog.ShowDialog() == DialogResult.OK)
                            {
                                string chosenPath = openFileDialog.FileName;
                                if (Path.GetFileName(chosenPath).Equals("AOQuickLauncher.exe", StringComparison.OrdinalIgnoreCase))
                                {
                                    aoqlDir = Path.GetDirectoryName(chosenPath) ?? "";
                                    File.WriteAllText(Path.Combine(settingsDir, "AOQLPath.txt"), aoqlDir);
                                    MessageBox.Show("AOQuickLauncher path saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    validExeChosen = true;
                                }
                                else
                                {
                                    MessageBox.Show("You must select AOQuickLauncher.exe.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                // Canceling deletes AOPath.txt
                                string aoPathFile = Path.Combine(settingsDir, "AOPath.txt");
                                if (File.Exists(aoPathFile))
                                {
                                    File.Delete(aoPathFile);
                                }
                                MessageBox.Show("AOQuickLauncher.exe selection was canceled. AO path will not be saved.",
                                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }

                    // Hide button and restart
                    ButtonAOPath.Visible = false;
                    Application.Restart();
                }
                catch (Exception ex)
                {
                    string aoPathFile = Path.Combine(settingsDir, "AOPath.txt");
                    if (File.Exists(aoPathFile))
                    {
                        File.Delete(aoPathFile);
                    }
                    MessageBox.Show($"Error saving path: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void LoadCharacterNames()
        {
            string dataDir = "data";
            ListBox1.Items.Clear();
            ListBox1.SelectedItems.Clear();

            if (Directory.Exists(dataDir))
            {
                var files = Directory.GetFiles(dataDir, "*.bat");
                var displayNames = new List<string>();
                foreach (var file in files)
                {
                    string name = Path.GetFileNameWithoutExtension(file);
                    // Expecting filename: account-charName or account-charName-rk19
                    string account, charName;
                    bool hasRk19 = name.EndsWith("-rk19");
                    var baseName = hasRk19 ? name[..^5] : name;
                    var parts = baseName.Split('-', 2);
                    if (parts.Length == 2)
                    {
                        account = parts[0];
                        charName = parts[1];
                        string displayName = hasRk19
                            ? $"{account}-{charName}-rk19"
                            : $"{account}-{charName}";
                        displayNames.Add(displayName);
                    }
                    else
                    {
                        // fallback to original name if parsing fails
                        displayNames.Add(name);
                    }
                }
                // Sort alphabetically
                displayNames.Sort(StringComparer.OrdinalIgnoreCase);
                foreach (var displayName in displayNames)
                {
                    ListBox1.Items.Add(displayName);
                }
            }
        }

        private void TextBoxCharID_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBoxCharID_KeyPress(object? sender, KeyPressEventArgs e)
        {
            // Allow control keys (e.g., backspace), and digits only
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TextBoxCharName_TextChanged(object sender, EventArgs e)
        {

        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (ListBox1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select one or more characters to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string dataDir = "data";
            // Use ToList() to avoid modifying the collection while iterating
            var selectedNames = ListBox1.SelectedItems.Cast<string>().ToList();

            foreach (var charName in selectedNames)
            {
                string filePath = Path.Combine(dataDir, $"{charName}.bat");
                string notesDir = Path.Combine("data", "notes");
                string notespath = Path.Combine(notesDir, $"{charName}.txt"); // Change path if needed

                var result = MessageBox.Show(
                    $"Are you sure you want to delete '{charName}'?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }

                        if (File.Exists(notespath))
                        {
                            File.Delete(notespath);
                        }

                        ListBox1.Items.Remove(charName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting file for {charName}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            Form3 openForm3 = Application.OpenForms.OfType<Form3>().FirstOrDefault();
            bool isOpen = openForm3 != null;

            if (isOpen)
            {
                openForm3.Close();
            }

            if (ListBox1.Items.Count == 0) // Replacing Any() with Count == 0
            {
                ListBox1.ClearSelected();
            }

            MessageBox.Show("Selected character(s) deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void LoginButton_Click(object sender, EventArgs e)
        {
            
            if (ListBox1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select one or more characters to login.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedNames = ListBox1.SelectedItems.Cast<string>().ToList();
            var duplicateAccounts = selectedNames
                .Select(name => ExtractAccountName(name))
                .GroupBy(account => account, StringComparer.OrdinalIgnoreCase)
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

            List<string> missingFiles = new();

            foreach (var charName in selectedNames)
            {
                string filePath = Path.Combine(dataDir, $"{charName}.bat");
                if (!File.Exists(filePath))
                {
                    missingFiles.Add(charName);
                    continue;
                }

                try
                {
                    await Task.Run(() =>
                    {
                        using var process = new System.Diagnostics.Process();
                        process.StartInfo.FileName = "cmd.exe";
                        process.StartInfo.Arguments = $"/c \"{filePath}\"";
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.CreateNoWindow = true;
                        process.Start();
                        LoginButton.Enabled = false;
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"[{charName}] Exception: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (missingFiles.Any())
            {
                MessageBox.Show($"Missing batch files for: {string.Join(", ", missingFiles)}", "Missing Files", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            LoginButton.Enabled = true;
        }

        private void ListBox1_SelectedIndexChanged(object? sender, EventArgs e)
        {
            TextBoxPass.PasswordChar = '*';
            // Always disable editing fields when selection changes
            TextBoxCharName.Enabled = false;
            TextBoxCharID.Enabled = false;
            TextBoxAccount.Enabled = false;
            TextBoxPass.Enabled = false;
            ComboBoxProfession.Enabled = false;

            if (ListBox1.SelectedItems.Count > 1)
            {
                TextBoxCharName.Clear();
                TextBoxCharID.Clear();
                TextBoxAccount.Clear();
                TextBoxPass.Clear();
                if (checkBoxRK19 != null)
                    checkBoxRK19.Checked = false;
                ComboBoxProfession.SelectedIndex = -1;
                return;
            }

            if (ListBox1.SelectedItem == null)
                return;

            string selected = ListBox1.SelectedItem.ToString() ?? "";
            bool hasRk19 = selected.EndsWith("-rk19");
            string dataDir = "data";
            string fileName = selected + ".bat";
            string filePath = Path.Combine(dataDir, fileName);

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                string? profession = lines.FirstOrDefault(line => line.StartsWith(":: Profession: "))?[":: Profession: ".Length..].Trim();
                ComboBoxProfession.SelectedItem = !string.IsNullOrEmpty(profession) ? profession : null;

                string? launcherLine = lines.FirstOrDefault(l => l.TrimStart().StartsWith("dotnet AOQuickLauncher.dll"));
                if (launcherLine != null)
                {
                    var launcherParts = launcherLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    int offset = hasRk19 && launcherParts.Length > 2 && launcherParts[2] == "--rk19" ? 3 : 2;

                    if (launcherParts.Length > offset + 2)
                    {
                        TextBoxAccount.Text = launcherParts[offset];
                        TextBoxPass.Text = launcherParts[offset + 1];
                        TextBoxCharID.Text = launcherParts[offset + 2];
                    }
                    else
                    {
                        TextBoxAccount.Clear();
                        TextBoxPass.Clear();
                        TextBoxCharID.Clear();
                    }
                }
                else
                {
                    TextBoxAccount.Clear();
                    TextBoxPass.Clear();
                    TextBoxCharID.Clear();
                }
            }

            var selectedParts = hasRk19 ? selected[..^5].Split('-', 2) : selected.Split('-', 2);
            if (selectedParts.Length == 2)
            {
                TextBoxAccount.Text = selectedParts[0];
                TextBoxCharName.Text = selectedParts[1];
            }

            if (hasRk19)
            {
                checkBoxRK19.Checked = true;
            }
            else
            {
                checkBoxRK19.Checked = false;
            }
        }

        private void LabelAccountName_Click(object sender, EventArgs e)
        {

        }

        private void TextBoxAccount_TextChanged(object sender, EventArgs e)
        {

        }

        private void LabelCharID_Click(object sender, EventArgs e)
        {

        }

        private void EditButton_Click(object? sender, EventArgs e)
        {
            if (ListBox1.SelectedItems.Count != 1)
            {
                MessageBox.Show("Please select a single character to edit.", "Edit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Enable input fields for editing
            TextBoxCharName.Enabled = true;
            TextBoxCharID.Enabled = true;
            TextBoxAccount.Enabled = true;
            TextBoxPass.Enabled = true;
            ComboBoxProfession.Enabled = true;
            //TextBoxPass.UseSystemPasswordChar = false;
            TextBoxPass.PasswordChar = '\0';
        }

        private void ApplyTheme(bool lightTheme)
        {
            Color backColor, foreColor;
            if (lightTheme)
            {
                backColor = Color.White;
                foreColor = Color.Black;
            }
            else
            {
                backColor = Color.Black;
                foreColor = Color.White;
            }

            this.BackColor = backColor;
            this.ForeColor = foreColor;

            // Recursively update all controls except comboBoxProfession and ComboBoxTextColor
            void UpdateColors(Control parent)
            {
                foreach (Control ctrl in parent.Controls)
                {
                    if ((ctrl is ComboBox cb && (cb == ComboBoxProfession || cb == ComboBoxTextColor)))
                    {
                        // Set colors explicitly after recursion
                    }
                    else
                    {
                        ctrl.BackColor = backColor;
                        ctrl.ForeColor = foreColor;
                    }
                    if (ctrl.HasChildren)
                        UpdateColors(ctrl);
                }
            }
            UpdateColors(this);

            // Set ComboBox styles and colors for both profession and text color ComboBoxes
            ComboBoxProfession.FlatStyle = FlatStyle.Standard;
            ComboBoxProfession.BackColor = backColor;
            ComboBoxProfession.ForeColor = foreColor;

            ComboBoxTextColor.FlatStyle = FlatStyle.Standard;
            ComboBoxTextColor.BackColor = backColor;
            ComboBoxTextColor.ForeColor = foreColor;

            // Set text boxes' background color to black in dark mode, white in light mode
            if (!lightTheme)
            {
                TextBoxCharName.BackColor = Color.Black;
                TextBoxCharID.BackColor = Color.Black;
                TextBoxAccount.BackColor = Color.Black;
                TextBoxPass.BackColor = Color.Black;
                ComboBoxProfession.BackColor = Color.Black;
                ComboBoxTextColor.BackColor = Color.Black;
                ComboBoxProfession.ForeColor = Color.White;
                ComboBoxTextColor.ForeColor = Color.White; // <-- Set to white for readability
            }
            else
            {
                TextBoxCharName.BackColor = backColor;
                TextBoxCharID.BackColor = backColor;
                TextBoxAccount.BackColor = backColor;
                TextBoxPass.BackColor = backColor;
                ComboBoxProfession.BackColor = backColor;
                ComboBoxTextColor.BackColor = backColor;
                ComboBoxProfession.ForeColor = backColor;
                ComboBoxTextColor.ForeColor = foreColor; // <-- Set to black for readability
            }
        }

        private void ApplyTextColor(string? colorName)
        {
            Color color = Color.White;
            switch (colorName)
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
            }
            // Also update the titlebar and its controls
            titleBar.ForeColor = color;
            btnClose.ForeColor = color;
            btnMaximize.ForeColor = color;
            btnMinimize.ForeColor = color;
            titleBar.Invalidate();

            // Set ComboBoxTextColor's ForeColor to the selected color
            ComboBoxTextColor.ForeColor = color;
        }

        public void ComboBoxTextColor_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (CheckBoxTheme.Checked)
                return;
            string? colorName = ComboBoxTextColor.SelectedItem?.ToString();
            ApplyTextColor(colorName);
            options.TextColorName = colorName;
            SaveOptions();
        }



        private void CheckBoxTheme_CheckedChanged(object? sender, EventArgs e)
        {
            ApplyTheme(CheckBoxTheme.Checked);

            if (CheckBoxTheme.Checked)
            {
                // Set ComboBoxTextColor to "Black" and hide it
                ComboBoxTextColor.SelectedItem = "Black";
                ComboBoxTextColor.Visible = false;
                ApplyTextColor("Black");
            }
            else
            {
                // Show ComboBoxTextColor and restore previous color if available
                ComboBoxTextColor.Visible = true;
                if (!string.IsNullOrEmpty(options.TextColorName))
                {
                    ComboBoxTextColor.SelectedItem = options.TextColorName;
                    ApplyTextColor(options.TextColorName);
                }
            }
            SaveOptions();
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

        private void InitializeCustomTitleBar()
        {
            titleBar.Paint += TitleBar_Paint;
        }

        private void ListBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int index = ListBox1.IndexFromPoint(e.Location);
                if (index != ListBox.NoMatches)
                {
                    if (!ListBox1.SelectedIndices.Contains(index))
                    {
                        ListBox1.ClearSelected();
                        ListBox1.SelectedIndex = index;
                    }
                }
            }
        }

        private void Team1StripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedItems = ListBox1.SelectedItems.Cast<string>().ToList();
            // Add only selected items to ListBox2 and save them
            foreach (var item in selectedItems)
            {
                if (!ListBox1.Items.Contains(item))
                    ListBox1.Items.Add(item);
            }

            // Save to file
            SaveSelectedItemsToFile(selectedItems);

            var openForm2 = Application.OpenForms.OfType<Form2>().FirstOrDefault();

            bool isOpen = Application.OpenForms["Form2"] != null;

            if (isOpen)
            {
                if (openForm2 != null)
                {
                    openForm2!.LoadListBoxFromFile();
                }
            }



        }

        private static void SaveSelectedItemsToFile(List<string> items)
        {
            string settingsDir = Path.Combine("data", "settings");
            string saveFilePath = Path.Combine(settingsDir, "SavedListItems1.txt"); // Change path if needed

            // Step 1: Read all existing lines from file (or empty if not found)
            HashSet<string> existingLines = File.Exists(saveFilePath)
                ? [.. File.ReadAllLines(saveFilePath)]
                : [];


            using StreamWriter writer = new(saveFilePath, append: true); // false = overwrite
            foreach (var item in items)
            {
                string line = item.ToString();

                if (!existingLines.Contains(line))
                {
                    writer.WriteLine(line);
                    existingLines.Add(line); // update the set in case more matches later
                }
            }
        }

        private static void SaveSelectedItemsToFile2(List<string> items)
        {
            string settingsDir = Path.Combine("data", "settings");
            string saveFilePath = Path.Combine(settingsDir, "SavedListItems2.txt"); // Change path if needed

            // Step 1: Read all existing lines from file (or empty if not found)
            HashSet<string> existingLines = File.Exists(saveFilePath)
                ? [.. File.ReadAllLines(saveFilePath)]
                : [];


            using StreamWriter writer = new(saveFilePath, append: true); // false = overwrite
            foreach (var item in items)
            {
                string line = item.ToString();

                if (!existingLines.Contains(line))
                {
                    writer.WriteLine(line);
                    existingLines.Add(line); // update the set in case more matches later
                }
            }
        }

        private static void SaveSelectedItemsToFile3(List<string> items)
        {
            string settingsDir = Path.Combine("data", "settings");
            string saveFilePath = Path.Combine(settingsDir, "SavedListItems3.txt"); // Change path if needed

            // Step 1: Read all existing lines from file (or empty if not found)
            HashSet<string> existingLines = File.Exists(saveFilePath)
                ? [.. File.ReadAllLines(saveFilePath)]
                : [];


            using StreamWriter writer = new(saveFilePath, append: true); // false = overwrite
            foreach (var item in items)
            {
                string line = item.ToString();

                if (!existingLines.Contains(line))
                {
                    writer.WriteLine(line);
                    existingLines.Add(line); // update the set in case more matches later
                }
            }
        }

        private static void SaveSelectedItemsToFile4(List<string> items)
        {
            string settingsDir = Path.Combine("data", "settings");
            string saveFilePath = Path.Combine(settingsDir, "SavedListItems4.txt"); // Change path if needed

            // Step 1: Read all existing lines from file (or empty if not found)
            HashSet<string> existingLines = File.Exists(saveFilePath)
                ? [.. File.ReadAllLines(saveFilePath)]
                : [];


            using StreamWriter writer = new(saveFilePath, append: true); // false = overwrite
            foreach (var item in items)
            {
                string line = item.ToString();

                if (!existingLines.Contains(line))
                {
                    writer.WriteLine(line);
                    existingLines.Add(line); // update the set in case more matches later
                }
            }
        }


        private void MBTeams_Click(object sender, EventArgs e)
        {
            Form2 openForm2 = Application.OpenForms.OfType<Form2>().FirstOrDefault();
            bool isOpen = openForm2 != null;

            AppOptions tempOptions = new();
            if (File.Exists(OptionsFile))
            {
                try
                {
                    string json = File.ReadAllText(OptionsFile);
                    tempOptions = JsonSerializer.Deserialize<AppOptions>(json) ?? new AppOptions();
                }
                catch
                {
                    tempOptions = new AppOptions();
                }
            }

            options.DarkTheme = tempOptions.DarkTheme;
            options.TextColorName = tempOptions.TextColorName;
            CheckBoxTheme.Checked = options.DarkTheme;
            if (!options.DarkTheme && !string.IsNullOrEmpty(options.TextColorName))
            {
                ComboBoxTextColor.SelectedItem = options.TextColorName;
                ApplyTextColor(options.TextColorName);
            }

            if (!isOpen)
            {
                Form2 newForm = new(options.DarkTheme, options.TextColorName)
                {
                    StartPosition = FormStartPosition.Manual,
                    Location = new Point(this.Location.X + this.Width + 1, this.Location.Y)
                };
                newForm.Show();
            }
            else
            {
                openForm2.Close();  // Close the open Form2
            }
        }

        private void Form1_LocationOrSizeChanged(object? sender, EventArgs e)
        {
            var openForm2 = Application.OpenForms.OfType<Form2>().FirstOrDefault();
            if (openForm2 != null && !openForm2.IsDisposed)
            {
                // Keep Form2 positioned relative to Form1
                openForm2.Location = new Point(this.Location.X + this.Width + 1, this.Location.Y);
            }
        }

        private void Team2StripMenuItem_Click(object sender, EventArgs e)
        {

            var selectedItems = ListBox1.SelectedItems.Cast<string>().ToList();
            // Add only selected items to ListBox2 and save them
            foreach (var item in selectedItems)
            {
                if (!ListBox1.Items.Contains(item))
                    ListBox1.Items.Add(item);
            }

            // Save to file
            SaveSelectedItemsToFile2(selectedItems);

            var openForm2 = Application.OpenForms.OfType<Form2>().FirstOrDefault();

            bool isOpen = Application.OpenForms["Form2"] != null;

            if (isOpen)
            {
                if (openForm2 != null)
                {
                    openForm2!.LoadListBoxFromFile2();
                }
            }

        }

        private void ComboBoxTextColor_SelectedValueChanged(object sender, EventArgs e)
        {
            bool isOpen = Application.OpenForms["Form2"] != null;

            if (isOpen)
            {
                var openForm2 = Application.OpenForms.OfType<Form2>().FirstOrDefault();
                string? ComboBoxTextColorstring = ComboBoxTextColor.SelectedItem?.ToString();

                if (openForm2 != null && ComboBoxTextColorstring != null)
                {
                    openForm2.Skuly(ComboBoxTextColorstring);
                }
            }

        }

        private void Team4StripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedItems = ListBox1.SelectedItems.Cast<string>().ToList();
            // Add only selected items to ListBox2 and save them
            foreach (var item in selectedItems)
            {
                if (!ListBox1.Items.Contains(item))
                    ListBox1.Items.Add(item);
            }

            // Save to file
            SaveSelectedItemsToFile4(selectedItems);

            var openForm2 = Application.OpenForms.OfType<Form2>().FirstOrDefault();

            bool isOpen = Application.OpenForms["Form2"] != null;

            if (isOpen)
            {
                if (openForm2 != null)
                {
                    openForm2!.LoadListBoxFromFile4();
                }
            }
        }

        private void Team3StripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedItems = ListBox1.SelectedItems.Cast<string>().ToList();
            // Add only selected items to ListBox2 and save them
            foreach (var item in selectedItems)
            {
                if (!ListBox1.Items.Contains(item))
                    ListBox1.Items.Add(item);
            }

            // Save to file
            SaveSelectedItemsToFile3(selectedItems);

            var openForm2 = Application.OpenForms.OfType<Form2>().FirstOrDefault();

            bool isOpen = Application.OpenForms["Form2"] != null;

            if (isOpen)
            {
                if (openForm2 != null)
                {
                    openForm2!.LoadListBoxFromFile3();
                }
            }
        }

        private void NotesButton_Click(object sender, EventArgs e)
        {
            // check if ListBox1 has a single item select

            if (ListBox1.SelectedItems.Count == 0 || ListBox1.SelectedItems.Count > 1)
            {
                MessageBox.Show("You must select one item to view or add a note.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            Form3 openForm3 = Application.OpenForms.OfType<Form3>().FirstOrDefault();
            bool isOpen = openForm3 != null;
            string ListBoxEntry = ListBox1.SelectedItem?.ToString();

            AppOptions tempOptions = new();
            if (File.Exists(OptionsFile))
            {
                try
                {
                    string json = File.ReadAllText(OptionsFile);
                    tempOptions = JsonSerializer.Deserialize<AppOptions>(json) ?? new AppOptions();
                }
                catch
                {
                    tempOptions = new AppOptions();
                }
            }

            options.DarkTheme = tempOptions.DarkTheme;
            options.TextColorName = tempOptions.TextColorName;
            CheckBoxTheme.Checked = options.DarkTheme;
            if (!options.DarkTheme && !string.IsNullOrEmpty(options.TextColorName))
            {
                ComboBoxTextColor.SelectedItem = options.TextColorName;
                ApplyTextColor(options.TextColorName);
            }

            if (!isOpen)
            {
                Form3 newForm = new(options.DarkTheme, options.TextColorName)
                {
                    StartPosition = FormStartPosition.Manual,
                    Location = new Point(this.Location.X + this.Width + 1, this.Location.Y),
                    PassedText = ListBoxEntry,
                    TitleBarTextForm3 = $"{ListBoxEntry} Notes"
                };
                newForm.Show();
            }
            else
            {
                openForm3.Close();  // Close the open Form3
            }




        }

        private string ExtractAccountName(string name)
        {
            if (name.EndsWith("-rk19"))
                name = name[..^5]; // Remove "-rk19"

            var parts = name.Split('-', 2);
            return parts.Length == 2 ? parts[0] : name; // fall back to full name
        }

        private void ListBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string ListBoxEntry = ListBox1.SelectedItem?.ToString();
            var openForm3 = Application.OpenForms.OfType<Form3>().FirstOrDefault();
            if (openForm3 != null)
            {
                using (Form3 newForm = new(options.DarkTheme, options.TextColorName)
                {
                    StartPosition = FormStartPosition.Manual,
                    Location = new Point(this.Location.X + this.Width + 1, this.Location.Y)
                })
                {
                }

                openForm3.PassedText = ListBoxEntry;
                openForm3.TitleBarTextForm3 = $"{ListBoxEntry} Notes";

                if (!string.IsNullOrEmpty(openForm3.PassedText))
                {
                    string notesDir = Path.Combine("data", "notes");
                    string saveFilePath1 = Path.Combine(notesDir, $"{openForm3.PassedText}.txt"); // Change path if needed
                    if (File.Exists(saveFilePath1))
                    {
                        openForm3.RichTextBox1.Text = File.ReadAllText(saveFilePath1);
                    }
                    else
                    {
                        openForm3.RichTextBox1.Clear();
                    }

                }
                openForm3.Refresh();
                openForm3.Show();
            }
        }

        private void checkBoxRK19_CheckedChanged(object sender, EventArgs e)
        {

        }
    }




    public class AppOptions
    {
        public bool DarkTheme { get; set; }
        public string? TextColorName { get; set; } // Add this property
    }
}
