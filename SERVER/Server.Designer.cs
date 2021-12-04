
namespace SERVER
{
    partial class Server
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server));
            this.lbl_serverIP = new System.Windows.Forms.Label();
            this.txtBox_serverIP = new System.Windows.Forms.TextBox();
            this.btn_start = new System.Windows.Forms.Button();
            this.txtBox_message = new System.Windows.Forms.TextBox();
            this.lbl_message = new System.Windows.Forms.Label();
            this.txtBox_messageList = new System.Windows.Forms.TextBox();
            this.btn_send = new System.Windows.Forms.Button();
            this.listBox_clientIP = new System.Windows.Forms.ListBox();
            this.lbl_clientIP = new System.Windows.Forms.Label();
            this.picBox_test = new System.Windows.Forms.PictureBox();
            this.btn_Pict = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picBox_test)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_serverIP
            // 
            this.lbl_serverIP.AutoSize = true;
            this.lbl_serverIP.Location = new System.Drawing.Point(41, 33);
            this.lbl_serverIP.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_serverIP.Name = "lbl_serverIP";
            this.lbl_serverIP.Size = new System.Drawing.Size(50, 17);
            this.lbl_serverIP.TabIndex = 0;
            this.lbl_serverIP.Text = "Server";
            // 
            // txtBox_serverIP
            // 
            this.txtBox_serverIP.Location = new System.Drawing.Point(100, 33);
            this.txtBox_serverIP.Margin = new System.Windows.Forms.Padding(4);
            this.txtBox_serverIP.Name = "txtBox_serverIP";
            this.txtBox_serverIP.Size = new System.Drawing.Size(485, 22);
            this.txtBox_serverIP.TabIndex = 1;
            // 
            // btn_start
            // 
            this.btn_start.Location = new System.Drawing.Point(487, 447);
            this.btn_start.Margin = new System.Windows.Forms.Padding(4);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(100, 28);
            this.btn_start.TabIndex = 2;
            this.btn_start.Text = "Start";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // txtBox_message
            // 
            this.txtBox_message.Location = new System.Drawing.Point(100, 415);
            this.txtBox_message.Margin = new System.Windows.Forms.Padding(4);
            this.txtBox_message.Name = "txtBox_message";
            this.txtBox_message.Size = new System.Drawing.Size(485, 22);
            this.txtBox_message.TabIndex = 4;
            // 
            // lbl_message
            // 
            this.lbl_message.AutoSize = true;
            this.lbl_message.Location = new System.Drawing.Point(25, 415);
            this.lbl_message.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_message.Name = "lbl_message";
            this.lbl_message.Size = new System.Drawing.Size(65, 17);
            this.lbl_message.TabIndex = 3;
            this.lbl_message.Text = "Message";
            // 
            // txtBox_messageList
            // 
            this.txtBox_messageList.Location = new System.Drawing.Point(100, 65);
            this.txtBox_messageList.Margin = new System.Windows.Forms.Padding(4);
            this.txtBox_messageList.Multiline = true;
            this.txtBox_messageList.Name = "txtBox_messageList";
            this.txtBox_messageList.ReadOnly = true;
            this.txtBox_messageList.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtBox_messageList.Size = new System.Drawing.Size(485, 341);
            this.txtBox_messageList.TabIndex = 5;
            // 
            // btn_send
            // 
            this.btn_send.Location = new System.Drawing.Point(379, 447);
            this.btn_send.Margin = new System.Windows.Forms.Padding(4);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(100, 28);
            this.btn_send.TabIndex = 6;
            this.btn_send.Text = "Send";
            this.btn_send.UseVisualStyleBackColor = true;
            this.btn_send.Click += new System.EventHandler(this.btn_send_Click);
            // 
            // listBox_clientIP
            // 
            this.listBox_clientIP.FormattingEnabled = true;
            this.listBox_clientIP.ItemHeight = 16;
            this.listBox_clientIP.Location = new System.Drawing.Point(631, 65);
            this.listBox_clientIP.Margin = new System.Windows.Forms.Padding(4);
            this.listBox_clientIP.Name = "listBox_clientIP";
            this.listBox_clientIP.Size = new System.Drawing.Size(331, 84);
            this.listBox_clientIP.TabIndex = 7;
            // 
            // lbl_clientIP
            // 
            this.lbl_clientIP.AutoSize = true;
            this.lbl_clientIP.Location = new System.Drawing.Point(627, 33);
            this.lbl_clientIP.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_clientIP.Name = "lbl_clientIP";
            this.lbl_clientIP.Size = new System.Drawing.Size(55, 17);
            this.lbl_clientIP.TabIndex = 8;
            this.lbl_clientIP.Text = "ClientIP";
            // 
            // picBox_test
            // 
            this.picBox_test.Image = ((System.Drawing.Image)(resources.GetObject("picBox_test.Image")));
            this.picBox_test.Location = new System.Drawing.Point(631, 171);
            this.picBox_test.Margin = new System.Windows.Forms.Padding(4);
            this.picBox_test.Name = "picBox_test";
            this.picBox_test.Size = new System.Drawing.Size(332, 304);
            this.picBox_test.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBox_test.TabIndex = 9;
            this.picBox_test.TabStop = false;
            // 
            // btn_Pict
            // 
            this.btn_Pict.Location = new System.Drawing.Point(297, 452);
            this.btn_Pict.Name = "btn_Pict";
            this.btn_Pict.Size = new System.Drawing.Size(75, 23);
            this.btn_Pict.TabIndex = 10;
            this.btn_Pict.Text = "Pict";
            this.btn_Pict.UseVisualStyleBackColor = true;
            this.btn_Pict.Click += new System.EventHandler(this.btn_Pict_Click);
            // 
            // Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1205, 554);
            this.Controls.Add(this.btn_Pict);
            this.Controls.Add(this.picBox_test);
            this.Controls.Add(this.lbl_clientIP);
            this.Controls.Add(this.listBox_clientIP);
            this.Controls.Add(this.btn_send);
            this.Controls.Add(this.txtBox_messageList);
            this.Controls.Add(this.txtBox_message);
            this.Controls.Add(this.lbl_message);
            this.Controls.Add(this.btn_start);
            this.Controls.Add(this.txtBox_serverIP);
            this.Controls.Add(this.lbl_serverIP);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Server";
            this.Text = "ServerApp";
            this.Load += new System.EventHandler(this.Server_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picBox_test)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_serverIP;
        private System.Windows.Forms.TextBox txtBox_serverIP;
        private System.Windows.Forms.Button btn_start;
        private System.Windows.Forms.TextBox txtBox_message;
        private System.Windows.Forms.Label lbl_message;
        private System.Windows.Forms.TextBox txtBox_messageList;
        private System.Windows.Forms.Button btn_send;
        private System.Windows.Forms.ListBox listBox_clientIP;
        private System.Windows.Forms.Label lbl_clientIP;
        private System.Windows.Forms.PictureBox picBox_test;
        private System.Windows.Forms.Button btn_Pict;
    }
}

