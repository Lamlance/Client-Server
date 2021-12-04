using System;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace CLIENT
{
    class ClientSocketStuff
    {
        static string str;
        private static Socket _clientSocket;
        private static StringBuilder sb_buffer = new StringBuilder();
        private static byte[] buffer = new byte[1024];
        private static TextBox clientMessageBox;
        private static Stream XMLStream = new MemoryStream();
        private static DataTable test = new DataTable();
        public ClientSocketStuff(ref Socket clientSocket, ref TextBox MessBox)
        {
            _clientSocket = clientSocket;
            clientMessageBox = MessBox;
        }

        private static void addTo_textBox(string str)
        {
            if (clientMessageBox.InvokeRequired)
            {
                Action safeWrite = delegate { addTo_textBox(str); };
                clientMessageBox.Invoke(safeWrite);
            }
            else
            {
                clientMessageBox.Text += $"{str}{Environment.NewLine}";
            }
        }
        public bool Connect(string IP, string port)
        {
            if (!_clientSocket.Connected)
            {
                try
                {
                    _clientSocket.Connect(IPAddress.Loopback, 9000);
                    MessageBox.Show(((IPEndPoint)(_clientSocket.RemoteEndPoint)).Address.ToString());
                    _clientSocket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), sb_buffer);
                }
                catch (SocketException)
                {
                    return false;
                }
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
                    //sb_buffer.Append(Encoding.ASCII.GetString(dataBuf));
                    //Console.WriteLine($"Builder: {sb_buffer}");
                    XMLStream.Write(dataBuf, 0, bytesRead);
                }
                else
                {
                    //if (sb_buffer.Length > 1)
                    //{
                    // Console.WriteLine($"Builder print");
                    // str = sb_buffer.ToString();
                    // addTo_textBox($"Server: {str}");
                    // sb_buffer.Length = 0;
                    //}
                    XMLStream.Seek(0, SeekOrigin.Begin);
                    /*using (Stream file = File.Create("test.xml"))
                    {
                        XMLStream.CopyTo(file);
                        file.Close();
                    }*/
                    using (StreamReader sr = new StreamReader(XMLStream))
                    {
                        test.ReadXml(sr);
                    }
                    XMLStream.SetLength(0);
                }
                _clientSocket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), sb_buffer);

            }
            catch (SocketException e)
            {
                Console.WriteLine("Socket Exception: {0}", e);
                addTo_textBox("Error while receiving from server, check debug log for more detail");
            }
        }

        public void sendMessage(string data)
        {
            if (_clientSocket.Connected)
            {
                byte[] byteData = Encoding.ASCII.GetBytes(data);
                _clientSocket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(SendCallback), null);
            }
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                int bytesSent = _clientSocket.EndSend(ar);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Socket Exception: {0}", e);
                addTo_textBox("Error while sending message to server, check debug log for more detail");
            }
        }


    }
}
