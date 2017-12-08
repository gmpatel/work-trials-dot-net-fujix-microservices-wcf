namespace FujiXerox.RangerClient
{
    partial class SettingsForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.CloseButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SectionNameTextbox = new System.Windows.Forms.TextBox();
            this.ValueNameTextbox = new System.Windows.Forms.TextBox();
            this.ValueTextbox = new System.Windows.Forms.TextBox();
            this.GetButton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.DriverOptionFileButton = new System.Windows.Forms.Button();
            this.GeneralOptionFileButton = new System.Windows.Forms.Button();
            this.DriverOptionFileTextbox = new System.Windows.Forms.TextBox();
            this.GeneralOptionFileTextbox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Section Name";
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(433, 480);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 1;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Value Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "Value";
            // 
            // SectionNameTextbox
            // 
            this.SectionNameTextbox.Location = new System.Drawing.Point(115, 3);
            this.SectionNameTextbox.Name = "SectionNameTextbox";
            this.SectionNameTextbox.Size = new System.Drawing.Size(235, 22);
            this.SectionNameTextbox.TabIndex = 4;
            // 
            // ValueNameTextbox
            // 
            this.ValueNameTextbox.Location = new System.Drawing.Point(115, 34);
            this.ValueNameTextbox.Name = "ValueNameTextbox";
            this.ValueNameTextbox.Size = new System.Drawing.Size(235, 22);
            this.ValueNameTextbox.TabIndex = 5;
            // 
            // ValueTextbox
            // 
            this.ValueTextbox.Location = new System.Drawing.Point(115, 67);
            this.ValueTextbox.Name = "ValueTextbox";
            this.ValueTextbox.ReadOnly = true;
            this.ValueTextbox.Size = new System.Drawing.Size(235, 22);
            this.ValueTextbox.TabIndex = 6;
            // 
            // GetButton
            // 
            this.GetButton.Location = new System.Drawing.Point(404, 63);
            this.GetButton.Name = "GetButton";
            this.GetButton.Size = new System.Drawing.Size(75, 23);
            this.GetButton.TabIndex = 7;
            this.GetButton.Text = "Get";
            this.GetButton.UseVisualStyleBackColor = true;
            this.GetButton.Click += new System.EventHandler(this.GetButton_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(578, 462);
            this.tabControl1.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(570, 433);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Files";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.DriverOptionFileButton);
            this.panel2.Controls.Add(this.GeneralOptionFileButton);
            this.panel2.Controls.Add(this.DriverOptionFileTextbox);
            this.panel2.Controls.Add(this.GeneralOptionFileTextbox);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Location = new System.Drawing.Point(0, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(561, 165);
            this.panel2.TabIndex = 9;
            // 
            // DriverOptionFileButton
            // 
            this.DriverOptionFileButton.Location = new System.Drawing.Point(511, 26);
            this.DriverOptionFileButton.Name = "DriverOptionFileButton";
            this.DriverOptionFileButton.Size = new System.Drawing.Size(49, 23);
            this.DriverOptionFileButton.TabIndex = 5;
            this.DriverOptionFileButton.Text = "...";
            this.DriverOptionFileButton.UseVisualStyleBackColor = true;
            this.DriverOptionFileButton.Click += new System.EventHandler(this.DriverOptionFileButton_Click);
            // 
            // GeneralOptionFileButton
            // 
            this.GeneralOptionFileButton.Location = new System.Drawing.Point(512, 1);
            this.GeneralOptionFileButton.Name = "GeneralOptionFileButton";
            this.GeneralOptionFileButton.Size = new System.Drawing.Size(49, 23);
            this.GeneralOptionFileButton.TabIndex = 4;
            this.GeneralOptionFileButton.Text = "...";
            this.GeneralOptionFileButton.UseVisualStyleBackColor = true;
            this.GeneralOptionFileButton.Click += new System.EventHandler(this.GeneralOptionFileButton_Click);
            // 
            // DriverOptionFileTextbox
            // 
            this.DriverOptionFileTextbox.Location = new System.Drawing.Point(151, 29);
            this.DriverOptionFileTextbox.Name = "DriverOptionFileTextbox";
            this.DriverOptionFileTextbox.Size = new System.Drawing.Size(354, 22);
            this.DriverOptionFileTextbox.TabIndex = 3;
            // 
            // GeneralOptionFileTextbox
            // 
            this.GeneralOptionFileTextbox.Location = new System.Drawing.Point(151, 1);
            this.GeneralOptionFileTextbox.Name = "GeneralOptionFileTextbox";
            this.GeneralOptionFileTextbox.Size = new System.Drawing.Size(354, 22);
            this.GeneralOptionFileTextbox.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 32);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(118, 17);
            this.label5.TabIndex = 1;
            this.label5.Text = "Driver Option File";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "General Option File";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(570, 433);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.SectionNameTextbox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.GetButton);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.ValueTextbox);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.ValueNameTextbox);
            this.panel1.Location = new System.Drawing.Point(6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(489, 122);
            this.panel1.TabIndex = 9;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 514);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.CloseButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox SectionNameTextbox;
        private System.Windows.Forms.TextBox ValueNameTextbox;
        private System.Windows.Forms.TextBox ValueTextbox;
        private System.Windows.Forms.Button GetButton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button DriverOptionFileButton;
        private System.Windows.Forms.Button GeneralOptionFileButton;
        private System.Windows.Forms.TextBox DriverOptionFileTextbox;
        private System.Windows.Forms.TextBox GeneralOptionFileTextbox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}