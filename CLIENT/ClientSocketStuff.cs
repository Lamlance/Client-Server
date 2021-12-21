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
        public string cmd { get; set; }
        public byte[] byteBuffer { get; set; }
        public MemoryStream sw { get; set; }
        public ClientRecivedArgs() 
        {
            sw = new MemoryStream();
        }
        public ClientRecivedArgs(ClientRecivedArgs obj)
        {
            sw = new MemoryStream();
            obj.sw.Seek(0, SeekOrigin.Begin);
            obj.sw.CopyTo(sw);
            cmd = obj.cmd;
        }
        public void Reset()
        {
            sw = new MemoryStream();
            sw.Seek(0, SeekOrigin.Begin);
        }

    }
    class ClientSocketStuff
    {
        public delegate void ClientReciveddEventHandlder(ClientRecivedArgs e);
        public static event ClientReciveddEventHandlder ClientRecivedEvent;
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        private static ClientRecivedArgs clientRecivedArgsGOBAL = new ClientRecivedArgs();
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
                _clientSocket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), clientRecivedArgsGOBAL);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private static async Task<ClientRecivedArgs> CopyGlobalArgument()
        {
            ClientRecivedArgs copy = new ClientRecivedArgs(clientRecivedArgsGOBAL);
            //await Task.Delay(3000);
            return copy;
        }

        private static void EndAsyncEvent(IAsyncResult iar)
        {
           //var ar = (System.Runtime.Remoting.Messaging.AsyncResult)iar;
           //var invokedMethod = (ClientReciveddEventHandlder)iar.AsyncState;

            try
            {
                clientRecivedArgsGOBAL.Reset();
                ClientRecivedEvent.EndInvoke(iar);
            }
            catch
            {
                // Handle any exceptions that were thrown by the invoked method
                MessageBox.Show("An event listener went kaboom!");
            }
        }

        private async void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                int bytesRead = _clientSocket.EndReceive(ar);
                //ClientRecivedArgs clientRecivedArgs = (ClientRecivedArgs)ar.AsyncState;
                clientRecivedArgsGOBAL.byteBuffer = new byte[bytesRead]; 
                Array.Copy(buffer, clientRecivedArgsGOBAL.byteBuffer, bytesRead);

                Console.WriteLine($"{bytesRead}");
                if(bytesRead==4)
                {
                    string maybeProtocol = Encoding.ASCII.GetString(clientRecivedArgsGOBAL.byteBuffer);
                    if (maybeProtocol.Equals("pict") == true 
                        || maybeProtocol.Equals("chat") == true 
                        || maybeProtocol.Equals("xmls") == true
                        || maybeProtocol.Equals("done") == true
                        )
                    {
                        clientRecivedArgsGOBAL.cmd = maybeProtocol;
                        var CopyTask = CopyGlobalArgument();
                        var CopyResult = await CopyTask;

                        ClientRecivedEvent.BeginInvoke(CopyResult,new AsyncCallback(EndAsyncEvent), null);
                        //ClientRecivedEvent.Invoke(CopyResult);
                        //clientRecivedArgsGOBAL.Reset();

                    }
                    else
                    {
                        clientRecivedArgsGOBAL.sw.Write(clientRecivedArgsGOBAL.byteBuffer, 0, clientRecivedArgsGOBAL.byteBuffer.Length);
                    }
                }
                else if(bytesRead > 0)
                {
                    clientRecivedArgsGOBAL.sw.Write(clientRecivedArgsGOBAL.byteBuffer, 0, clientRecivedArgsGOBAL.byteBuffer.Length);
                }
                _clientSocket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), null);

            }
            catch (Exception e) 
            {
                Console.WriteLine(e.Message);
            }
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
