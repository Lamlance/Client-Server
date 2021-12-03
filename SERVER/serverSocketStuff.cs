using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using System.Threading;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Reflection;

namespace SERVER
{
    public class ServerRecivedArgs
    {
        public string IP { get; set; }
        public string cmd { get; set; }
        public string cmd_details { get; set; }
        public ServerRecivedArgs(string text) { IP = text; }
    }
    public class ServerSocketStuff
    {
        public delegate void ServerRecivedEventHandlder(ServerRecivedArgs e);

        public static event ServerRecivedEventHandlder ServerRecivedEvent;
        public static event ServerRecivedEventHandlder ClientDisconnect;

        public static ManualResetEvent allDone = new ManualResetEvent(false);

        private static Socket _serverSocket;
        private static byte[] _byteBuffer;
        string ConnectionString;
        SqlConnection connection;

        private static Dictionary<string,Socket> _clientDictionary =new Dictionary<string, Socket>();

        private void readPhoneBookInfo(ref DataTable PhoneBookTable)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "SERVER.SqlQuery.PhoneBookInfo_query.sql";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string PhoneBookInfo_query = reader.ReadToEnd();

                        using (connection = new SqlConnection(ConnectionString))
                        using (SqlDataAdapter adapter = new SqlDataAdapter(PhoneBookInfo_query, connection))
                        {
                            adapter.Fill(PhoneBookTable);
                            return;
                        }
                    }
                }
            }
            return;
        }

        public void getData(string cmd)
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["SERVER.Properties.Settings.PhoneBookConnectionString"].ConnectionString;

            if (cmd.Equals("info") == true)
            {
                DataTable PhoneBookTable = new DataTable("PhoneBookTable");
                readPhoneBookInfo(ref PhoneBookTable);
                PhoneBookTable.WriteXml("PhoneBookInfo.xml");
            }
        }
        
        public ServerSocketStuff(ref Socket server)
        {
            //_clietnSocket = new List<Socket>();
            _serverSocket = server;
            _byteBuffer = new byte[1024];//1KB
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


        public static void AcceptCallBack(IAsyncResult async)
        {
            Socket socket = _serverSocket.EndAccept(async);

            string clietnIP = $"{((IPEndPoint)(socket.RemoteEndPoint)).Address}" +
                              $":{((IPEndPoint)(socket.RemoteEndPoint)).Port}";

            ServerRecivedArgs clientArgs = new ServerRecivedArgs(clietnIP);
            _clientDictionary.Add(clietnIP, socket);
            socket.BeginReceive(_byteBuffer, 0, _byteBuffer.Length, SocketFlags.None, new AsyncCallback(RecivedComand), clientArgs);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), null);
        }
        private static bool SocketConnected(Socket s)
        {
            bool part1 = s.Poll(1000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            if (part1 & part2)
                return false;
            else
                return true;
        }
        public static void RecivedComand(IAsyncResult async)
        {
            ServerRecivedArgs clientArgs = (ServerRecivedArgs)async.AsyncState;
            Socket socket = _clientDictionary[clientArgs.IP];
            if (SocketConnected(socket) == false)
            {

                _clientDictionary.Remove(clientArgs.IP);
                ClientDisconnect?.Invoke(clientArgs);
                //RemoveTo_listBox(clietnIP);
                return;
            }else
            {
                int recived = socket.EndReceive(async);
                byte[] dataBuf = new byte[recived];
                Array.Copy(_byteBuffer, dataBuf, recived);

                string cmd_str = Encoding.ASCII.GetString(dataBuf);
                clientArgs.cmd = cmd_str.Substring(0, 4);
                clientArgs.cmd_details = cmd_str.Substring(5);

                ServerRecivedEvent?.Invoke(clientArgs);

                _clientDictionary[clientArgs.IP].BeginReceive(_byteBuffer, 0, _byteBuffer.Length, SocketFlags.None, new AsyncCallback(RecivedComand), clientArgs);
            }
        }
           

    }
}
