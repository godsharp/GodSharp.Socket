using GodSharp.Chat.Enum;
using GodSharp.Chat.Object;
using GodSharp.Chat.Properties;
using GodSharp.Sockets;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GodSharp.Chat.Server
{
    public partial class FormServer : Form
    {
        SocketServer server = null;
        int clientNumber = 0;
        
        public FormServer()
        {
            InitializeComponent();

            richTextBox1.Clear();

            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }

        private void FormServer_Load(object sender, EventArgs e)
        {
            R.ServerEndPoint = new ServerParameter() { Host = Settings.Default.ServerEndPointHost, Port = Settings.Default.ServerEndPointPort };
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (server?.Running == true)
            {
                return;
            }

            server = new SocketServer(R.ServerEndPoint.Host, R.ServerEndPoint.Port);
            server.Encoding = Encoding.UTF8;
            server.OnConnected = OnConnected;
            server.OnData = OnData;
            server.OnClosed = OnClosed;
            server.OnException = OnException;

            server.Listen();
            server.Start();

            btnStart.Enabled = !server.Running;
            btnStop.Enabled = server.Running;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (server?.Running == true)
            {
                server.Stop();
            }

            btnStart.Enabled = !server.Running;
            btnStop.Enabled = server.Running;
        }
        
        private void OnConnected(TcpSender sender)
        {
            AppendMessage(sender.RemoteEndPoint.ToString(), sender.RemoteEndPoint.ToString() + " join.", MessageType.Join);
            clientNumber = server.Clients.Count;
            SetOnlineNumber(clientNumber);

            byte[] tmp = sender.Guid.ToByteArray();
            byte[] buffer = new byte[tmp.Length + 7];

            buffer[0] = 0x02;
            buffer[buffer.Length - 6] = 0x4F;
            buffer[buffer.Length - 5] = (byte)(clientNumber & 0xff);
            buffer[buffer.Length - 4] = (byte)((clientNumber >> 8) & 0xff);
            buffer[buffer.Length - 3] = (byte)((clientNumber >> 16) & 0xff);
            buffer[buffer.Length - 2] = (byte)((clientNumber >> 24) & 0xff);
            buffer[buffer.Length - 1] = 0x03;
            Buffer.BlockCopy(tmp, 0, buffer, 1, tmp.Length);

            sender.Send(buffer);

            Send(sender, buffer);
        }

        private void OnData(TcpSender sender,byte[] bytes)
        {
            byte[] tmp = sender.Guid.ToByteArray();
            byte[] buffer = new byte[tmp.Length + bytes.Length + 3];
            buffer[0] = 0x02;
            buffer[tmp.Length + 1] = 0x4D;
            buffer[buffer.Length - 1] = 0x03;
            Buffer.BlockCopy(tmp, 0, buffer, 1, tmp.Length);
            Buffer.BlockCopy(bytes, 0, buffer, tmp.Length + 2, bytes.Length);

            Send(sender, buffer);
        }

        private void OnClosed(TcpSender sender)
        {
            AppendMessage(sender.RemoteEndPoint.ToString(), sender.RemoteEndPoint.ToString() + " leave.", MessageType.Leave);
            clientNumber = server.Clients.Count;
            SetOnlineNumber(clientNumber);
            
            byte[] tmp = sender.Guid.ToByteArray();
            byte[] buffer = new byte[tmp.Length + 7];

            buffer[0] = 0x02;
            buffer[buffer.Length - 6] = 0x43;
            buffer[buffer.Length - 5] = (byte)(clientNumber & 0xff);
            buffer[buffer.Length - 4] = (byte)((clientNumber >> 8) & 0xff);
            buffer[buffer.Length - 3] = (byte)((clientNumber >> 16) & 0xff);
            buffer[buffer.Length - 2] = (byte)((clientNumber >> 24) & 0xff);
            buffer[buffer.Length - 1] = 0x03;
            Buffer.BlockCopy(tmp, 0, buffer, 1, tmp.Length);

            Send(sender, buffer);
        }

        private void OnException(TcpSender sender, Exception exception)
        {
            Debug.WriteLine($"{sender.RemoteEndPoint.ToString()} throw exception : {exception.Message},TargetSite : {exception.TargetSite}");
        }

        private void Send(TcpSender sender, byte[] bytes)
        {
            foreach (TcpSender item in server.Clients)
            {
                if (item.Guid != sender.Guid)
                {
                    item.Send(bytes);
                }
            }
        }

        private void SetOnlineNumber(int number)
        {
            if (this.lblOnlineNumber.InvokeRequired)
            {
                Action<int> action = SetOnlineNumber;
                this.Invoke(action, new object[] { number });
            }
            else
            {
                this.lblOnlineNumber.Text = "Online Clients: " + number;
            }
        }

        private void AppendMessage(string host,string text, MessageType type=MessageType.Chat)
        {
            if (this.richTextBox1.InvokeRequired)
            {
                Action<string, string, MessageType> action = AppendMessage;
                this.Invoke(action, new object[] { host, text, type });
            }
            else
            {
                Color color = type == MessageType.Chat ? Color.Blue : (type == MessageType.Join ? Color.Green : Color.Black);
                this.richTextBox1.AppendLine(text, color);
                this.richTextBox1.ScrollToBottom();
            }
        }

        private void lblSetting_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormServerSetting form = new FormServerSetting(SocketType.Server);
            form.ShowDialog();
        }
    }
}
