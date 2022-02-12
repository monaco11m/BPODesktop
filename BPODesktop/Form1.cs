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
        private string userIdSelected;
        private long groupIdSelected;
        public Form1()
        {
            InitializeComponent();
            ddlUser.DataSource = AppUserAutoBatchingBl.Instance.GetAutoBatchingUsers();
            ddlUser.DisplayMember="UserName";
            ddlUser.ValueMember ="Id";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            userIdSelected = (string)ddlUser.SelectedValue;
            groupIdSelected = Convert.ToInt64(ddlGroupId.SelectedValue);
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
                pbDownloading.Value = 0;
                btnDownload.Enabled = false;
                btnDownload.Text = "Downloading...";
                btnDownload.Refresh();
                pbDownloading.Visible = true;
                pbDownloading.Refresh();

                bool result = await DownloadAsync(saveFileDialog.FileName);
                if (result)
                    MessageBox.Show("Success");
                else
                    MessageBox.Show("Error");

                btnDownload.Enabled = true;
                pbDownloading.Visible = false;
                btnDownload.Text = "Download";
                lblMessage.Text = "";
                lblMessage.Refresh();
            }
        }
        private async Task<bool> DownloadAsync(string ZipFileName)
        {
            try
            {
                string path = Path.GetTempPath();
                List<AutoBatchingUrl> list = AutoBatchingUrlBl.Instance.GetUrlListAutoBatching(userIdSelected, groupIdSelected);
                lblMessage.Text = "0/"+list.Count;
                lblMessage.Refresh();
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
                        string mergedFileName = path + autoBatchingUrl.LabelFileName;
                        string summaryFileName = string.Format("summary_{0}.pdf", autoBatchingUrl.BatchNumber);

                        if (String.IsNullOrEmpty(autoBatchingUrl.SummaryFileName))
                        {
                            await AutoBatchingUrlBl.Instance.DownloadListAsync(autoBatchingUrl.UrlList, path);
                            
                            PdfHelper.Instance.FileFromBase64(autoBatchingUrl.SummaryPage, path + summaryFileName);
                            autoBatchingUrl.UrlList.Add("add/" + summaryFileName);
                            
                            LabelStorageUrlsBl.Instance.MergePdf(autoBatchingUrl.UrlList, mergedFileName, true);
                        }
                        else
                        {
                            BatchLabelsText batchLabelsTextBl = BatchLabelsTextBl.Instance.GetBatchLabelsText(userIdSelected, autoBatchingUrl.BatchNumber, autoBatchingUrl.AutoBatchDto.BatchNotes);
                            if (batchLabelsTextBl != null)
                            {
                                summaryFileName = path + autoBatchingUrl.SummaryFileName;
                                File.WriteAllText(mergedFileName, batchLabelsTextBl.Label);
                                File.WriteAllText(summaryFileName, batchLabelsTextBl.Summary);
                                filesToZip.Add(summaryFileName);
                            }
                        }
                        filesToZip.Add(mergedFileName);



                        pbDownloading.Invoke(new Action(() =>
                        {
                            pbDownloading.Value += 1;
                        }));

                        lblMessage.Invoke(new Action(() =>
                        {
                            lblMessage.Text = pbDownloading.Value + "/" + list.Count;
                            lblMessage.Refresh();
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
