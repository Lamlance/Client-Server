
namespace CLIENT
{
    partial class ClientApp
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
            this.lbl_serverIp = new System.Windows.Forms.Label();
            this.btn_connect = new System.Windows.Forms.Button();
            this.txtBox_ipBox = new System.Windows.Forms.TextBox();
            this.txtBox_serverChat = new System.Windows.Forms.TextBox();
            this.txtBox_message = new System.Windows.Forms.TextBox();
            this.lbl_messBox = new System.Windows.Forms.Label();
            this.btn_sendMess = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_serverIp
            // 
            this.lbl_serverIp.AutoSize = true;
            this.lbl_serverIp.Location = new System.Drawing.Point(12, 18);
            this.lbl_serverIp.Name = "lbl_serverIp";
            this.lbl_serverIp.Size = new System.Drawing.Size(51, 13);
            this.lbl_serverIp.TabIndex = 0;
            this.lbl_serverIp.Text = "Server IP";
            // 
            // btn_connect
            // 
            this.btn_connect.Location = new System.Drawing.Point(333, 288);
            this.btn_connect.Name = "btn_connect";
            this.btn_connect.Size = new System.Drawing.Size(75, 23);
            this.btn_connect.TabIndex = 1;
            this.btn_connect.Text = "Connect";
            this.btn_connect.UseVisualStyleBackColor = true;
            this.btn_connect.Click += new System.EventHandler(this.btn_connect_Click);
            // 
            // txtBox_ipBox
            // 
            this.txtBox_ipBox.Location = new System.Drawing.Point(69, 18);
            this.txtBox_ipBox.Name = "txtBox_ipBox";
            this.txtBox_ipBox.Size = new System.Drawing.Size(339, 20);
            this.txtBox_ipBox.TabIndex = 2;
            // 
            // txtBox_serverChat
            // 
            this.txtBox_serverChat.Location = new System.Drawing.Point(69, 44);
            this.txtBox_serverChat.Multiline = true;
            this.txtBox_serverChat.Name = "txtBox_serverChat";
            this.txtBox_serverChat.ReadOnly = true;
            this.txtBox_serverChat.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtBox_serverChat.Size = new System.Drawing.Size(339, 212);
            this.txtBox_serverChat.TabIndex = 3;
            // 
            // txtBox_message
            // 
            this.txtBox_message.Location = new System.Drawing.Point(69, 262);
            this.txtBox_message.Name = "txtBox_message";
            this.txtBox_message.Size = new System.Drawing.Size(339, 20);
            this.txtBox_message.TabIndex = 5;
            // 
            // lbl_messBox
            // 
            this.lbl_messBox.AutoSize = true;
            this.lbl_messBox.Location = new System.Drawing.Point(12, 262);
            this.lbl_messBox.Name = "lbl_messBox";
            this.lbl_messBox.Size = new System.Drawing.Size(50, 13);
            this.lbl_messBox.TabIndex = 4;
            this.lbl_messBox.Text = "Message";
            // 
            // btn_sendMess
            // 
            this.btn_sendMess.Location = new System.Drawing.Point(252, 288);
            this.btn_sendMess.Name = "btn_sendMess";
            this.btn_sendMess.Size = new System.Drawing.Size(75, 23);
            this.btn_sendMess.TabIndex = 6;
            this.btn_sendMess.Text = "Send";
            this.btn_sendMess.UseVisualStyleBackColor = true;
            this.btn_sendMess.Click += new System.EventHandler(this.btn_sendMess_Click);
            // 
            // ClientApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 323);
            this.Controls.Add(this.btn_sendMess);
            this.Controls.Add(this.txtBox_message);
            this.Controls.Add(this.lbl_messBox);
            this.Controls.Add(this.txtBox_serverChat);
            this.Controls.Add(this.txtBox_ipBox);
            this.Controls.Add(this.btn_connect);
            this.Controls.Add(this.lbl_serverIp);
            this.Name = "ClientApp";
            this.Text = "Client";
            this.Load += new System.EventHandler(this.ClientApp_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_serverIp;
        private System.Windows.Forms.Button btn_connect;
        private System.Windows.Forms.TextBox txtBox_ipBox;
        private System.Windows.Forms.TextBox txtBox_serverChat;
        private System.Windows.Forms.TextBox txtBox_message;
        private System.Windows.Forms.Label lbl_messBox;
        private System.Windows.Forms.Button btn_sendMess;
    }
}

