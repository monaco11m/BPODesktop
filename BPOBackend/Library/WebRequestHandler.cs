using System.IO;
using System.Net;
using System.Text;

namespace BPOBackend
{
    public static class WebRequestHandler
    {
        public static string ApiGet(string url)
        {
            string contentType = "application/json; charset=utf-8";
            string service = null;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                var request = WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = contentType;
                request.Headers.Add("X-Buku-Salt", SettingsHandler.Instance.GetFromAppSetting("BukuSalt"));
                request.Headers.Add("Authorization", SettingsHandler.Instance.GetFromAppSetting("Authorization"));
                var response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    service = reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                if (ex != null)
                {
                    WebResponse errorResponse = ex.Response;
                    if (errorResponse != null)
                    {
                        using (Stream responseStream = errorResponse.GetResponseStream())
                        {
                            StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                            service = reader.ReadToEnd();
                        }
                    }
                }
            }
            return service;
        }
        
    }
}
