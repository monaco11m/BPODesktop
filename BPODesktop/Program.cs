using BPOBackend;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BPODesktop
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //DownloadMassiveAsync();
            String result = LabelStorageUrlsBl.Instance.GetPathFromAppSetting();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        static async Task DownloadMassiveAsync()
        {
            List<String> list = LabelStorageUrlsBl.Instance.GetUrls();

            await LabelStorageUrlsBl.Instance.DownloadListAsync(list);
        }

        
    }
}
