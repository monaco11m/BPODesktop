using Ionic.Zip;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace BPOBackend
{
    public class LabelStorageUrlsBl
    {
        private static LabelStorageUrlsBl instance = null;
        public static LabelStorageUrlsBl Instance
        {
            get
            {
                return instance ?? new LabelStorageUrlsBl();
            }
        }

        public List<string> GetUrls()
        {
            return LabelStorageUrlsDao.Instance.GetUrls();
        }
        public List<LabelStorageUrl> GetUrlsByParameters(string userId, int groupId, DateTime startDate)
        {
            return LabelStorageUrlsDao.Instance.GetUrlsByParameters(userId,groupId, startDate);
        }
        public async Task DownloadListAsync(List<LabelStorageUrl> list, string path)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var block = new ActionBlock<LabelStorageUrl>(async label =>
                    {
                        await DownloadHelper.DownloadFileAsync(httpClient, path, label.Url);
                        if (label.Format.Equals("PNG")) {
                            string[] splitedFileUrl = label.Url.Split('/');
                            string[] splitedFileName = splitedFileUrl[splitedFileUrl.Length-1].Split('.');

                            PdfHelper.Instance.SaveImageAsPdf(path+splitedFileUrl[splitedFileUrl.Length - 1], path + splitedFileName[0]+".pdf");
                        }
                    }, new ExecutionDataflowBlockOptions()
                    {
                        MaxDegreeOfParallelism = 10
                    });
                    foreach (LabelStorageUrl label in list)
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
        public void MergePdf(List<string> list,string fileName,bool splitPdfName=false)
        {
            try
            {
                string path = RemoveFileName(fileName);
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                PdfDocument outputDocument = new PdfDocument
                {
                    PageLayout = PdfPageLayout.SinglePage
                };

                foreach (string pdf in list)
                {
                    try
                    {
                        string pdfName = pdf;
                        if (splitPdfName)
                        {
                            string[] pdfNameSplited = pdf.Split('/');
                            pdfName = pdfNameSplited[pdfNameSplited.Length - 1];
                        }
                        string[] splitedPdfName = pdfName.Split('.');
                        if (!splitedPdfName[splitedPdfName.Length - 1].Equals("pdf") && !splitedPdfName[splitedPdfName.Length - 1].Equals("PDF"))
                        {
                            pdfName = splitedPdfName[0] + ".pdf";
                        }

                        PdfDocument inputDocument = PdfReader.Open(path + pdfName, PdfDocumentOpenMode.Import);
                        foreach (PdfPage page in inputDocument.Pages)
                        {
                            outputDocument.AddPage(page);
                        }
                        File.Delete(path + pdfName);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                outputDocument.Save(fileName);
            }
            catch(Exception ex)
            {

            }

        }
        public async Task<bool> DownloadZip(string zipFilename,string userId,int groupId,DateTime startDate)
        {
            bool result = false;
            try
            {
                List<string> filesToZip = new List<String>();
                string path = Path.GetTempPath();
                List<LabelStorageUrl> list = GetUrlsByParameters(userId, groupId, startDate);

                await DownloadListAsync(list, path);
                
                while (list != null && list.Count > 0)
                {
                    long currentBatchNumber = list[0].BatchNumber;
                    List<LabelStorageUrl> subList = list.Where(x => x.BatchNumber == currentBatchNumber).ToList();

                    string mergedFileName = path + string.Format("Label_{0}_{1}_{2}", list[0].BatchNumber, list[0].ItemSku.WithMaxLength(20), list[0].ItemQuantity) + ".pdf";
                    filesToZip.Add(mergedFileName);
                    MergePdf(subList.Select(x => x.Url).ToList(), mergedFileName, true);

                    list.RemoveAll(x => x.BatchNumber == currentBatchNumber);
                }

                using (ZipFile zip = new ZipFile())
                {
                    zip.AddFiles(filesToZip, false,"");
                    zip.Save(zipFilename);
                    DeleteFiles(filesToZip);
                }
                result = true;
            }
            catch(Exception ex)
            {
                result = false;
            }
            return result;
        }
        private string RemoveFileName(string fileNameWithPath)
        {
            string[] fileNameSplited = fileNameWithPath.Split('\\');
            string onlyName = fileNameSplited[fileNameSplited.Length - 1];
            return fileNameWithPath.Substring(0, fileNameWithPath.Length - onlyName.Length);
        }
        public void DeleteFiles(List<string> list)
        {
            foreach(string file in list)
                File.Delete(file);
        }
    }
}
