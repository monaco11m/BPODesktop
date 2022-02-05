using Ionic.Zip;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace BPOBackend
{
    public class AutoBatchingUrlBl
    {
        private static AutoBatchingUrlBl instance = null;
        public static AutoBatchingUrlBl Instance
        {
            get
            {
                return instance ?? new AutoBatchingUrlBl();
            }
        }
        public List<AutoBatchingUrl> GetUrlListAutoBatching(string userId, long groupId)
        {
            try
            {
                string webResult = WebRequestHandler.ApiGet(SettingsHandler.Instance.GetFromAppSetting("MainDomain") + "getUrlListAutoBatching?"+
                    string.Format("userId={0}&groupId={1}", userId, groupId));

                AutoBatchingUrlResponse response= JsonConvert.DeserializeObject<AutoBatchingUrlResponse>(webResult);
                return response != null ? response.AutoBatchingUrl : new List<AutoBatchingUrl>();
            }
            catch (Exception ex)
            {
                return new List<AutoBatchingUrl>();
            }
        }
        public async Task<bool> DownloadZip(string zipFilename, string userId, long groupId)
        {
            bool result;
            try 
            {
                string path = Path.GetTempPath();
                List<AutoBatchingUrl> list = GetUrlListAutoBatching(userId, groupId);
                List<string> filesToZip = await PreparePdfsToZip(list, path);

                using (ZipFile zip = new ZipFile())
                {
                    zip.AddFiles(filesToZip, false, "");
                    zip.Save(zipFilename);
                    LabelStorageUrlsBl.Instance.DeleteFiles(filesToZip);
                }
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
        private async Task<List<string>> PreparePdfsToZip(List<AutoBatchingUrl> list, string path)
        {
            List<string> filesToZip = new List<string>();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var block = new ActionBlock<AutoBatchingUrl>(async autoBatchingUrl =>
                    {
                        await DownloadListAsync(autoBatchingUrl.UrlList, path);
                        string mergedFileName = path + autoBatchingUrl.LabelFileName;
                        string summaryFileName = string.Format("summary_{0}.pdf", autoBatchingUrl.BatchNumber);
                        PdfHelper.Instance.FileFromBase64(autoBatchingUrl.SummaryPage, path+ summaryFileName);
                        autoBatchingUrl.UrlList.Add("add/"+summaryFileName);
                        filesToZip.Add(mergedFileName);
                        LabelStorageUrlsBl.Instance.MergePdf(autoBatchingUrl.UrlList, mergedFileName, true);

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
        private async Task DownloadListAsync(List<string> list, string path)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var block = new ActionBlock<string>(async label =>
                    {
                        await DownloadHelper.DownloadFileAsync(httpClient, path, label);
                        if (!label.GetFileFormat().Equals("pdf"))
                        {
                            string[] splitedFileUrl = label.Split('/');
                            string[] splitedFileName = splitedFileUrl.LastOrDefault().Split('.');

                            PdfHelper.Instance.SaveImageAsPdf(path + splitedFileUrl.LastOrDefault(), path + splitedFileName[0] + ".pdf");
                        }
                    }, new ExecutionDataflowBlockOptions()
                    {
                        MaxDegreeOfParallelism = 10
                    });
                    foreach (string label in list)
                    {
                        await block.SendAsync(label);
                    }
                    block.Complete();
                    await block.Completion;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
