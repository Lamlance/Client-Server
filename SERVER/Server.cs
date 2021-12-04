using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading;


namespace SERVER
{
   
    public partial class Server : Form
    {       
        public Server()
        {
            InitializeComponent();
        }
        private static Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        ServerSocketStuff server1;
        private void btn_start_Click(object sender, EventArgs e)
        {
            server1 = new ServerSocketStuff(ref _serverSocket, ref listBox_clientIP, ref txtBox_messageList);
            //ThreadStart childref = new ThreadStart(ServerSocketStuff.starter);
            //Thread childThread = new Thread(childref);
            //childThread.Start();
            server1.starter();
            txtBox_messageList.Text += $"Server has started and is listening {Environment.NewLine}";
        }

        private void Server_Load(object sender, EventArgs e)
        {

        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            if (listBox_clientIP.SelectedItem != null && !string.IsNullOrEmpty(txtBox_message.Text))
            {
                server1.sender(listBox_clientIP.SelectedItem.ToString(), txtBox_message.Text);
                txtBox_message.Text = string.Empty;
            }
            Thread.Sleep(2000) ;
            server1.sender2(listBox_clientIP.SelectedItem.ToString(), "done");
        }

    }
}
