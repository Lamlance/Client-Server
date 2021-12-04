using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Drawing;

namespace CLIENT
{
    public class ClientRecivedArgs
    {
        public string tmd { get; }
        public StringBuilder sb_buffer { get; }
        public byte[] byteBuffer { get; set; }
        public ClientRecivedArgs() 
        {
            sb_buffer = new StringBuilder();
        }
    }
    class ClientSocketStuff
    {
        public delegate void ClientReciveddEventHandlder(ClientRecivedArgs e);
        public static event ClientReciveddEventHandlder ClientRecivedEvent;
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        private static Socket _clientSocket;
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
                _clientSocket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), new ClientRecivedArgs());
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
                ClientRecivedArgs clientRecivedArgs = (ClientRecivedArgs)ar.AsyncState;
                clientRecivedArgs.byteBuffer = new byte[bytesRead]; 
                Array.Copy(buffer, clientRecivedArgs.byteBuffer, bytesRead);
                

                Console.WriteLine($"{bytesRead}");
                if(bytesRead==4)
                {
                    string maybeProtocol = Encoding.ASCII.GetString(clientRecivedArgs.byteBuffer);
                    if (maybeProtocol.Equals("pict") == true || maybeProtocol.Equals("done") == true )
                    {
                        if (clientRecivedArgs.sb_buffer.Length > 1)
                        {
                            if (maybeProtocol.Equals("pict") == true)
                            {
                                ClientRecivedEvent?.Invoke(clientRecivedArgs);
                                allDone.Set();
                            }
                            else
                            {
                                ClientRecivedEvent?.Invoke(clientRecivedArgs);
                                allDone.Set();
                            }
                        }
                    }
                    else
                    {
                        clientRecivedArgs.sb_buffer.Append(Encoding.ASCII.GetString(clientRecivedArgs.byteBuffer));
                    }
                }
                else
                {
                    clientRecivedArgs.sb_buffer.Append(Encoding.ASCII.GetString(clientRecivedArgs.byteBuffer));

                }
                _clientSocket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), clientRecivedArgs);

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

        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
        //private  void SaveImage()
        //{
        //    Image.Save("D:\\git\\Test\\Client-Server\\Receive_img1", byteArrayToImage().Png);
        //}
    }
}
