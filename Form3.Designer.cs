namespace AO_Login
{
    partial class Form3
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
        private void InitializeComponent()
        {
            RichTextBox1 = new RichTextBox();
            SaveButtonForm3 = new Button();
            SuspendLayout();
            // 
            // RichTextBox1
            // 
            RichTextBox1.BorderStyle = BorderStyle.FixedSingle;
            RichTextBox1.Location = new Point(12, 32);
            RichTextBox1.Name = "RichTextBox1";
            RichTextBox1.Size = new Size(403, 216);
            RichTextBox1.TabIndex = 0;
            RichTextBox1.Text = "";
            // 
            // SaveButtonForm3
            // 
            SaveButtonForm3.Location = new Point(175, 254);
            SaveButtonForm3.Name = "SaveButtonForm3";
            SaveButtonForm3.Size = new Size(75, 23);
            SaveButtonForm3.TabIndex = 1;
            SaveButtonForm3.Text = "Save";
            SaveButtonForm3.UseVisualStyleBackColor = true;
            SaveButtonForm3.Click += SaveButtonForm3_Click;
            // 
            // Form3
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(437, 280);
            Controls.Add(SaveButtonForm3);
            Controls.Add(RichTextBox1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Form3";
            Text = "Form3";
            Load += Form3_Load;
            ResumeLayout(false);
        }

        #endregion

        public RichTextBox RichTextBox1;
        private Button SaveButtonForm3;
    }
}