using System;
using System.Windows.Forms;
using AxRANGERLib;
using FujiXerox.RangerClient.Controllers;
using FujiXerox.RangerClient.Views;

namespace FujiXerox.RangerClient
{
    public partial class SettingsForm : Form, ISettingsView
    {
        private readonly SettingController _controller;

        public SettingsForm()
        {
            InitializeComponent();
            _controller = new SettingController(this);
        }

        public AxRanger AxRanger { get; set; }

        public string GeneralOptionFilename
        {
            get { return GeneralOptionFileTextbox.Text; }
            set { SetGeneralOptionsFile(value); }
        }

        private void SetGeneralOptionsFile(string value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(SetGeneralOptionsFile), value);
                return;
            }
            GeneralOptionFileTextbox.Text = value;
        }

        private void SetDriverOptionsFile(string value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(SetDriverOptionsFile), value);
                return;
            }
            DriverOptionFileTextbox.Text = value;
        }

        public string DriverOptionFilename
        {
            get { return DriverOptionFileTextbox.Text; }
            set { SetDriverOptionsFile(value); }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void GetButton_Click(object sender, EventArgs e)
        {
            ValueTextbox.Text = _controller.GetGenericOption(SectionNameTextbox.Text, ValueNameTextbox.Text);
        }

        private void GeneralOptionFileButton_Click(object sender, EventArgs e)
        {
            var result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) _controller.SetGenericOptionFilename(openFileDialog1.FileName);
        }

        private void DriverOptionFileButton_Click(object sender, EventArgs e)
        {
            var result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) _controller.SetDriverOptionFilename(openFileDialog1.FileName);
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            _controller.GetDriverOptionFileName();
            _controller.GetGenericOptionFileName();
            _controller.Initialize();
        }
    }
}
