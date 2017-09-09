using GodSharp.Chat.Object;
using System;
using System.Net;
using System.Windows.Forms;

namespace GodSharp.Chat.Client
{
    public partial class FormServerSetting : Form
    {
        public FormServerSetting()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.tbHost.Text))
            {
                this.tbHost.Focus();
                MessageBox.Show("host cannot be null", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IPAddress.TryParse(this.tbHost.Text, out _))
            {
                this.tbHost.Focus();
                MessageBox.Show("host format is incorrect", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ServerParameter.Host = this.tbHost.Text;
            ServerParameter.Port = (int)this.nudPort.Value;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}