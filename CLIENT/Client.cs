using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;

namespace CLIENT
{
    public partial class ClientApp : Form
    {
        public ClientApp()
        {
            InitializeComponent();
        }
        private ClientSocketStuff client;
        private static Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private void btn_connect_Click(object sender, EventArgs e)
        {
            btn_sendMess.Enabled = true;
            client = new ClientSocketStuff(ref _clientSocket);// int &a
            int foundIP = txtBox_ipBox.Text.IndexOf(":");
            //string IP = txtBox_ipBox.Text.Substring(0, foundIP);
            //string port = txtBox_ipBox.Text.Substring(foundIP+1);
            if (client.Connect("POI", "POI"))
            {
                MessageBox.Show("YEAH");
            }
            else
            {
                MessageBox.Show("NO");
            }
        }
        private void HandleClientRecived(ClientRecivedArgs e)
        {
            if (e.cmd.Equals("done") == true)
            {
                txtBox_serverChat.Text += $"Server:{e.sb_buffer}{Environment.NewLine}";
            }else if(e.cmd.Equals("pict") == true)
            {
                txtBox_serverChat.Text += $"Da nhan dc hinh {Environment.NewLine}";
                txtBox_serverChat.Text += $"Nhan dc {e.sb_buffer.Length} {Environment.NewLine}";

                e.sw.Seek(0, SeekOrigin.Begin);
                using (Stream file = File.Create(@"D:\CLASS\DaiHock\DaiHoc\Dumb\Client-Server\b3.jpg"))
                {
                    e.sw.CopyTo(file);
                    file.Close();
                }
            }
            e.sb_buffer.Clear();
        }
        private void btn_sendMess_Click(object sender, EventArgs e)
        {
            //txtBox_message.Text, pict ? hỏi hình
            // thông tin chi tiet xml + hình
            // detl ==> server biet su dung sql ==> dataTable [Số][Tên][CV][CT][NOTE] 
            //                                                  *   *    x   x    x
            //  Đường dẫn hình => gửi hình
            if (!string.IsNullOrEmpty(txtBox_message.Text))
            {
                if (txtBox_message.Text.Equals("pict"))
                {
                    string message = "pict*abc";
                    client.sendMessage(message);
                    txtBox_message.Text = string.Empty;
                }
                else
                {
                    string message = "chat*" + txtBox_message.Text;
                    client.sendMessage(message);
                    txtBox_message.Text = string.Empty;
                }
                
            }
        }

        private void ClientApp_Load(object sender, EventArgs e)
        {
            ClientSocketStuff.ClientRecivedEvent += HandleClientRecived;
            btn_sendMess.Enabled = false;
        }

    }
}
