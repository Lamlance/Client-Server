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
            ServerSocketStuff.ClientConnected += HandleClientConnected;
        }

        private delegate void addStringCallBack(string message,bool isRemove = false);
        private void EndAsync(IAsyncResult async)
        {
            addStringCallBack callback = (addStringCallBack)async.AsyncState;
            callback.EndInvoke(async);
        }
        private void AddStringTo_messageList(string message, bool isRemove = false)
        {
            txtBox_messageList.Text += $"{message}{Environment.NewLine}";
        }
        private void AddRemove_listBox(string message, bool isRemove = false)
        {
            if (isRemove)
            {
                listBox_clientIP.Items.Remove(message);
            }
            else
            {
                listBox_clientIP.Items.Add(message);
            }
        }


        private void HandleClientConnected(ServerRecivedArgs e)
        {
            if (listBox_clientIP.InvokeRequired || txtBox_messageList.InvokeRequired)
            {
                var action = new addStringCallBack(AddRemove_listBox);
                action.BeginInvoke(e.IP,false, EndAsync, action);

                action = new addStringCallBack(AddStringTo_messageList);
                action.BeginInvoke($"{e.IP} has connected", false, EndAsync, action);

            }
            else
            {
                listBox_clientIP.Items.Add(e.IP);
                txtBox_messageList.Text += $"{e.IP} has connected {Environment.NewLine}";
            }
        }

        private void HandleClientDisconnected(ServerRecivedArgs e)
        {
            if (listBox_clientIP.InvokeRequired || txtBox_messageList.InvokeRequired)
            {
                var action = new addStringCallBack(AddRemove_listBox);
                action.BeginInvoke(e.IP, true, EndAsync, action);

                action = new addStringCallBack(AddStringTo_messageList);
                action.BeginInvoke($"{e.IP} has disconnected", false, EndAsync, action);

            }
            else
            {
                listBox_clientIP.Items.Remove(e.IP);
                txtBox_messageList.Text += $"{e.IP} has disconnected {Environment.NewLine}";
            }
            //listBox_clientIP.Items.Remove(e.IP);
            //txtBox_messageList.Text += $"{e.IP}:Disconnected:(( {Environment.NewLine}";
        }
        private async void HandleServerRecived(ServerRecivedArgs e)
        {
            switch (e.cmd)
            {
                case "chat":
                    txtBox_messageList.Text += $"{e.IP}:{e.cmd_details}{Environment.NewLine}";
                    break;
                case "pict":
                    await server1.imageConversion("2344.jpg", $"{e.IP}");
                    //Thread.Sleep(5000);
                    await server1.sender(e.IP, "pict");
                    break;
                case "info":
                    server1.xmlsConversion(e.cmd, e.IP);
                    //server1.sender(e.IP, "xmls");
                    break;
                case "detl":
                    server1.xmlsConversion(e.cmd, e.IP,e.cmd_details);
                    break;
                default:
                    break;
            }
        }
        private void btn_send_Click(object sender, EventArgs e)
        {
            if (listBox_clientIP.SelectedItem != null && !string.IsNullOrEmpty(txtBox_message.Text))
            {
                server1.sender(listBox_clientIP.SelectedItem.ToString(), txtBox_message.Text); // Gửi tin nhắn nè
                txtBox_message.Text = string.Empty;
            }
            Thread.Sleep(2000);
            server1.sender(listBox_clientIP.SelectedItem.ToString(), "chat");
        }
    }
}
