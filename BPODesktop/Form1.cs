using BPOBackend;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BPODesktop
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ddlUser.DataSource = AppUserAutoBatchingBl.Instance.GetAutoBatchingUsers();
            ddlUser.DisplayMember="UserName";
            ddlUser.ValueMember ="Id";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            ShowDialogToSaveFile();
        }

        private void DownloadingProgressBar_FormClosed(object sender, FormClosedEventArgs e)
        {
            btnDownload.Enabled = true;
        }

        private async Task<bool> DownloadZip(string fileName)
        {
            try
            {
                return await AutoBatchingUrlBl.Instance.DownloadZip(fileName, (string)ddlUser.SelectedValue, Convert.ToInt64(ddlGroupId.SelectedValue));
            }
            catch(Exception ex)
            {
                return false;
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
            List<AutomateLabel> result = AutomateLabelsBl.Instance.GetAutomateLabel((string)ddlUser.SelectedValue, dtStartDate.Value, dtEndDate.Value);
            ddlGroupId.DataSource = result;
            ddlGroupId.DisplayMember = "AutomateId";
            ddlGroupId.ValueMember = "AutomateId";
        }
        private void ShowDialogToSaveFile()
        {
            saveFileDialog.FileName = "Label_Group_" + ddlGroupId.SelectedValue.ToString() + ".zip";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                btnDownload.Enabled = false;
                //btnDownload.Text = "Downloading...";
                //bool result=await DownloadZip(saveFileDialog.FileName);

                DownloadingProgressBar downloadingProgressBar = new DownloadingProgressBar(saveFileDialog.FileName, (string)ddlUser.SelectedValue, Convert.ToInt64(ddlGroupId.SelectedValue));
                downloadingProgressBar.FormClosed += DownloadingProgressBar_FormClosed;
                downloadingProgressBar.Show();

                //if (result)
                //    MessageBox.Show("Success");
                //else
                //    MessageBox.Show("Error");
                //btnDownload.Enabled = true;
                //btnDownload.Text = "Download";
            }
        }

    }
}
