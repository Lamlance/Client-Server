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
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Resources;
using System.Drawing;
using System.Collections;
using System.Globalization;

namespace SERVER
{
    public class ServerRecivedArgs
    {
        public string IP { get; set; }
        public string cmd { get; set; }
        public string cmd_details { get; set; }
        public ServerRecivedArgs(string text) { IP = text; }
        public ServerRecivedArgs(ServerRecivedArgs obj)
        {
            cmd = obj.cmd;
            IP = obj.IP;
            cmd_details = obj.cmd_details;
        }
        public void reset()
        {
            cmd = string.Empty;
            IP = string.Empty;
            cmd_details = string.Empty;

        }
    }
    public class ServerSocketStuff
    {
        public delegate void ServerRecivedEventHandlder(ServerRecivedArgs e);

        public static event ServerRecivedEventHandlder ServerRecivedEvent;
        public static event ServerRecivedEventHandlder ClientConnected;
        public static event ServerRecivedEventHandlder ClientDisconnect;

        public static ManualResetEvent manualResetEvent = new ManualResetEvent(false);

        private static Socket _serverSocket;
        private static byte[] _byteBuffer;
        string ConnectionString;
        SqlConnection connection;

        private static Dictionary<string,Socket> _clientDictionary =new Dictionary<string, Socket>();
      
        private void readPhoneBookInfo(ref DataTable PhoneBookTable,bool isDetail,string detail = "")
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName;

            if (isDetail)
            {
                resourceName = $"SERVER.SqlQuery.DetailSearch_query.sql";
            }
            else
            {
                resourceName = $"SERVER.SqlQuery.PhoneBookInfo_query.sql";
            }

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string PhoneBookInfo_query = reader.ReadToEnd();

                        using (connection = new SqlConnection(ConnectionString))
                        using (SqlDataAdapter adapter = new SqlDataAdapter(PhoneBookInfo_query+ detail, connection))
                        {
                            adapter.Fill(PhoneBookTable);
                            return;
                        }
                    }
                }
            }
            return;
        }

        public string getData(string cmd,ref DataTable PhoneBookTable,string detail = "")
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["SERVER.Properties.Settings.PhoneBookConnectionString"].ConnectionString;

            switch (cmd)
            {
                case "info":
                    readPhoneBookInfo(ref PhoneBookTable, false);
                    PhoneBookTable.WriteXml("PhoneBookInfo.xml", XmlWriteMode.WriteSchema);
                    return "PhoneBookInfo.xml";
                case "detl":
                    readPhoneBookInfo(ref PhoneBookTable, true, detail);
                    PhoneBookTable.WriteXml("Details.xml", XmlWriteMode.WriteSchema);
                    return "Details.xml";
                default:
                    break;
            }
            return String.Empty;
        }
        
        public ServerSocketStuff(ref Socket server)
        {
            //_clietnSocket = new List<Socket>();
            _serverSocket = server;
            _byteBuffer = new byte[1024];//1KB
        }

        public async Task<bool> sender(string clientAddress,string message)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(message);
            Socket socket = _clientDictionary[clientAddress];
            IAsyncResult result = socket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None,new AsyncCallback(SendCallback), socket);

            await Task.Delay(2000);

            return true;
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                int bytesSent = client.EndSend(ar);
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
            ClientConnected.Invoke(clientArgs);

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
                return;
            }else
            {
                int recived = socket.EndReceive(async);
                byte[] dataBuf = new byte[recived];
                Array.Copy(_byteBuffer, dataBuf, recived);

                string cmd_str = Encoding.ASCII.GetString(dataBuf);
                clientArgs.cmd = cmd_str.Substring(0, 4);
                clientArgs.cmd_details = cmd_str.Substring(5);

                ServerRecivedEvent?.Invoke(new ServerRecivedArgs(clientArgs));

                _clientDictionary[clientArgs.IP].BeginReceive(_byteBuffer, 0, _byteBuffer.Length, SocketFlags.None, new AsyncCallback(RecivedComand), new ServerRecivedArgs(clientArgs.IP));
            }
        }



        public async Task<bool> imageConversion(string imageName,string IP)
        {
            //Initialize a file stream to read the image file
            FileStream fs = new FileStream($"PhoneBookAvatar\\{imageName}", FileMode.Open, FileAccess.Read);

            string img = imageName;

            if (img.Length < 52)
            {
                img += new string('\0', 52 - img.Length);
            }

            byte[] byteArr = Encoding.ASCII.GetBytes(img);
            MemoryStream memory = new MemoryStream();
            memory.Write(byteArr, 0, byteArr.Length);

            fs.Seek(0, SeekOrigin.Begin);
            fs.CopyTo(memory);

            memory.Seek(0, SeekOrigin.Begin);
            byteArr = new byte[memory.Length];
            memory.Read(byteArr, 0, Convert.ToInt32(memory.Length) );

            IAsyncResult asyncResult = _clientDictionary[IP].BeginSend(byteArr, 0, byteArr.Length, SocketFlags.None, 
                new AsyncCallback(SendCallback), _clientDictionary[IP]);

            await Task.Delay(5000);

            var SendTask = await sender(IP, "pict");
            return true;
        }

        public async Task<bool> xmlsConversion(string cmd, string IP,string detail = "")
        {

            DataTable PhoneBookTable = new DataTable("PhoneBookTable");
            string xmlName = getData(cmd,ref PhoneBookTable, detail);

            List<string> AvatarNameList = new List<string>() ;
            foreach (DataRow item in PhoneBookTable.Rows)
            {
                string AvatarName = item["AvatarPath"].ToString();
                if (!String.IsNullOrEmpty(AvatarName))
                {
                    AvatarNameList.Add(item["AvatarPath"].ToString());
                }
            }

            FileStream fs = new FileStream(xmlName, FileMode.Open, FileAccess.Read);
            byte[] xmlByteArr = new byte[fs.Length];
            fs.Read(xmlByteArr, 0, Convert.ToInt32(fs.Length));
            fs.Close();

            IAsyncResult result = _clientDictionary[IP].BeginSend(xmlByteArr, 0, xmlByteArr.Length, SocketFlags.None, 
                new AsyncCallback(SendCallback), _clientDictionary[IP]);


            Thread.Sleep(5000);

            var SendTask = await sender(IP, "xmls");


            List<string> Distict_AvatarNameList = AvatarNameList.Distinct().ToList();
            foreach (string item in Distict_AvatarNameList)
            {
                var ImageTask = await imageConversion(item, IP);
            }
            return true;
        }
    }
}
