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
using System.Data.SqlClient;

using System.IO;
using System.Reflection;

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
            server1 = new ServerSocketStuff(ref _serverSocket);
            server1.starter();
            //server1.getData("info");
            txtBox_messageList.Text += $"Server has started and is listening {Environment.NewLine}";
        }

        private void Server_Load(object sender, EventArgs e)
        {
            ServerSocketStuff.ServerRecivedEvent += HandleServerRecived;
            ServerSocketStuff.ClientDisconnect += HandleClientDisconnected;
        }
        private void HandleClientDisconnected(ServerRecivedArgs e)
        {
            listBox_clientIP.Items.Remove(e.IP);
            txtBox_messageList.Text += $"{e.IP}:Disconnected:(( {Environment.NewLine}";
        }
        private void HandleServerRecived(ServerRecivedArgs e)
        {
            if (listBox_clientIP.FindStringExact(e.IP) == ListBox.NoMatches)
            {
                listBox_clientIP.Items.Add(e.IP);
            }
            if (e.cmd.Equals("chat") == true)
            {
                txtBox_messageList.Text += $"{e.IP}:{e.cmd_details}{Environment.NewLine}";
            }
            else if (e.cmd.Equals("info") == true)
            {
                server1.getData("info");
            }
        }
        private void btn_send_Click(object sender, EventArgs e)
        {
            if (listBox_clientIP.SelectedItem != null && !string.IsNullOrEmpty(txtBox_message.Text))
            {
                server1.sender(listBox_clientIP.SelectedItem.ToString(), txtBox_message.Text);
                txtBox_message.Text = string.Empty;
            }
            Thread.Sleep(2000) ;
            server1.sender(listBox_clientIP.SelectedItem.ToString(), "done");
        }

    }
}
