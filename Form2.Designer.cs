namespace AO_Login
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        public void InitializeComponent()
        {
            MBListBox1 = new ListBox();
            MBListBox2 = new ListBox();
            MBListBox3 = new ListBox();
            MBListBox4 = new ListBox();
            LoginButton1 = new Button();
            LoginButton2 = new Button();
            LoginButton3 = new Button();
            LoginButton4 = new Button();
            DeleteButton1 = new Button();
            DeleteButton2 = new Button();
            DeleteButton3 = new Button();
            DeleteButton4 = new Button();
            SuspendLayout();
            // 
            // MBListBox1
            // 
            MBListBox1.FormattingEnabled = true;
            MBListBox1.ItemHeight = 15;
            MBListBox1.Location = new Point(10, 40);
            MBListBox1.Name = "MBListBox1";
            MBListBox1.SelectionMode = SelectionMode.MultiExtended;
            MBListBox1.Size = new Size(155, 94);
            MBListBox1.TabIndex = 0;
            MBListBox1.SelectedIndexChanged += MBListBox1_SelectedIndexChanged;
            // 
            // MBListBox2
            // 
            MBListBox2.FormattingEnabled = true;
            MBListBox2.ItemHeight = 15;
            MBListBox2.Location = new Point(171, 40);
            MBListBox2.Name = "MBListBox2";
            MBListBox2.SelectionMode = SelectionMode.MultiExtended;
            MBListBox2.Size = new Size(155, 94);
            MBListBox2.TabIndex = 1;
            MBListBox2.SelectedIndexChanged += MBListBox2_SelectedIndexChanged;
            // 
            // MBListBox3
            // 
            MBListBox3.FormattingEnabled = true;
            MBListBox3.ItemHeight = 15;
            MBListBox3.Location = new Point(332, 40);
            MBListBox3.Name = "MBListBox3";
            MBListBox3.SelectionMode = SelectionMode.MultiExtended;
            MBListBox3.Size = new Size(155, 94);
            MBListBox3.TabIndex = 2;
            MBListBox3.SelectedIndexChanged += MBListBox3_SelectedIndexChanged;
            // 
            // MBListBox4
            // 
            MBListBox4.FormattingEnabled = true;
            MBListBox4.ItemHeight = 15;
            MBListBox4.Location = new Point(493, 40);
            MBListBox4.Name = "MBListBox4";
            MBListBox4.SelectionMode = SelectionMode.MultiSimple;
            MBListBox4.Size = new Size(155, 94);
            MBListBox4.TabIndex = 3;
            MBListBox4.SelectedIndexChanged += ListBox4_SelectedIndexChanged;
            // 
            // LoginButton1
            // 
            LoginButton1.Location = new Point(10, 140);
            LoginButton1.Name = "LoginButton1";
            LoginButton1.Size = new Size(75, 23);
            LoginButton1.TabIndex = 4;
            LoginButton1.Text = "Login";
            LoginButton1.UseVisualStyleBackColor = true;
            LoginButton1.Click += LoginButton1_Click;
            // 
            // LoginButton2
            // 
            LoginButton2.Location = new Point(171, 140);
            LoginButton2.Name = "LoginButton2";
            LoginButton2.Size = new Size(75, 23);
            LoginButton2.TabIndex = 5;
            LoginButton2.Text = "Login";
            LoginButton2.UseVisualStyleBackColor = true;
            LoginButton2.Click += LoginButton2_Click;
            // 
            // LoginButton3
            // 
            LoginButton3.Location = new Point(332, 140);
            LoginButton3.Name = "LoginButton3";
            LoginButton3.Size = new Size(75, 23);
            LoginButton3.TabIndex = 6;
            LoginButton3.Text = "Login";
            LoginButton3.UseVisualStyleBackColor = true;
            LoginButton3.Click += LoginButton3_Click;
            // 
            // LoginButton4
            // 
            LoginButton4.Location = new Point(494, 140);
            LoginButton4.Name = "LoginButton4";
            LoginButton4.Size = new Size(75, 23);
            LoginButton4.TabIndex = 7;
            LoginButton4.Text = "Login";
            LoginButton4.UseVisualStyleBackColor = true;
            LoginButton4.Click += LoginButton4_Click;
            // 
            // DeleteButton1
            // 
            DeleteButton1.Location = new Point(90, 140);
            DeleteButton1.Name = "DeleteButton1";
            DeleteButton1.Size = new Size(75, 23);
            DeleteButton1.TabIndex = 8;
            DeleteButton1.Text = "Delete";
            DeleteButton1.UseVisualStyleBackColor = true;
            DeleteButton1.Click += DeleteButton1_Click;
            // 
            // DeleteButton2
            // 
            DeleteButton2.Location = new Point(251, 140);
            DeleteButton2.Name = "DeleteButton2";
            DeleteButton2.Size = new Size(75, 23);
            DeleteButton2.TabIndex = 9;
            DeleteButton2.Text = "Delete";
            DeleteButton2.UseVisualStyleBackColor = true;
            DeleteButton2.Click += DeleteButton2_Click;
            // 
            // DeleteButton3
            // 
            DeleteButton3.Location = new Point(413, 140);
            DeleteButton3.Name = "DeleteButton3";
            DeleteButton3.Size = new Size(75, 23);
            DeleteButton3.TabIndex = 10;
            DeleteButton3.Text = "Delete";
            DeleteButton3.UseVisualStyleBackColor = true;
            DeleteButton3.Click += DeleteButton3_Click;
            // 
            // DeleteButton4
            // 
            DeleteButton4.Location = new Point(573, 140);
            DeleteButton4.Name = "DeleteButton4";
            DeleteButton4.Size = new Size(75, 23);
            DeleteButton4.TabIndex = 11;
            DeleteButton4.Text = "Delete";
            DeleteButton4.UseVisualStyleBackColor = true;
            DeleteButton4.Click += DeleteButton4_Click;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(657, 172);
            ControlBox = false;
            Controls.Add(DeleteButton4);
            Controls.Add(DeleteButton3);
            Controls.Add(DeleteButton2);
            Controls.Add(DeleteButton1);
            Controls.Add(LoginButton4);
            Controls.Add(LoginButton3);
            Controls.Add(LoginButton2);
            Controls.Add(LoginButton1);
            Controls.Add(MBListBox4);
            Controls.Add(MBListBox3);
            Controls.Add(MBListBox2);
            Controls.Add(MBListBox1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Form2";
            ShowIcon = false;
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "Multi-Box Teams";
            ResumeLayout(false);
        }

        #endregion

        private ListBox MBListBox1;
        private ListBox MBListBox2;
        private ListBox MBListBox3;
        private ListBox MBListBox4;
        private Button LoginButton1;
        private Button LoginButton2;
        private Button LoginButton3;
        private Button LoginButton4;
        private Button DeleteButton1;
        private Button DeleteButton2;
        private Button DeleteButton3;
        private Button DeleteButton4;
    }
}