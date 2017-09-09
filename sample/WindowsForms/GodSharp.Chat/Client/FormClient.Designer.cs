namespace GodSharp.Chat.Client
{
    partial class FormClient
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormClient));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmiServerSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiServerConnect = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiServerDisconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSystemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lbClients = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rtbChatInputBox = new System.Windows.Forms.RichTextBox();
            this.rtbChatBox = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSend = new System.Windows.Forms.Button();
            this.lblOnlineNumber = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiServerSetting,
            this.tsmiServerConnect,
            this.tsmiServerDisconnect,
            this.tsmiSystemAbout});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(602, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmiServerSetting
            // 
            this.tsmiServerSetting.Name = "tsmiServerSetting";
            this.tsmiServerSetting.Size = new System.Drawing.Size(56, 20);
            this.tsmiServerSetting.Text = "Setting";
            this.tsmiServerSetting.Click += new System.EventHandler(this.tsmiServerSetting_Click);
            // 
            // tsmiServerConnect
            // 
            this.tsmiServerConnect.Name = "tsmiServerConnect";
            this.tsmiServerConnect.Size = new System.Drawing.Size(64, 20);
            this.tsmiServerConnect.Text = "Connect";
            this.tsmiServerConnect.Click += new System.EventHandler(this.tsmiServerConnect_Click);
            // 
            // tsmiServerDisconnect
            // 
            this.tsmiServerDisconnect.Name = "tsmiServerDisconnect";
            this.tsmiServerDisconnect.Size = new System.Drawing.Size(78, 20);
            this.tsmiServerDisconnect.Text = "Disconnect";
            this.tsmiServerDisconnect.Click += new System.EventHandler(this.tsmiServerDisconnect_Click);
            // 
            // tsmiSystemAbout
            // 
            this.tsmiSystemAbout.Name = "tsmiSystemAbout";
            this.tsmiSystemAbout.Size = new System.Drawing.Size(52, 20);
            this.tsmiSystemAbout.Text = "About";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lbClients);
            this.splitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.splitContainer1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.splitContainer1.Size = new System.Drawing.Size(602, 429);
            this.splitContainer1.SplitterDistance = 175;
            this.splitContainer1.TabIndex = 1;
            // 
            // lbClients
            // 
            this.lbClients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbClients.FormattingEnabled = true;
            this.lbClients.Location = new System.Drawing.Point(0, 0);
            this.lbClients.Name = "lbClients";
            this.lbClients.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lbClients.Size = new System.Drawing.Size(175, 429);
            this.lbClients.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.rtbChatInputBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.rtbChatBox, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(423, 393);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // rtbChatInputBox
            // 
            this.rtbChatInputBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbChatInputBox.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbChatInputBox.Location = new System.Drawing.Point(3, 278);
            this.rtbChatInputBox.Name = "rtbChatInputBox";
            this.rtbChatInputBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.rtbChatInputBox.Size = new System.Drawing.Size(417, 112);
            this.rtbChatInputBox.TabIndex = 1;
            this.rtbChatInputBox.Text = "Hi,world!";
            // 
            // rtbChatBox
            // 
            this.rtbChatBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbChatBox.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtbChatBox.Location = new System.Drawing.Point(3, 3);
            this.rtbChatBox.Name = "rtbChatBox";
            this.rtbChatBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.rtbChatBox.Size = new System.Drawing.Size(417, 269);
            this.rtbChatBox.TabIndex = 0;
            this.rtbChatBox.Text = "Hello world!";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblOnlineNumber);
            this.panel1.Controls.Add(this.btnSend);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 393);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(423, 36);
            this.panel1.TabIndex = 1;
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSend.Location = new System.Drawing.Point(325, 7);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // lblOnlineNumber
            // 
            this.lblOnlineNumber.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblOnlineNumber.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOnlineNumber.Location = new System.Drawing.Point(0, 0);
            this.lblOnlineNumber.Name = "lblOnlineNumber";
            this.lblOnlineNumber.Size = new System.Drawing.Size(219, 36);
            this.lblOnlineNumber.TabIndex = 1;
            this.lblOnlineNumber.Text = "Online Clients: 0";
            this.lblOnlineNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FormClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 453);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(618, 492);
            this.Name = "FormClient";
            this.Text = "GodSharp Chat Client";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RichTextBox rtbChatInputBox;
        private System.Windows.Forms.RichTextBox rtbChatBox;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.ListBox lbClients;
        private System.Windows.Forms.ToolStripMenuItem tsmiServerConnect;
        private System.Windows.Forms.ToolStripMenuItem tsmiServerDisconnect;
        private System.Windows.Forms.ToolStripMenuItem tsmiSystemAbout;
        private System.Windows.Forms.ToolStripMenuItem tsmiServerSetting;
        private System.Windows.Forms.Label lblOnlineNumber;
    }
}

