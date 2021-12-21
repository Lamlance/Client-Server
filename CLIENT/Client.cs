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
        private int TaskLeft = 0;

        private void setAllButton(bool isEnable = true)
        {
            button_detl.Enabled = isEnable;
            btn_sendMess.Enabled = isEnable;
            button_info.Enabled = isEnable;
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            client = new ClientSocketStuff(ref _clientSocket);// int &a
            int foundIP = txtBox_ipBox.Text.IndexOf(":");
            //string IP = txtBox_ipBox.Text.Substring(0, foundIP);
            //string port = txtBox_ipBox.Text.Substring(foundIP+1);
            if (client.Connect("POI", "POI"))
            {
                setAllButton(true);
                txtBox_serverChat.Text += $"Server connected {Environment.NewLine}";
            }
            else
            {
                txtBox_serverChat.Text += $"Failed to connect to server {Environment.NewLine}";
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

        private async Task<bool> ClientRecived_image(ClientRecivedArgs e)
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
                TaskLeft--;
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
            return true;
        }

        private async Task<bool> ClientRecived_xmls (ClientRecivedArgs e)
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
                //SetDataGridFromXML_wSchema(fileName, getInfo);
                var threadParameters = new System.Threading.ThreadStart(delegate { SetDataGridFromXML_wSchema(fileName, getInfo); });
                var thread2 = new System.Threading.Thread(threadParameters);
                thread2.Start();
                //SetDataGridFromXML_wSchema("info.xml", getInfo);
            }
            return true;
        }

        private int CountAvatarList( DataTable dt)
        {
            List<string> s = dt.AsEnumerable().Select(x => x["AvatarPath"].ToString()).Distinct().ToList();
            TaskLeft = s.Count;
            return s.Count;
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
                        CountAvatarList( infoTable.Tables["PhoneBookTable"] );
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
                        CountAvatarList(DetailTable.Tables["PhoneBookTable"]);
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
        private async void HandleClientRecived(ClientRecivedArgs e)
        {
            setAllButton(false);
            switch (e.cmd)
            {
                case "chat":
                    e.sw.Seek(0, SeekOrigin.Begin);
                    using (StreamReader reader = new StreamReader(e.sw))
                    {
                        //addTxt_toLog($"Server: {reader.ReadToEnd()}");
                        txtBox_serverChat.Text += $"Server:{reader.ReadToEnd()} {Environment.NewLine}";
                    }
                    break;
                case "pict":
                    await ClientRecived_image(e);
                    break;
                case "xmls":
                    TaskLeft = 1;
                    await ClientRecived_xmls(e);
                    break;
                case "done":
                    break;
                default:
                    break;
            }

            if (TaskLeft <= 0)
            {
                setAllButton(true);
            }
        }

        private void ClientApp_Load(object sender, EventArgs e)
        {
            ClientSocketStuff.ClientRecivedEvent += HandleClientRecived;
            setAllButton(false);
        }

        private void btn_sendMess_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtBox_message.Text))
            {
                string message = $"chat*{txtBox_message.Text}";
                client.sendMessage(message);
                txtBox_message.Text = string.Empty;
                txtBox_serverChat.Text += $"Message send! {Environment.NewLine}";
                setAllButton(false);

            }
        }

        private void dataGridView_info_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //int row = e.RowIndex;
            //string serial =  dataGridView_info.Rows[row].Cells[0].Value.ToString() ;
            //textBox_log.Text += $"Selected: {serial} {Environment.NewLine}";
            //setAllButton(false);
            //string message = $"detl*{serial}";
            //getInfo = false;
            //client.sendMessage(message);
        }

        private void button_info_Click(object sender, EventArgs e)
        {
            string message = "info*abc";
            getInfo = true;
            client.sendMessage(message);
            txtBox_message.Text = string.Empty;
            txtBox_serverChat.Text += $"Request send! {Environment.NewLine}";
            setAllButton(false);
        }

        private void button_detl_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtBox_message.Text))
            {

                string message = $"detl*{txtBox_message.Text}";
                getInfo = false;
                client.sendMessage(message);
                txtBox_message.Text = string.Empty;
                txtBox_serverChat.Text += $"Request send! {Environment.NewLine}";
                setAllButton(false);
            }
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