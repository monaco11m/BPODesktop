using BPOBackend;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
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
            ShowDialogToSaveFileAsync();
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
        private async Task ShowDialogToSaveFileAsync()
        {
            saveFileDialog.FileName = "Label_Group_" + ddlGroupId.SelectedValue.ToString() + ".zip";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                btnDownload.Enabled = false;
                pbDownloading.Visible = true;
                btnDownload.Text = "Downloading...";


                bool result = await DownloadAsync(saveFileDialog.FileName);
                if (result)
                    MessageBox.Show("Success");
                else
                    MessageBox.Show("Error");

                btnDownload.Enabled = true;
                pbDownloading.Visible = false;
                btnDownload.Text = "Download";
            }
        }
        private async Task<bool> DownloadAsync(string ZipFileName)
        {
            try
            {
                string path = Path.GetTempPath();
                List<AutoBatchingUrl> list = AutoBatchingUrlBl.Instance.GetUrlListAutoBatching((string)ddlUser.SelectedValue, Convert.ToInt64(ddlGroupId.SelectedValue));
                List<string> filesToZip = await PreparePdfsToZip(list, path);

                using (ZipFile zip = new ZipFile())
                {
                    zip.AddFiles(filesToZip, false, "");
                    zip.Save(ZipFileName);
                    LabelStorageUrlsBl.Instance.DeleteFiles(filesToZip);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            
            return true;
        }
        public async Task<List<string>> PreparePdfsToZip(List<AutoBatchingUrl> list, string path)
        {
            List<string> filesToZip = new List<string>();
            try
            {
                pbDownloading.Maximum = list.Count;

                using (var httpClient = new HttpClient())
                {
                    var block = new ActionBlock<AutoBatchingUrl>(async autoBatchingUrl =>
                    {
                        await AutoBatchingUrlBl.Instance.DownloadListAsync(autoBatchingUrl.UrlList, path);
                        string mergedFileName = path + autoBatchingUrl.LabelFileName;
                        string summaryFileName = string.Format("summary_{0}.pdf", autoBatchingUrl.BatchNumber);
                        PdfHelper.Instance.FileFromBase64(autoBatchingUrl.SummaryPage, path + summaryFileName);
                        autoBatchingUrl.UrlList.Add("add/" + summaryFileName);
                        filesToZip.Add(mergedFileName);
                        LabelStorageUrlsBl.Instance.MergePdf(autoBatchingUrl.UrlList, mergedFileName, true);

                        pbDownloading.Invoke(new Action(() =>
                        {
                            pbDownloading.Value += 1;
                        }));


                    }, new ExecutionDataflowBlockOptions()
                    {
                        MaxDegreeOfParallelism = 10
                    });
                    foreach (AutoBatchingUrl autoBatchingUrl in list)
                    {
                        await block.SendAsync(autoBatchingUrl);
                    }
                    block.Complete();
                    await block.Completion;
                }
            }
            catch (Exception ex)
            {
                filesToZip = new List<string>();
            }
            return filesToZip;
        }

    }
}
