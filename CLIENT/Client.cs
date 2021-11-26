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

namespace CLIENT
{
    public partial class ClientApp : Form
    {
        public ClientApp()
        {
            InitializeComponent();
        }
        ClientSocketStuff client;
        private static Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private void btn_connect_Click(object sender, EventArgs e)
        {
            btn_sendMess.Enabled = true;
            client = new ClientSocketStuff(ref _clientSocket, ref txtBox_serverChat);// int &a
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

        private void btn_sendMess_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtBox_message.Text))
            {
                string message = "chat*" + txtBox_message.Text;
                while (!string.IsNullOrEmpty(txtBox_message.Text))
                {
                    client.sendMessage(message);
                    txtBox_message.Text = string.Empty;
                }
                
            }
        }

        private void ClientApp_Load(object sender, EventArgs e)
        {
            btn_sendMess.Enabled = false;
        }

    }
}
