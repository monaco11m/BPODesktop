using Microsoft.Extensions.Configuration;
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

        public List<String> GetUrls()
        {
            return LabelStorageUrlsDao.Instance.GetUrls();
        }
        public List<LabelStorageUrl> GetUrlsByParameters(String userId, Int32 groupId)
        {
            return LabelStorageUrlsDao.Instance.GetUrlsByParameters(userId,groupId);
        }
        public async Task DownloadListAsync(List<LabelStorageUrl> list, String path)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var block = new ActionBlock<LabelStorageUrl>(async label =>
                    {
                        await DownloadHelper.DownloadFileAsync(httpClient, path, label.Url);
                        if (label.Format.Equals("PNG")) {
                            String[] splitedFileUrl = label.Url.Split('/');
                            String[] splitedFileName = splitedFileUrl[splitedFileUrl.Length-1].Split('.');

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
        public void MergePdf(List<String> list,String fileName,Boolean splitPdfName=false)
        {
            try
            {
                String[] fileNameSplited = fileName.Split('\\');
                String onlyName = fileNameSplited[fileNameSplited.Length - 1];
                String path = fileName.Substring(0, fileName.Length - onlyName.Length);
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                PdfDocument outputDocument = new PdfDocument
                {
                    PageLayout = PdfPageLayout.SinglePage
                };

                foreach (String pdf in list)
                {
                    try
                    {
                        String pdfName = pdf;
                        if (splitPdfName)
                        {
                            String[] pdfNameSplited = pdf.Split('/');
                            pdfName = pdfNameSplited[pdfNameSplited.Length - 1];
                        }
                        //String[] splitedPdfName = pdfName.Split('.');
                        //if (!splitedPdfName[splitedPdfName.Length-1].Equals("pdf")&& !splitedPdfName[splitedPdfName.Length - 1].Equals("PDF"))
                        //{
                        //    PdfHelper.Instance.SaveImageAsPdf(path + pdfName, path + splitedPdfName[0]+".pdf");
                        //    pdfName = splitedPdfName[0] + ".pdf";
                        //}

                        PdfDocument inputDocument = PdfReader.Open(path + pdfName, PdfDocumentOpenMode.Import);
                        foreach (PdfPage page in inputDocument.Pages)
                        {
                            outputDocument.AddPage(page);
                        }
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
        private IConfiguration SetConfig()
        {
            return new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", false, true)
                    .Build();
        }
        public String GetPathFromAppSetting()
        {
            IConfiguration configuration = SetConfig();
            return configuration["Path"];
        }
        public void MergePdfGroupByTrackingNumber()
        {

        }
    }
}
