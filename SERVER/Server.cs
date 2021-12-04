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
            else if (e.cmd.Equals("pict") == true)
            {
                MessageBox.Show("Se gui hinh");
                server1.imageConversion(@"D:\CLASS\DaiHock\DaiHoc\Dumb\Client-Server\img1\Banana.jpg",$"{e.IP}");
                Thread.Sleep(5000);
                server1.sender(e.IP, "pict");
            }
        }
        private void btn_send_Click(object sender, EventArgs e)
        {
            if (listBox_clientIP.SelectedItem != null && !string.IsNullOrEmpty(txtBox_message.Text))
            {
                server1.sender(listBox_clientIP.SelectedItem.ToString(), txtBox_message.Text); // Gửi tin nhắn nè
                txtBox_message.Text = string.Empty;
            }
            Thread.Sleep(2000) ;// Đợi 2 giây
            server1.sender(listBox_clientIP.SelectedItem.ToString(), "done"); // Gửi done báo hiệu đã gửi xong ok ong. mà đọc file hình là mình đọc trong folder của server 
            //Đúng r ông thử gửi mấy cái hình đó đi cái hình 10k là cũng nhẹ nè thử gửi cái đó đi
            // Đọc file vào stream rồi stream sang byte[] rồi gửi
            // ok ? ok ok rồi client nó nhận nó luu vào folder của nó ông cứ lưu nó lại thì nó lưu cùng thư mục với file êxe
            // Hiểu ko ? để tui xem lm thử. Ok v nha tăt
        }

        private void btn_Pict_Click(object sender, EventArgs e)
        {
            //byte[] buffer= server1.imageConversion("D:\\git\\Test\\Client-Server\\img1\\Banana.jpg","");
            //if (listBox_clientIP.SelectedItem != null && !string.IsNullOrEmpty(txtBox_message.Text))
            //{
            //    server1.sender1(listBox_clientIP.SelectedItem.ToString(), buffer); // Gửi tin nhắn nè
            //    txtBox_message.Text = string.Empty;
            //}
            //Thread.Sleep(2000);// Đợi 2 giây
            //server1.sender(listBox_clientIP.SelectedItem.ToString(), "pict");
        }
    }
}
