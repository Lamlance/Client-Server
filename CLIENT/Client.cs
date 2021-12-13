using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;

namespace CLIENT
{
    public partial class ClientApp : Form
    {
        public ClientApp()
        {
            InitializeComponent();
        }
        private ClientSocketStuff client;
        private static Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static bool getInfo = true;
        private void btn_connect_Click(object sender, EventArgs e)
        {
            btn_sendMess.Enabled = true;
            client = new ClientSocketStuff(ref _clientSocket);// int &a
            int foundIP = txtBox_ipBox.Text.IndexOf(":");
            //string IP = txtBox_ipBox.Text.Substring(0, foundIP);
            //string port = txtBox_ipBox.Text.Substring(foundIP+1);
            if (client.Connect("POI", "POI"))
            {
                MessageBox.Show("YEAH");
            }
            else
            {
                MessageBox.Show("NO");
            }
        }

        private delegate void addString_toLogCallBack(string message);

        private void EndAsync(IAsyncResult async)
        {
            addString_toLogCallBack callback = (addString_toLogCallBack)async.AsyncState;
            callback.EndInvoke(async);
        }

        private void addString_toLog(string message)
        {
            textBox_log.Text += $"{message}{Environment.NewLine}";
        }
        private void addTxt_toLog(string message)
        {
            if (textBox_log.InvokeRequired)
            {
                var action = new addString_toLogCallBack(addString_toLog);
                action.BeginInvoke(message, EndAsync, action);
            }
            else
            {
                addString_toLog(message);
            }
        }

