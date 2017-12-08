using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using AxRANGERLib;
using FujiXerox.RangerClient.Controllers;
using FujiXerox.RangerClient.Views;
using Serilog;

namespace FujiXerox.RangerClient
{
    public partial class MainForm : Form, IMainView
    {
        private readonly MainController _mainController;

        public MainForm()
        {
            InitializeComponent();
            var log = new LoggerConfiguration().ReadFrom.AppSettings().CreateLogger();
            Log.Logger = log;

            _mainController = new MainController(this);
        }

        public AxRanger AxRanger
        {
            get { return axRanger1; }
        }

        public string ScannerStatus
        {
            set { SetScannerStatus(value); }
        }

        public string EventStatus { set { SetEventStatus(value); } }

        public bool StartUpEnabled
        {
            set { SetStartupEnabled(value); }
        }

        public bool ShutDownEnabled
        {
            set { SetShutDownEnabled(value); }
        }

        public bool EnableOptionsEnabled
        {
            set { SetEnableOptionsEnabled(value); }
        }

        public bool StartFeedingEnabled
        {
            set { SetStartFeedingEnabled(value); }
        }

        public bool StopFeedingEnabled
        {
            set { SetStopFeedingEnabled(value); }
        }

        public bool PrepareToChangeOptionsEnabled
        {
            set { SetPrepareToChangeOptionsEnabled(value); }
        }

        public Image FrontImage
        {
            set { SetFrontImage(value); }
        }

        public Image RearImage
        {
            set { SetRearImage(value); }
        }

        public List<string> DataRow
        {
            set { AddRowToGrid(value); }
        }

        private void SetScannerStatus(string value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(SetScannerStatus), value);
                return;
            }
            ScannerStatusLabel.Text = value;
        }

        private void SetEventStatus(string value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(SetEventStatus), value);
                return;
            }
            EventStatusLabel.Text = value;
        }

        private void SetStartupEnabled(bool value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<bool>(SetStartupEnabled), value);
                return;
            }
            ConnectButton.Enabled = value;
        }

        private void SetShutDownEnabled(bool value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<bool>(SetShutDownEnabled), value);
                return;
            }
            ShutdownButton.Enabled = value;
        }

        private void SetEnableOptionsEnabled(bool value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<bool>(SetEnableOptionsEnabled), value);
                return;
            }
            EnableOptionsButton.Enabled = value;
        }

        private void SetStartFeedingEnabled(bool value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<bool>(SetStartFeedingEnabled), value);
                return;
            }
            StartFeedingButton.Enabled = value;
        }

        private void SetStopFeedingEnabled(bool value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<bool>(SetStopFeedingEnabled), value);
                return;
            }
            StopFeedingButton.Enabled = value;
        }

        private void SetPrepareToChangeOptionsEnabled(bool value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<bool>(SetPrepareToChangeOptionsEnabled), value);
                return;
            }
            ChangeOptionsButton.Enabled = value;
        }

        private void SetFrontImage(Image value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Image>(SetFrontImage), value);
                return;
            }
            FrontPictureBox.Image = value;
        }

        private void SetRearImage(Image value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Image>(SetRearImage), value);
                return;
            }
            RearPictureBox.Image = value;
        }

        private void AddRowToGrid(List<string> data)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<List<string>>(AddRowToGrid), data);
                return;
            }
            dataGridView1.Rows.Add(data.ToArray());
        }

        private void StartUpButton_Click(object sender, EventArgs e)
        {
            _mainController.StartUp();
        }

        private void ShutDownButton_Click(object sender, EventArgs e)
        {
            _mainController.ShutDown();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _mainController.ShutDown();
            _mainController.Unsubscribe();
        }

        private void EnableOptionsButton_Click(object sender, EventArgs e)
        {
            _mainController.EnableOptions();
        }

        private void StartFeedingButton_Click(object sender, EventArgs e)
        {
            _mainController.StartFeeding();
        }

        private void ChangeOptionsButton_Click(object sender, EventArgs e)
        {
            _mainController.PrepareToChangeOptions();
        }

        private void StopFeedingButton_Click(object sender, EventArgs e)
        {
            _mainController.StopFeeding();
        }

        private void splitContainer1_Resize(object sender, EventArgs e)
        {
            splitContainer1.SplitterDistance = splitContainer1.Width/2;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            _mainController.SelectRow(e.RowIndex);           
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new SettingsForm {AxRanger = AxRanger};
            form.ShowDialog();
        }
    }
}
