using GodSharp.Chat.Enum;
using GodSharp.Chat.Object;
using GodSharp.Chat.Properties;
using System;
using System.Net;
using System.Windows.Forms;

namespace GodSharp.Chat
{
    public partial class FormServerSetting : Form
    {
        public SocketType SocketType { get; set; }

        public FormServerSetting()
        {
            InitializeComponent();
        }

        public FormServerSetting(SocketType type):this()
        {
            SocketType = type;
        }

        private void FormServerSetting_Load(object sender, EventArgs e)
        {
            this.tbHost.Text= SocketType == SocketType.Client ? R.ClientEndPoint.Host:R.ServerEndPoint.Host;
            this.nudPort.Value = SocketType == SocketType.Client ? R.ClientEndPoint.Port : R.ServerEndPoint.Port;
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

            if (SocketType == SocketType.Client)
            {
                R.ClientEndPoint.Host = this.tbHost.Text;
                R.ClientEndPoint.Port = (int)this.nudPort.Value;

                Settings.Default.ClientEndPointHost = R.ClientEndPoint.Host;
                Settings.Default.ClientEndPointPort = R.ClientEndPoint.Port;
            }
            else
            {
                R.ServerEndPoint.Host = this.tbHost.Text;
                R.ServerEndPoint.Port = (int)this.nudPort.Value;

                Settings.Default.ServerEndPointHost = R.ServerEndPoint.Host;
                Settings.Default.ServerEndPointPort = R.ServerEndPoint.Port;
            }

            Settings.Default.Save();

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}