        private void ClientRecived_image(ClientRecivedArgs e)
        {
            e.sw.Seek(0, SeekOrigin.Begin);

            Directory.CreateDirectory("SavedAvatar");

            byte[] nameByteArr = new byte[53];
            e.sw.Read(nameByteArr, 0, 52);
            string imageName = Encoding.ASCII.GetString(nameByteArr);

            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            //textBox_log.Text += $"Image saved: SavedAvatar\\{imageName} {Environment.NewLine}";
            foreach (char c in invalid)
            {
                imageName = imageName.Replace(c.ToString(), "");
            }
            addTxt_toLog($"Image saved: SavedAvatar\\{imageName}");
            try
            {
                e.sw.Seek(0, SeekOrigin.Begin);
                using (Stream file = File.Create(@"SavedAvatar\" + imageName))
                {
                    e.sw.Seek(52, SeekOrigin.Begin);
                    e.sw.CopyTo(file);
                }
            }
            catch (Exception exception)
            {
                //textBox_log.Text += $"{exception.Message} {Environment.NewLine}";
            }
            using (Stream stream = File.OpenRead(@"SavedAvatar\" + imageName))
            {
                Image avatar = Image.FromStream(stream);
                pictureBox_avatar.Image = avatar;
            }
        }

        private void ClientRecived_xmls (ClientRecivedArgs e)
        {
            //textBox_log.Text += $"Da nhan dc xmls {Environment.NewLine}";
            addTxt_toLog($"Da nhan dc xmls");
            e.sw.Seek(0, SeekOrigin.Begin);
            string fileName = "info.xml";
            if (getInfo)
            {
                fileName = "info.xml";
            }
            else
            {
                fileName = "detail.xml";
            }
            using (Stream file = File.Create(fileName))
            {
                file.Seek(0, SeekOrigin.Begin);
                e.sw.CopyTo(file);
                var threadParameters = new System.Threading.ThreadStart(delegate { SetDataGridFromXML_wSchema(fileName, getInfo); });
                var thread2 = new System.Threading.Thread(threadParameters);
                thread2.Start();
                //SetDataGridFromXML_wSchema("info.xml", getInfo);
            }
        }



        public void SetDataGridFromXML_wSchema(string xmlFilePath, bool isInfo = false)
        {
            if (dataGridView_detail.InvokeRequired || dataGridView_info.InvokeRequired)
            {
                if (isInfo)
                {
                    dataGridView_info.Invoke((MethodInvoker)delegate {
                        SetDataGridFromXML_wSchema(xmlFilePath, isInfo);
                    });
                }
                else
                {
                    dataGridView_detail.Invoke((MethodInvoker)delegate {
                        SetDataGridFromXML_wSchema(xmlFilePath, isInfo);
                    });
                }

            }
            else
            {
                try
                {


                    if (isInfo)
                    {
                        dataGridView_info.DataSource = null;
                        DataSet infoTable = new DataSet("InfoTable");
                        infoTable.ReadXml(xmlFilePath);
                        dataGridView_info.AutoGenerateColumns = true;
                        dataGridView_info.DataSource = infoTable;
                        dataGridView_info.DataMember = "PhoneBookTable";
                        dataGridView_info.Columns["AvatarPath"].Visible = false;
                        dataGridView_info.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
                    else
                    {
                        dataGridView_detail.DataSource = null;
                        DataSet DetailTable = new DataSet("DetailTable");
                        DetailTable.ReadXml(xmlFilePath);
                        dataGridView_detail.AutoGenerateColumns = true;
                        dataGridView_detail.DataSource = DetailTable;
                        dataGridView_detail.DataMember = "PhoneBookTable";
                        dataGridView_detail.Columns["AvatarPath"].Visible = false;
                        dataGridView_detail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }


                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    //textBox_log.Text += $"{e.Message}";
                }
            }

        }
        private void HandleClientRecived(ClientRecivedArgs e)
        {
            switch (e.cmd)
            {
                case "chat":
                    e.sw.Seek(0, SeekOrigin.Begin);
                    using (StreamReader reader = new StreamReader(e.sw))
                    {
                        addTxt_toLog($"Server: {reader.ReadToEnd()}");
                        //txtBox_serverChat.Text += $"Server {reader.ReadToEnd()}";
                    }
                    break;
                case "pict":
                    ClientRecived_image(e);
                    break;
                case "xmls":
                    ClientRecived_xmls(e);
                    break;
                case "done":
                    break;
                default:
                    break;
            }
        }

        private void ClientApp_Load(object sender, EventArgs e)
        {
            ClientSocketStuff.ClientRecivedEvent += HandleClientRecived;
            btn_sendMess.Enabled = false;
        }

        private void btn_sendMess_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtBox_message.Text))
            {
                if (txtBox_message.Text.Equals("pict"))
                {
                    string message = "pict*abc";
                    client.sendMessage(message);
                    txtBox_message.Text = string.Empty;
                }
                else if (txtBox_message.Text.Equals("info"))
                {
                    string message = "info*abc";
                    getInfo = true;
                    client.sendMessage(message);
                    txtBox_message.Text = string.Empty;
                }
                else if(txtBox_message.Text.Equals("detl"))
                {
                    string message = "detl*1995";
                    getInfo = true;
                    client.sendMessage(message);
                    txtBox_message.Text = string.Empty;
                }
                else
                {
                    string message = "chat*" + txtBox_message.Text;
                    client.sendMessage(message);
                    txtBox_message.Text = string.Empty;
                }

            }
        }

        private void dataGridView_info_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            string serial =  dataGridView_info.Rows[row].Cells[0].Value.ToString() ;
            textBox_log.Text += $"Selected: {serial} {Environment.NewLine}";
            string message = $"detl*{serial}";
            getInfo = false;
            client.sendMessage(message);
        }
    }
}

/*
        using (Stream file = File.Create(@"info.xml", unchecked((int)e.sw.Length),FileOptions.DeleteOnClose))
                {
                    file.Seek(0, SeekOrigin.Begin);
                    e.sw.CopyTo(file);
                    file.Close();
                }
        using (Stream file = File.Create(@"avatar.jpg",1024,FileOptions.DeleteOnClose))
                {
                    file.Seek(0, SeekOrigin.Begin);
                    e.sw.CopyTo(file);
                    file.Close();
                }
         */