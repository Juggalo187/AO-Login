namespace AO_Login
{
    partial class Form1 : Form
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        // Ensure this is the only InitializeComponent method in the Form1 class.
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            TextBoxAccount = new TextBox();
            TextBoxCharID = new TextBox();
            TextBoxCharName = new TextBox();
            LabelAccountName = new Label();
            LabelCharName = new Label();
            LabelCharID = new Label();
            ButtonSave = new Button();
            ButtonNew = new Button();
            ButtonDelete = new Button();
            ListBox1 = new ListBox();
            ContextMenuListBox = new ContextMenuStrip(components);
            Team1StripMenuItem = new ToolStripMenuItem();
            Team2StripMenuItem = new ToolStripMenuItem();
            Team3StripMenuItem = new ToolStripMenuItem();
            Team4StripMenuItem = new ToolStripMenuItem();
            ButtonAOPath = new Button();
            LoginButton = new Button();
            TextBoxPass = new TextBox();
            LabelPass = new Label();
            checkBoxRK19 = new CheckBox();
            ComboBoxProfession = new ComboBox();
            labelProfession = new Label();
            EditButton = new Button();
            CheckBoxTheme = new CheckBox();
            ComboBoxTextColor = new ComboBox();
            LabelTextColor = new Label();
            MBTeams = new Button();
            ContextMenuListBox.SuspendLayout();
            SuspendLayout();
            // 
            // TextBoxAccount
            // 
            TextBoxAccount.BorderStyle = BorderStyle.FixedSingle;
            TextBoxAccount.Enabled = false;
            TextBoxAccount.Location = new Point(275, 70);
            TextBoxAccount.Name = "TextBoxAccount";
            TextBoxAccount.Size = new Size(150, 23);
            TextBoxAccount.TabIndex = 0;
            TextBoxAccount.TextChanged += TextBoxAccount_TextChanged;
            // 
            // TextBoxCharID
            // 
            TextBoxCharID.BorderStyle = BorderStyle.FixedSingle;
            TextBoxCharID.Enabled = false;
            TextBoxCharID.Location = new Point(275, 157);
            TextBoxCharID.Name = "TextBoxCharID";
            TextBoxCharID.Size = new Size(150, 23);
            TextBoxCharID.TabIndex = 1;
            TextBoxCharID.TextChanged += TextBoxCharID_TextChanged;
            // 
            // TextBoxCharName
            // 
            TextBoxCharName.BorderStyle = BorderStyle.FixedSingle;
            TextBoxCharName.Enabled = false;
            TextBoxCharName.Location = new Point(275, 99);
            TextBoxCharName.Name = "TextBoxCharName";
            TextBoxCharName.Size = new Size(150, 23);
            TextBoxCharName.TabIndex = 3;
            TextBoxCharName.TextChanged += TextBoxCharName_TextChanged;
            // 
            // LabelAccountName
            // 
            LabelAccountName.AutoSize = true;
            LabelAccountName.Location = new Point(208, 72);
            LabelAccountName.Name = "LabelAccountName";
            LabelAccountName.Size = new Size(55, 15);
            LabelAccountName.TabIndex = 2;
            LabelAccountName.Text = "Account:";
            LabelAccountName.Click += LabelAccountName_Click;
            // 
            // LabelCharName
            // 
            LabelCharName.AutoSize = true;
            LabelCharName.Location = new Point(205, 101);
            LabelCharName.Name = "LabelCharName";
            LabelCharName.Size = new Size(61, 15);
            LabelCharName.TabIndex = 4;
            LabelCharName.Text = "Character:";
            // 
            // LabelCharID
            // 
            LabelCharID.AutoSize = true;
            LabelCharID.Location = new Point(208, 159);
            LabelCharID.Name = "LabelCharID";
            LabelCharID.Size = new Size(46, 15);
            LabelCharID.TabIndex = 5;
            LabelCharID.Text = "CharID:";
            LabelCharID.Click += LabelCharID_Click;
            // 
            // ButtonSave
            // 
            ButtonSave.Location = new Point(275, 217);
            ButtonSave.Name = "ButtonSave";
            ButtonSave.Size = new Size(67, 23);
            ButtonSave.TabIndex = 6;
            ButtonSave.Text = "Save";
            ButtonSave.UseVisualStyleBackColor = false;
            ButtonSave.Click += ButtonSave_Click;
            // 
            // ButtonNew
            // 
            ButtonNew.Location = new Point(274, 41);
            ButtonNew.Name = "ButtonNew";
            ButtonNew.Size = new Size(66, 23);
            ButtonNew.TabIndex = 7;
            ButtonNew.Text = "New";
            ButtonNew.UseVisualStyleBackColor = false;
            ButtonNew.Click += ButtonNew_Click;
            // 
            // ButtonDelete
            // 
            ButtonDelete.Location = new Point(85, 246);
            ButtonDelete.Name = "ButtonDelete";
            ButtonDelete.Size = new Size(66, 23);
            ButtonDelete.TabIndex = 8;
            ButtonDelete.Text = "Delete";
            ButtonDelete.UseVisualStyleBackColor = false;
            ButtonDelete.Click += ButtonDelete_Click;
            // 
            // ListBox1
            // 
            ListBox1.ContextMenuStrip = ContextMenuListBox;
            ListBox1.FormattingEnabled = true;
            ListBox1.ItemHeight = 15;
            ListBox1.Location = new Point(6, 41);
            ListBox1.Name = "ListBox1";
            ListBox1.SelectionMode = SelectionMode.MultiExtended;
            ListBox1.Size = new Size(193, 199);
            ListBox1.TabIndex = 9;
            ListBox1.MouseDown += ListBox1_MouseDown;
            // 
            // ContextMenuListBox
            // 
            ContextMenuListBox.Items.AddRange(new ToolStripItem[] { Team1StripMenuItem, Team2StripMenuItem, Team3StripMenuItem, Team4StripMenuItem });
            ContextMenuListBox.Name = "ContextMenuListBox";
            ContextMenuListBox.Size = new Size(173, 92);
            // 
            // Team1StripMenuItem
            // 
            Team1StripMenuItem.Name = "Team1StripMenuItem";
            Team1StripMenuItem.Size = new Size(172, 22);
            Team1StripMenuItem.Text = "Add to MB Team 1";
            Team1StripMenuItem.Click += Team1StripMenuItem_Click;
            // 
            // Team2StripMenuItem
            // 
            Team2StripMenuItem.Name = "Team2StripMenuItem";
            Team2StripMenuItem.Size = new Size(172, 22);
            Team2StripMenuItem.Text = "Add to MB Team 2";
            Team2StripMenuItem.Click += Team2StripMenuItem_Click;
            // 
            // Team3StripMenuItem
            // 
            Team3StripMenuItem.Name = "Team3StripMenuItem";
            Team3StripMenuItem.Size = new Size(172, 22);
            Team3StripMenuItem.Text = "Add to MB Team 3";
            Team3StripMenuItem.Click += Team3StripMenuItem_Click;
            // 
            // Team4StripMenuItem
            // 
            Team4StripMenuItem.Name = "Team4StripMenuItem";
            Team4StripMenuItem.Size = new Size(172, 22);
            Team4StripMenuItem.Text = "Add to MB Team 4";
            Team4StripMenuItem.Click += Team4StripMenuItem_Click;
            // 
            // ButtonAOPath
            // 
            ButtonAOPath.Location = new Point(111, 122);
            ButtonAOPath.Name = "ButtonAOPath";
            ButtonAOPath.Size = new Size(214, 23);
            ButtonAOPath.TabIndex = 10;
            ButtonAOPath.Text = "Select your AO Installation Folder.";
            ButtonAOPath.UseVisualStyleBackColor = false;
            ButtonAOPath.Visible = false;
            ButtonAOPath.Click += ButtonAOPath_Click_1;
            // 
            // LoginButton
            // 
            LoginButton.Location = new Point(9, 247);
            LoginButton.Name = "LoginButton";
            LoginButton.Size = new Size(66, 23);
            LoginButton.TabIndex = 11;
            LoginButton.Text = "Login";
            LoginButton.UseVisualStyleBackColor = false;
            LoginButton.Click += LoginButton_Click;
            // 
            // TextBoxPass
            // 
            TextBoxPass.BorderStyle = BorderStyle.FixedSingle;
            TextBoxPass.Enabled = false;
            TextBoxPass.Location = new Point(275, 128);
            TextBoxPass.Name = "TextBoxPass";
            TextBoxPass.Size = new Size(150, 23);
            TextBoxPass.PasswordChar = '*';
            TextBoxPass.TabIndex = 12;
            // 
            // LabelPass
            // 
            LabelPass.AutoSize = true;
            LabelPass.Location = new Point(208, 130);
            LabelPass.Name = "LabelPass";
            LabelPass.Size = new Size(60, 15);
            LabelPass.TabIndex = 13;
            LabelPass.Text = "Password:";
            // 
            // checkBoxRK19
            // 
            checkBoxRK19.AutoSize = true;
            checkBoxRK19.Location = new Point(216, 45);
            checkBoxRK19.Name = "checkBoxRK19";
            checkBoxRK19.Size = new Size(52, 19);
            checkBoxRK19.TabIndex = 14;
            checkBoxRK19.Text = "RK19";
            checkBoxRK19.UseVisualStyleBackColor = false;
            // 
            // ComboBoxProfession
            // 
            ComboBoxProfession.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboBoxProfession.Enabled = false;
            ComboBoxProfession.FormattingEnabled = true;
            ComboBoxProfession.Items.AddRange(new object[] { "Adventurer", "Bureaucrat", "Doctor", "Enforcer", "Engineer", "Fixer", "Keeper", "Martial Artist" });
            ComboBoxProfession.Location = new Point(275, 184);
            ComboBoxProfession.Name = "ComboBoxProfession";
            ComboBoxProfession.Size = new Size(150, 23);
            ComboBoxProfession.TabIndex = 16;
            // 
            // labelProfession
            // 
            labelProfession.AutoSize = true;
            labelProfession.Location = new Point(205, 187);
            labelProfession.Name = "labelProfession";
            labelProfession.Size = new Size(65, 15);
            labelProfession.TabIndex = 15;
            labelProfession.Text = "Profession:";
            // 
            // EditButton
            // 
            EditButton.Location = new Point(348, 217);
            EditButton.Name = "EditButton";
            EditButton.Size = new Size(66, 23);
            EditButton.TabIndex = 17;
            EditButton.Text = "Edit";
            EditButton.UseVisualStyleBackColor = false;
            EditButton.Click += EditButton_Click;
            // 
            // CheckBoxTheme
            // 
            CheckBoxTheme.AutoSize = true;
            CheckBoxTheme.Location = new Point(341, 250);
            CheckBoxTheme.Name = "CheckBoxTheme";
            CheckBoxTheme.Size = new Size(93, 19);
            CheckBoxTheme.TabIndex = 18;
            CheckBoxTheme.Text = "Light Theme";
            CheckBoxTheme.UseVisualStyleBackColor = true;
            CheckBoxTheme.CheckedChanged += CheckBoxTheme_CheckedChanged;
            // 
            // ComboBoxTextColor
            // 
            ComboBoxTextColor.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboBoxTextColor.FormattingEnabled = true;
            ComboBoxTextColor.Items.AddRange(new object[] { "White", "Yellow", "Orange", "Green", "Blue", "Pink", "Purple" });
            ComboBoxTextColor.Location = new Point(235, 248);
            ComboBoxTextColor.Name = "ComboBoxTextColor";
            ComboBoxTextColor.Size = new Size(90, 23);
            ComboBoxTextColor.TabIndex = 19;
            ComboBoxTextColor.SelectedIndexChanged += ComboBoxTextColor_SelectedIndexChanged;
            ComboBoxTextColor.SelectedValueChanged += ComboBoxTextColor_SelectedValueChanged;
            // 
            // LabelTextColor
            // 
            LabelTextColor.AutoSize = true;
            LabelTextColor.Location = new Point(166, 252);
            LabelTextColor.Name = "LabelTextColor";
            LabelTextColor.Size = new Size(63, 15);
            LabelTextColor.TabIndex = 20;
            LabelTextColor.Text = "Text Color:";
            // 
            // MBTeams
            // 
            MBTeams.Location = new Point(346, 42);
            MBTeams.Name = "MBTeams";
            MBTeams.Size = new Size(75, 23);
            MBTeams.TabIndex = 21;
            MBTeams.Text = "Teams";
            MBTeams.UseVisualStyleBackColor = true;
            MBTeams.Click += MBTeams_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(437, 280);
            Controls.Add(MBTeams);
            Controls.Add(LabelTextColor);
            Controls.Add(checkBoxRK19);
            Controls.Add(LabelPass);
            Controls.Add(TextBoxPass);
            Controls.Add(LoginButton);
            Controls.Add(ListBox1);
            Controls.Add(ButtonDelete);
            Controls.Add(ButtonNew);
            Controls.Add(ButtonSave);
            Controls.Add(LabelCharID);
            Controls.Add(LabelCharName);
            Controls.Add(LabelAccountName);
            Controls.Add(TextBoxCharName);
            Controls.Add(TextBoxAccount);
            Controls.Add(TextBoxCharID);
            Controls.Add(ButtonAOPath);
            Controls.Add(labelProfession);
            Controls.Add(ComboBoxProfession);
            Controls.Add(EditButton);
            Controls.Add(ComboBoxTextColor);
            Controls.Add(CheckBoxTheme);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Name = "Form1";
            ShowIcon = false;
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "AO Login";
            ContextMenuListBox.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox TextBoxAccount;
        private System.Windows.Forms.TextBox TextBoxCharID;
        private System.Windows.Forms.TextBox TextBoxCharName;
        private System.Windows.Forms.Label LabelAccountName;
        private System.Windows.Forms.Label LabelCharName;
        private System.Windows.Forms.Label LabelCharID;
        private System.Windows.Forms.Button ButtonSave;
        private System.Windows.Forms.Button ButtonNew;
        private System.Windows.Forms.Button ButtonDelete;
        private System.Windows.Forms.ListBox ListBox1;
        private System.Windows.Forms.Button ButtonAOPath;
        private System.Windows.Forms.Button LoginButton;
        private System.Windows.Forms.TextBox TextBoxPass;
        private System.Windows.Forms.Label LabelPass;
        private CheckBox checkBoxRK19;
        private System.Windows.Forms.ComboBox ComboBoxProfession;
        private System.Windows.Forms.Label labelProfession;
        private System.Windows.Forms.Button EditButton;
        private System.Windows.Forms.CheckBox CheckBoxTheme;
        private System.Windows.Forms.ComboBox ComboBoxTextColor;
        private Label LabelTextColor;
        private ContextMenuStrip ContextMenuListBox;
        private ToolStripMenuItem Team1StripMenuItem;
        private Button MBTeams;
        private ToolStripMenuItem Team2StripMenuItem;
        private ToolStripMenuItem Team3StripMenuItem;
        private ToolStripMenuItem Team4StripMenuItem;
    }
}
