using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;

namespace CLIENT
{
    class ClientSocketStuff
    {
        static string str;
        private static Socket _clientSocket;
        private static StringBuilder sb_buffer = new StringBuilder();
        private static byte[] buffer = new byte[1024];

        public ClientSocketStuff(ref Socket clientSocket)
        {
            _clientSocket = clientSocket;
        }
        public bool Connect(string IP, string port)
        {
            try
            {
                _clientSocket.Connect(IPAddress.Loopback, 9000);
                MessageBox.Show(((IPEndPoint)(_clientSocket.RemoteEndPoint)).Address.ToString());
                _clientSocket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), sb_buffer);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        private static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                int bytesRead = _clientSocket.EndReceive(ar);
                
                byte[] dataBuf = new byte[bytesRead];
                Array.Copy(buffer, dataBuf, bytesRead);
                //string cmd_str = Encoding.ASCII.GetString(dataBuf);
                //Console.WriteLine(cmd_str);
                Console.WriteLine($"{bytesRead}");
                if (bytesRead != 4)
                {
                    //Console.WriteLine($"{bytesRead}");
                    sb_buffer.Append(Encoding.ASCII.GetString(dataBuf));
                    //Console.WriteLine($"Builder: {sb_buffer}");
                }
                else
                {
                    if (sb_buffer.Length > 1)
                    {
                        Console.WriteLine($"Builder print");
                        str = sb_buffer.ToString();
                        Console.WriteLine($"Server:{str}");
                    }
                }
                _clientSocket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), sb_buffer);

            }
            catch (Exception) { }
        }

        public void sendMessage(string data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data);
            _clientSocket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(SendCallback), null);
        }
        
        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                int bytesSent = _clientSocket.EndSend(ar);
            }
            catch (Exception) { }
        }


    }
}
