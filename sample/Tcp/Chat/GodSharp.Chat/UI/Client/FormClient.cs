using GodSharp.Chat.Enum;
using GodSharp.Chat.Object;
using GodSharp.Chat.Properties;
using GodSharp.Sockets;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GodSharp.Chat.Client
{
    public partial class FormClient : Form
    {
        SocketClient client = null;
        int length = 0;
        byte[] buffer = null;

        Color toNameColor = Color.FromArgb(29, 176, 184);
        Color toTextColor = Color.FromArgb(55, 198, 192);
        Color fromNameColor = Color.FromArgb(25, 148, 117);
        Color fromTextColor = Color.FromArgb(11, 110, 72);
        Color toJoinColor = Color.FromArgb(239,206,232);
        Color fromJoinColor = Color.FromArgb(199,179,229);
        Color toLeaveColor = Color.FromArgb(233,224,106);
        Color fromLeaveColor = Color.FromArgb(254, 227, 136);

        Color color1;
        Color color2;

        string message;
        bool isSelf;
        int onlineNumber = 0;

        public FormClient()
        {
            InitializeComponent();
        }
        
        private void FormClient_Load(object sender, EventArgs e)
        {
            rtbChatBox.Clear();
            rtbChatInputBox.Clear();
            SetConnected(false);
            SetSendButton(false);

            R.ClientEndPoint = new ServerParameter() { Host = Settings.Default.ClientEndPointHost, Port = Settings.Default.ClientEndPointPort };
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string msg = this.rtbChatInputBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(msg))
            {
                return;
            }

            int count = client.Sender.Send(msg);
            if (count>0)
            {
                AppendMessage("you", msg, MessageType.Chat, true);
                rtbChatInputBox.Clear();
            }
        }
        
        private void tsmiServerSetting_Click(object sender, EventArgs e)
        {
            FormServerSetting form = new FormServerSetting(SocketType.Client);
            form.ShowDialog();
        }

        private void tsmiServerConnect_Click(object sender, EventArgs e)
        {
            client = new SocketClient(R.ClientEndPoint.Host, R.ClientEndPoint.Port);
            client.Encoding = Encoding.UTF8;
            client.OnConnected = OnConnected;
            client.OnData = OnData;
            client.OnClosed = OnClosed;
            client.OnException = OnException;

            client.Connect();
            client.Start();

            SetConnected(client.Connected);
        }

        private void tsmiServerDisconnect_Click(object sender, EventArgs e)
        {
            if (client!=null && client.Connected)
            {
                client.Stop();
                SetConnected(client.Connected);
            }
        }

        private void SetConnected(bool connected=true)
        {
            this.tsmiServerConnect.Enabled = !connected;
            this.tsmiServerSetting.Enabled = !connected;
            this.tsmiServerDisconnect.Enabled = connected;
        }

        private void OnConnected(TcpSender sender)
        {
            Debug.WriteLine($"connected to server {sender.RemoteEndPoint}");

            SetSendButton(true);
            //AppendMessage(sender.LocalEndPoint.ToString(), "join(you).", MessageType.Join, true);
        }
        
        private void OnData(TcpSender sender, byte[] bytes)
        {
            length = bytes.Length;
            if (bytes[0]==0x02)
            {
                buffer = bytes;
            }
            else
            {
                byte[] tmp = buffer;
                buffer = new byte[tmp.Length + length];

                Buffer.BlockCopy(tmp, 0, buffer, 0, tmp.Length);
                Buffer.BlockCopy(bytes, 0, buffer, tmp.Length, length);
            }

            if (buffer.Length < 19 || bytes[length - 1] != 0x03)
            {
                return;
            }

            byte[] gb = new byte[16];
            Buffer.BlockCopy(buffer, 1, gb, 0, 16);
            Guid guid = new Guid(gb);
            
            isSelf = guid == sender.Guid;

            byte cmd = buffer[17];

            switch (cmd)
            {
                case 0x4F://O
                    onlineNumber = BitConverter.ToInt32(buffer, 18);
                    SetOnlineNumber(onlineNumber);
                    AppendMessage(guid.ToString(), "join", MessageType.Join, isSelf);
                    break;
                case 0x4D://M
                    {
                        message = client.Encoding.GetString(buffer, 18, buffer.Length - 19);

                        AppendMessage(guid.ToString(), message, MessageType.Chat, isSelf);
                    }
                    break;
                case 0x43://C
                    onlineNumber = BitConverter.ToInt32(buffer, 18);
                    SetOnlineNumber(onlineNumber);
                    AppendMessage(guid.ToString(), "leave", MessageType.Leave, isSelf);
                    break;
            }
        }

        private void OnClosed(TcpSender sender)
        {
            Debug.WriteLine($"disconnect from server {sender.RemoteEndPoint}");
            //AppendMessage(sender.LocalEndPoint.ToString(), "leave(you).", MessageType.Leave, true);

            SetOnlineNumber(0);
            SetSendButton(false);
        }

        private void OnException(TcpSender sender, Exception exception)
        {
            Debug.WriteLine($"{sender.RemoteEndPoint.ToString()} throw exception : {exception.Message},TargetSite : {exception.TargetSite}");
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

        private void AppendMessage(string host, string text, MessageType type = MessageType.Chat, bool self = false)
        {
            if (this.rtbChatBox.InvokeRequired)
            {
                Action<string, string, MessageType, bool> action = AppendMessage;
                this.Invoke(action, new object[] { host, text, type, self });
            }
            else
            {
                SetColor(type, self);

                if (type == MessageType.Chat)
                {
                    this.rtbChatBox.AppendLine($"{host} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}", color1);
                    this.rtbChatBox.AppendLine(text, color2);
                }
                else
                {
                    this.rtbChatBox.AppendLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} [{host}] {text}", color2);
                }

                this.rtbChatBox.ScrollToBottom();
            }
        }

        private void SetColor(MessageType type, bool self = false)
        {
            switch (type)
            {
                case MessageType.Join:
                    color1 = self ? toJoinColor : fromJoinColor;
                    break;
                case MessageType.Chat:
                    color1 = self ? toNameColor : fromNameColor;
                    color2 = self ? toTextColor : fromTextColor;
                    break;
                case MessageType.Leave:
                    color1 = self ? toLeaveColor : fromLeaveColor;
                    break;
            }
        }

        private void SetSendButton(bool enable)
        {
            if (this.btnSend.InvokeRequired)
            {
                Action<bool> action = SetSendButton;
                this.Invoke(action, new object[] { enable });
            }
            else
            {
                btnSend.Enabled = enable;
            }
        }
    }
}
