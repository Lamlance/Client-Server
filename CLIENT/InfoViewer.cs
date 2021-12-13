using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CLIENT
{
    public partial class InfoViewer : Form
    {
        public InfoViewer()
        {
            InitializeComponent();
        }

        public void SetDataGridFromXML_wSchema(string xmlFilePath)
        {
            DataSet infoTable = new DataSet("InfoTable");
            infoTable.ReadXml(xmlFilePath);
            infoTable.WriteXml(@"dataSet.xml", XmlWriteMode.WriteSchema);
            dataGridView_info.AutoGenerateColumns = true;
            dataGridView_info.DataSource = infoTable;
            dataGridView_info.DataMember = "PhoneBookTable";

            dataGridView_info.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }
        public void setAvatar(Image avatar)
        {
            pictureBox_avatar.Image = avatar;
        }
        private void dataGridView_info_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            textBox_log.Text += $"Selected: {dataGridView_info.Rows[row].Cells[0].Value}{Environment.NewLine}";
            //MessageBox.Show(dataGridView_info.Rows[row].Cells[0].Value.ToString());
        }
    }
}
