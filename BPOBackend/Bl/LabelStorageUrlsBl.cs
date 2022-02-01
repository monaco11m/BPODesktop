﻿using Ionic.Zip;
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
        public List<LabelStorageUrl> GetUrlsByParameters(String userId, Int32 groupId, DateTime startDate)
        {
            return LabelStorageUrlsDao.Instance.GetUrlsByParameters(userId,groupId, startDate);
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
                String path = RemoveFileName(fileName);
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
                        String[] splitedPdfName = pdfName.Split('.');
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
        public async Task DownloadZip(String zipFilename,String userId,Int32 groupId,DateTime startDate)
        {
            try
            {
                List<String> filesToZip = new List<String>();
                String path = Path.GetTempPath();
                List<LabelStorageUrl> list = LabelStorageUrlsBl.Instance.GetUrlsByParameters(userId, groupId, startDate);

                await LabelStorageUrlsBl.Instance.DownloadListAsync(list, path);
                
                while (list != null && list.Count > 0)
                {
                    Int64 currentBatchNumber = list[0].BatchNumber;
                    List<LabelStorageUrl> subList = list.Where(x => x.BatchNumber == currentBatchNumber).ToList();

                    String mergedFileName = path + "batchNumber_" + list[0].BatchNumber.ToString()+".pdf";
                    filesToZip.Add(mergedFileName);
                    LabelStorageUrlsBl.Instance.MergePdf(subList.Select(x => x.Url).ToList(), mergedFileName, true);

                    list.RemoveAll(x => x.BatchNumber == currentBatchNumber);
                }

                using (ZipFile zip = new ZipFile())
                {
                    zip.AddFiles(filesToZip, false,"");
                    zip.Save(zipFilename);
                    DeleteFiles(filesToZip);
                }
            }
            catch(Exception ex)
            {

            }
        }
        private String RemoveFileName(String fileNameWithPath)
        {
            String[] fileNameSplited = fileNameWithPath.Split('\\');
            String onlyName = fileNameSplited[fileNameSplited.Length - 1];
            return fileNameWithPath.Substring(0, fileNameWithPath.Length - onlyName.Length);
        }
        private void DeleteFiles(List<String> list)
        {
            foreach(String file in list)
                File.Delete(file);
        }
    }
}
