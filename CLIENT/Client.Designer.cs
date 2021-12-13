
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
            this.dataGridView_info = new System.Windows.Forms.DataGridView();
            this.pictureBox_avatar = new System.Windows.Forms.PictureBox();
            this.dataGridView_detail = new System.Windows.Forms.DataGridView();
            this.textBox_log = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_info)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_avatar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_detail)).BeginInit();
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
            this.btn_connect.Location = new System.Drawing.Point(187, 470);
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
            this.txtBox_ipBox.Size = new System.Drawing.Size(193, 20);
            this.txtBox_ipBox.TabIndex = 2;
            // 
            // txtBox_serverChat
            // 
            this.txtBox_serverChat.Location = new System.Drawing.Point(69, 44);
            this.txtBox_serverChat.Multiline = true;
            this.txtBox_serverChat.Name = "txtBox_serverChat";
            this.txtBox_serverChat.ReadOnly = true;
            this.txtBox_serverChat.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtBox_serverChat.Size = new System.Drawing.Size(193, 394);
            this.txtBox_serverChat.TabIndex = 3;
            // 
            // txtBox_message
            // 
            this.txtBox_message.Location = new System.Drawing.Point(69, 444);
            this.txtBox_message.Name = "txtBox_message";
            this.txtBox_message.Size = new System.Drawing.Size(193, 20);
            this.txtBox_message.TabIndex = 5;
            // 
            // lbl_messBox
            // 
            this.lbl_messBox.AutoSize = true;
            this.lbl_messBox.Location = new System.Drawing.Point(12, 444);
            this.lbl_messBox.Name = "lbl_messBox";
            this.lbl_messBox.Size = new System.Drawing.Size(50, 13);
            this.lbl_messBox.TabIndex = 4;
            this.lbl_messBox.Text = "Message";
            // 
            // btn_sendMess
            // 
            this.btn_sendMess.Location = new System.Drawing.Point(106, 470);
            this.btn_sendMess.Name = "btn_sendMess";
            this.btn_sendMess.Size = new System.Drawing.Size(75, 23);
            this.btn_sendMess.TabIndex = 6;
            this.btn_sendMess.Text = "Send";
            this.btn_sendMess.UseVisualStyleBackColor = true;
            this.btn_sendMess.Click += new System.EventHandler(this.btn_sendMess_Click);
            // 
            // dataGridView_info
            // 
            this.dataGridView_info.AllowUserToAddRows = false;
            this.dataGridView_info.AllowUserToDeleteRows = false;
            this.dataGridView_info.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_info.Location = new System.Drawing.Point(287, 44);
            this.dataGridView_info.Name = "dataGridView_info";
            this.dataGridView_info.ReadOnly = true;
            this.dataGridView_info.Size = new System.Drawing.Size(491, 257);
            this.dataGridView_info.TabIndex = 7;
            this.dataGridView_info.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_info_CellContentClick);
            // 
            // pictureBox_avatar
            // 
            this.pictureBox_avatar.Location = new System.Drawing.Point(785, 44);
            this.pictureBox_avatar.Name = "pictureBox_avatar";
            this.pictureBox_avatar.Size = new System.Drawing.Size(245, 199);
            this.pictureBox_avatar.TabIndex = 8;
            this.pictureBox_avatar.TabStop = false;
            // 
            // dataGridView_detail
            // 
            this.dataGridView_detail.AllowUserToAddRows = false;
            this.dataGridView_detail.AllowUserToDeleteRows = false;
            this.dataGridView_detail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_detail.Location = new System.Drawing.Point(287, 336);
            this.dataGridView_detail.Name = "dataGridView_detail";
            this.dataGridView_detail.ReadOnly = true;
            this.dataGridView_detail.Size = new System.Drawing.Size(491, 150);
            this.dataGridView_detail.TabIndex = 9;
            // 
            // textBox_log
            // 
            this.textBox_log.Location = new System.Drawing.Point(785, 250);
            this.textBox_log.Multiline = true;
            this.textBox_log.Name = "textBox_log";
            this.textBox_log.ReadOnly = true;
            this.textBox_log.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_log.Size = new System.Drawing.Size(241, 236);
            this.textBox_log.TabIndex = 10;
            // 
            // ClientApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1038, 505);
            this.Controls.Add(this.textBox_log);
            this.Controls.Add(this.dataGridView_detail);
            this.Controls.Add(this.pictureBox_avatar);
            this.Controls.Add(this.dataGridView_info);
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
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_info)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_avatar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_detail)).EndInit();
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
        private System.Windows.Forms.DataGridView dataGridView_info;
        private System.Windows.Forms.PictureBox pictureBox_avatar;
        private System.Windows.Forms.DataGridView dataGridView_detail;
        private System.Windows.Forms.TextBox textBox_log;
    }
}

