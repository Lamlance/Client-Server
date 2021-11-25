using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;

namespace SERVER
{
   
    public class ServerSocketStuff
    {
        private static ListBox ipListBox;
        private static TextBox serverMessageBox;
        private static Socket _serverSocket;
        private static byte[] _byteBuffer;

        private static Dictionary<string,Socket> _clientDictionary =new Dictionary<string, Socket>();

        public ServerSocketStuff(ref Socket server,ref ListBox ipList,ref TextBox MessBox)
        {
            //_clietnSocket = new List<Socket>();
            _serverSocket = server;
            _byteBuffer = new byte[1024];//1KB
            ipListBox = ipList;
            serverMessageBox = MessBox;
        }

        public void sender(string clientAddress,string message)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(message);
            Socket socket = _clientDictionary[clientAddress];
            socket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None,new AsyncCallback(SendCallback), socket);
        }
        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;
                // Complete sending the data to the remote device.  
                int bytesSent = client.EndSend(ar);
                Console.WriteLine($"Sent {bytesSent} bytes to client.");
            }

            catch (Exception) { }
        }
        public void starter()
        {
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, 9000));
            _serverSocket.Listen(5);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), null);
        }

        private static void addTo_textBox(string str)
        {
            if (serverMessageBox.InvokeRequired)
            {
                Action safeWrite = delegate { addTo_textBox(str); };
                serverMessageBox.Invoke(safeWrite);
            }
            else
            {
                serverMessageBox.Text += $"{str}{Environment.NewLine}";
            }
        }
        private static void addTo_listBox(string str)
        {
            if (ipListBox.InvokeRequired)
            {
                Action safeWrite = delegate { addTo_listBox(str); };
                ipListBox.Invoke(safeWrite);
            }
            else
            {
                ipListBox.Items.Add(str);
            }
        }
        
        public static void AcceptCallBack(IAsyncResult async)
        {
            Socket socket = _serverSocket.EndAccept(async);

            string clietnIP = $"{((IPEndPoint)(socket.RemoteEndPoint)).Address}" +
                              $":{((IPEndPoint)(socket.RemoteEndPoint)).Port}";

            var threadParameters = new System.Threading.ThreadStart(delegate { addTo_listBox(clietnIP); });
            var thread2 = new System.Threading.Thread(threadParameters);
            thread2.Start();

            _clientDictionary.Add(clietnIP, socket);
            socket.BeginReceive(_byteBuffer, 0, _byteBuffer.Length, SocketFlags.None, new AsyncCallback(RecivedComand), socket);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), null);
        }

        //public static void RecivedComand(IAsyncResult async)
        //{

        //    Socket socket = (Socket)async.AsyncState;
        //    string clietnIP = $"{((IPEndPoint)(socket.RemoteEndPoint)).Address}" +
        //                      $":{((IPEndPoint)(socket.RemoteEndPoint)).Port}";
        //    int recived = socket.EndReceive(async);
        //    byte[] dataBuf = new byte[recived];
        //    Array.Copy(_byteBuffer, dataBuf, recived);

        //    string cmd_str = Encoding.ASCII.GetString(dataBuf);

        //    if (cmd_str.Substring(0, 4).Equals("chat") == true)
        //    {
        //        string message = $"{clietnIP}:{cmd_str.Substring(5)} ";
        //        var threadParameters = new System.Threading.ThreadStart(delegate { addTo_textBox(message); });
        //        var thread2 = new System.Threading.Thread(threadParameters);
        //        thread2.Start();
        //    }
        //    _clientDictionary[clietnIP].BeginReceive(_byteBuffer, 0, _byteBuffer.Length, SocketFlags.None, new AsyncCallback(RecivedComand), _clientDictionary[clietnIP]);
        //}
        public static bool SocketConnected(Socket s)
        {
            bool part1 = s.Poll(1000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            if (part1 & part2)
                return false;
            else
                return true;
        }
        public static void RemoveTo_listBox(string text)
        {
            if (ipListBox.InvokeRequired)
            {
                // Call this same method 
                Action safeWrite = delegate { RemoveTo_listBox($"{text}"); };
                ipListBox.Invoke(safeWrite);
            }
            else
                ipListBox.Items.Remove(text);
        }
        public static void RecivedComand(IAsyncResult async)
        {

            Socket socket = (Socket)async.AsyncState;
            string clietnIP = $"{((IPEndPoint)(socket.RemoteEndPoint)).Address}" +
                              $":{((IPEndPoint)(socket.RemoteEndPoint)).Port}";
            //Socket sock = (Socket)async.AsyncState;
            if (SocketConnected(socket) == false)
            {
                
                _clientDictionary.Remove(clietnIP);
                RemoveTo_listBox(clietnIP);
                return;
            }
            else
            {
                int recived = socket.EndReceive(async);
                byte[] dataBuf = new byte[recived];
                Array.Copy(_byteBuffer, dataBuf, recived);

                string cmd_str = Encoding.ASCII.GetString(dataBuf);

                if (cmd_str.Substring(0, 4).Equals("chat") == true)
                {
                    string message = $"{clietnIP}:{cmd_str.Substring(5)} ";
                    var threadParameters = new System.Threading.ThreadStart(delegate { addTo_textBox(message); });
                    var thread2 = new System.Threading.Thread(threadParameters);
                    thread2.Start();
                }

                _clientDictionary[clietnIP].BeginReceive(_byteBuffer, 0, _byteBuffer.Length, SocketFlags.None, new AsyncCallback(RecivedComand), _clientDictionary[clietnIP]);
            }
        }

    }
}
