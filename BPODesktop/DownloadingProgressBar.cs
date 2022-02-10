using BPOBackend;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows.Forms;

namespace BPODesktop
{
    public partial class DownloadingProgressBar : Form
    {
        private string UserId;
        private long GroupId;
        private string ZipFileName;
        public DownloadingProgressBar(string zipFilename,string userId,long groupId)
        {
            InitializeComponent();
            UserId = userId;
            GroupId=groupId;
            ZipFileName = zipFilename;
            DownloadAsync();
        }
        private async Task DownloadAsync()
        {
            bool result;
            try
            {
                string path = Path.GetTempPath();
                List<AutoBatchingUrl> list = AutoBatchingUrlBl.Instance.GetUrlListAutoBatching(UserId, GroupId);
                List<string> filesToZip = await PreparePdfsToZip(list, path);

                using (ZipFile zip = new ZipFile())
                {
                    zip.AddFiles(filesToZip, false, "");
                    zip.Save(ZipFileName);
                    LabelStorageUrlsBl.Instance.DeleteFiles(filesToZip);
                }
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            //return result;
        }

        public async Task<List<string>> PreparePdfsToZip(List<AutoBatchingUrl> list, string path)
        {
            List<string> filesToZip = new List<string>();
            try
            {
                pbDownloading.Maximum=list.Count;

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

        private async Task<bool> DownloadZip()
        {
            try
            {
                return await AutoBatchingUrlBl.Instance.DownloadZip(ZipFileName, UserId, GroupId);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
