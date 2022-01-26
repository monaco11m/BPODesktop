using Microsoft.Extensions.Configuration;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
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
        public async Task DownloadFileAsync(HttpClient httpClient, String link)
        {
            try
            {
                String[] splitedLink = link.Split('/');
                //var fileName = Guid.NewGuid().ToString(); // code to generate unique fileName;
                String filePath = Path.Combine("D:\\cj\\Elmo\\downloads", splitedLink[splitedLink.Length - 1]);
                if (File.Exists(filePath)) return;
                var response = await httpClient.GetAsync(link);
                response.EnsureSuccessStatusCode();
                using (var contentStream = await response.Content.ReadAsStreamAsync())
                using (var fileStream = new FileStream(filePath, FileMode.Create,
                    FileAccess.Write, FileShare.None, 32768, FileOptions.Asynchronous))
                {
                    await contentStream.CopyToAsync(fileStream);
                }
            }
            catch(Exception ex)
            {

            }
        }

        public List<String> GetUrls()
        {
            return LabelStorageUrlsDao.Instance.GetUrls();
        }
        public async Task DownloadListAsync(List<String> rest)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var block = new ActionBlock<String>(async link =>
                    {
                        await LabelStorageUrlsBl.Instance.DownloadFileAsync(httpClient, link);
                    }, new ExecutionDataflowBlockOptions()
                    {
                        MaxDegreeOfParallelism = 10
                    });
                    foreach (var link in rest)
                    {
                        await block.SendAsync(link);
                    }
                    block.Complete();
                    await block.Completion;
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void MergePdf(List<String> list,Boolean splitPdfName=false)
        {
            try
            {
                String path = "D:\\cj\\Elmo\\downloads\\";
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
                outputDocument.Save(path+"MergedPdf.pdf");
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
    }
}
