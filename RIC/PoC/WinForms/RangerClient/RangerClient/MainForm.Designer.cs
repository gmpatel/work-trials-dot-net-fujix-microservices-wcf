namespace FujiXerox.RangerClient
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.axRanger1 = new AxRANGERLib.AxRanger();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.ShutdownButton = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ScannerStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.EventStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.EnableOptionsButton = new System.Windows.Forms.Button();
            this.StartFeedingButton = new System.Windows.Forms.Button();
            this.StopFeedingButton = new System.Windows.Forms.Button();
            this.ChangeOptionsButton = new System.Windows.Forms.Button();
            this.FrontPictureBox = new System.Windows.Forms.PictureBox();
            this.RearPictureBox = new System.Windows.Forms.PictureBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ScanID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Micr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ocr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ButtonPanel = new System.Windows.Forms.Panel();
            this.TopPanel = new System.Windows.Forms.Panel();
            this.GridPanel = new System.Windows.Forms.Panel();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.axRanger1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FrontPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RearPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.BottomPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.ButtonPanel.SuspendLayout();
            this.TopPanel.SuspendLayout();
            this.GridPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // axRanger1
            // 
            this.axRanger1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.axRanger1.Enabled = true;
            this.axRanger1.Location = new System.Drawing.Point(1043, -4);
            this.axRanger1.Name = "axRanger1";
            this.axRanger1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axRanger1.OcxState")));
            this.axRanger1.Size = new System.Drawing.Size(96, 93);
            this.axRanger1.TabIndex = 0;
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(28, 30);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(147, 32);
            this.ConnectButton.TabIndex = 1;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.StartUpButton_Click);
            // 
            // ShutdownButton
            // 
            this.ShutdownButton.Location = new System.Drawing.Point(28, 267);
            this.ShutdownButton.Name = "ShutdownButton";
            this.ShutdownButton.Size = new System.Drawing.Size(147, 32);
            this.ShutdownButton.TabIndex = 2;
            this.ShutdownButton.Text = "Shutdown";
            this.ShutdownButton.UseVisualStyleBackColor = true;
            this.ShutdownButton.Click += new System.EventHandler(this.ShutDownButton_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ScannerStatusLabel,
            this.EventStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 624);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1139, 25);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ScannerStatusLabel
            // 
            this.ScannerStatusLabel.Name = "ScannerStatusLabel";
            this.ScannerStatusLabel.Size = new System.Drawing.Size(101, 20);
            this.ScannerStatusLabel.Text = "ScannerStatus";
            // 
            // EventStatusLabel
            // 
            this.EventStatusLabel.Name = "EventStatusLabel";
            this.EventStatusLabel.Size = new System.Drawing.Size(85, 20);
            this.EventStatusLabel.Text = "EventStatus";
            // 
            // EnableOptionsButton
            // 
            this.EnableOptionsButton.Location = new System.Drawing.Point(28, 75);
            this.EnableOptionsButton.Name = "EnableOptionsButton";
            this.EnableOptionsButton.Size = new System.Drawing.Size(147, 32);
            this.EnableOptionsButton.TabIndex = 4;
            this.EnableOptionsButton.Text = "Enable Options";
            this.EnableOptionsButton.UseVisualStyleBackColor = true;
            this.EnableOptionsButton.Click += new System.EventHandler(this.EnableOptionsButton_Click);
            // 
            // StartFeedingButton
            // 
            this.StartFeedingButton.Location = new System.Drawing.Point(28, 170);
            this.StartFeedingButton.Name = "StartFeedingButton";
            this.StartFeedingButton.Size = new System.Drawing.Size(147, 32);
            this.StartFeedingButton.TabIndex = 5;
            this.StartFeedingButton.Text = "Start Feeding";
            this.StartFeedingButton.UseVisualStyleBackColor = true;
            this.StartFeedingButton.Click += new System.EventHandler(this.StartFeedingButton_Click);
            // 
            // StopFeedingButton
            // 
            this.StopFeedingButton.Location = new System.Drawing.Point(28, 219);
            this.StopFeedingButton.Name = "StopFeedingButton";
            this.StopFeedingButton.Size = new System.Drawing.Size(147, 32);
            this.StopFeedingButton.TabIndex = 6;
            this.StopFeedingButton.Text = "Stop Feeding";
            this.StopFeedingButton.UseVisualStyleBackColor = true;
            this.StopFeedingButton.Click += new System.EventHandler(this.StopFeedingButton_Click);
            // 
            // ChangeOptionsButton
            // 
            this.ChangeOptionsButton.Location = new System.Drawing.Point(28, 121);
            this.ChangeOptionsButton.Name = "ChangeOptionsButton";
            this.ChangeOptionsButton.Size = new System.Drawing.Size(147, 32);
            this.ChangeOptionsButton.TabIndex = 7;
            this.ChangeOptionsButton.Text = "Change Options";
            this.ChangeOptionsButton.UseVisualStyleBackColor = true;
            this.ChangeOptionsButton.Click += new System.EventHandler(this.ChangeOptionsButton_Click);
            // 
            // FrontPictureBox
            // 
            this.FrontPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FrontPictureBox.Location = new System.Drawing.Point(0, 0);
            this.FrontPictureBox.Name = "FrontPictureBox";
            this.FrontPictureBox.Size = new System.Drawing.Size(599, 294);
            this.FrontPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.FrontPictureBox.TabIndex = 10;
            this.FrontPictureBox.TabStop = false;
            // 
            // RearPictureBox
            // 
            this.RearPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RearPictureBox.Location = new System.Drawing.Point(0, 0);
            this.RearPictureBox.Name = "RearPictureBox";
            this.RearPictureBox.Size = new System.Drawing.Size(536, 294);
            this.RearPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.RearPictureBox.TabIndex = 11;
            this.RearPictureBox.TabStop = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ScanID,
            this.Micr,
            this.Ocr});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(932, 298);
            this.dataGridView1.TabIndex = 12;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // ScanID
            // 
            this.ScanID.HeaderText = "Scan Id";
            this.ScanID.Name = "ScanID";
            this.ScanID.ReadOnly = true;
            // 
            // Micr
            // 
            this.Micr.HeaderText = "Micr";
            this.Micr.Name = "Micr";
            this.Micr.ReadOnly = true;
            // 
            // Ocr
            // 
            this.Ocr.HeaderText = "Ocr";
            this.Ocr.Name = "Ocr";
            this.Ocr.ReadOnly = true;
            // 
            // BottomPanel
            // 
            this.BottomPanel.Controls.Add(this.splitContainer1);
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BottomPanel.Location = new System.Drawing.Point(0, 0);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new System.Drawing.Size(1139, 294);
            this.BottomPanel.TabIndex = 13;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.FrontPictureBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.RearPictureBox);
            this.splitContainer1.Size = new System.Drawing.Size(1139, 294);
            this.splitContainer1.SplitterDistance = 599;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.Resize += new System.EventHandler(this.splitContainer1_Resize);
            // 
            // ButtonPanel
            // 
            this.ButtonPanel.Controls.Add(this.ConnectButton);
            this.ButtonPanel.Controls.Add(this.ShutdownButton);
            this.ButtonPanel.Controls.Add(this.EnableOptionsButton);
            this.ButtonPanel.Controls.Add(this.StartFeedingButton);
            this.ButtonPanel.Controls.Add(this.StopFeedingButton);
            this.ButtonPanel.Controls.Add(this.ChangeOptionsButton);
            this.ButtonPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.ButtonPanel.Location = new System.Drawing.Point(0, 0);
            this.ButtonPanel.Name = "ButtonPanel";
            this.ButtonPanel.Size = new System.Drawing.Size(207, 298);
            this.ButtonPanel.TabIndex = 14;
            // 
            // TopPanel
            // 
            this.TopPanel.Controls.Add(this.GridPanel);
            this.TopPanel.Controls.Add(this.ButtonPanel);
            this.TopPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TopPanel.Location = new System.Drawing.Point(0, 0);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Size = new System.Drawing.Size(1139, 298);
            this.TopPanel.TabIndex = 15;
            // 
            // GridPanel
            // 
            this.GridPanel.Controls.Add(this.dataGridView1);
            this.GridPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridPanel.Location = new System.Drawing.Point(207, 0);
            this.GridPanel.Name = "GridPanel";
            this.GridPanel.Size = new System.Drawing.Size(932, 298);
            this.GridPanel.TabIndex = 15;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 28);
            this.splitContainer2.MinimumSize = new System.Drawing.Size(0, 312);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.TopPanel);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.BottomPanel);
            this.splitContainer2.Size = new System.Drawing.Size(1139, 596);
            this.splitContainer2.SplitterDistance = 298;
            this.splitContainer2.TabIndex = 16;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1139, 28);
            this.menuStrip1.TabIndex = 17;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(73, 24);
            this.optionsToolStripMenuItem.Text = "&Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1139, 649);
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.axRanger1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Fuji Xerox Australia - Ranger Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.axRanger1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FrontPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RearPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.BottomPanel.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ButtonPanel.ResumeLayout(false);
            this.TopPanel.ResumeLayout(false);
            this.GridPanel.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AxRANGERLib.AxRanger axRanger1;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Button ShutdownButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ScannerStatusLabel;
        private System.Windows.Forms.Button EnableOptionsButton;
        private System.Windows.Forms.Button StartFeedingButton;
        private System.Windows.Forms.Button StopFeedingButton;
        private System.Windows.Forms.Button ChangeOptionsButton;
        private System.Windows.Forms.ToolStripStatusLabel EventStatusLabel;
        private System.Windows.Forms.PictureBox FrontPictureBox;
        private System.Windows.Forms.PictureBox RearPictureBox;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScanID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Micr;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ocr;
        private System.Windows.Forms.Panel BottomPanel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel ButtonPanel;
        private System.Windows.Forms.Panel TopPanel;
        private System.Windows.Forms.Panel GridPanel;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
    }
}

