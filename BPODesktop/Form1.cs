using BPOBackend;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BPODesktop
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ddlUser.DataSource = AspNetUserBl.Instance.GetAspNetUser();
            ddlUser.DisplayMember="UserName";
            ddlUser.ValueMember ="Id";
        }

        private void button1_Click(Object sender, EventArgs e)
        {
            ShowDialogToSaveFile();
        }
        private async Task DownloadZip(String fileName)
        {
            try
            {
                await LabelStorageUrlsBl.Instance.DownloadZip(fileName, (String)ddlUser.SelectedValue, Convert.ToInt32(ddlGroupId.SelectedValue), dtStartDate.Value);
            }
            catch(Exception ex)
            {

            }
        }

        private void dtStartDate_ValueChanged(object sender, EventArgs e)
        {
            LoadGroupIds();
        }
        private void dtEndDate_ValueChanged(object sender, EventArgs e)
        {
            LoadGroupIds();
        }
        private void LoadGroupIds()
        {
            List<AutomateLabel> result = AutomateLabelsBl.Instance.GetIdsByUserIdAndDate((String)ddlUser.SelectedValue, dtStartDate.Value, dtEndDate.Value);
            ddlGroupId.DataSource = result;
            ddlGroupId.DisplayMember = "Id";
            ddlGroupId.ValueMember = "Id";
        }
        private void ShowDialogToSaveFile()
        {
            if(saveFileDialog.ShowDialog()== DialogResult.OK)
            {
                DownloadZip(saveFileDialog.FileName);
            }
        }
        
    }
}
