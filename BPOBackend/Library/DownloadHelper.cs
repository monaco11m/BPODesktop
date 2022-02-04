using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace BPOBackend
{
    public static class DownloadHelper
    {
        public static async Task DownloadFileAsync(HttpClient httpClient, string path,string link)
        {
            try
            {
                string[] splitedLink = link.Split('/');
                string filePath = Path.Combine(path, splitedLink[splitedLink.Length - 1]);
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
            catch (Exception ex)
            {

            }
        }
        public static async Task DownloadListAsync(List<string> list, string path)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var block = new ActionBlock<string>(async link =>
                    {
                        await DownloadFileAsync(httpClient, path, link);
                    }, new ExecutionDataflowBlockOptions()
                    {
                        MaxDegreeOfParallelism = 10
                    });
                    foreach (string link in list)
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
    }
}
