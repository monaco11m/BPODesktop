using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace BPOBackend
{
    public class DownloadHelper
    {
        public static async Task DownloadFileAsync(HttpClient httpClient, String link)
        {
            try
            {
                String[] splitedLink = link.Split('/');
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
            catch (Exception ex)
            {

            }
        }
    }
}
