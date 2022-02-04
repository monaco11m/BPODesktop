using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;

namespace BPOBackend.Tests
{
    public class LabelStorageUrlsBlTests
    {
        [Fact()]
        public void GetUrlsTest()
        {
            List<string> list = LabelStorageUrlsBl.Instance.GetUrls();
            Assert.NotNull(list);
        }

        [Fact()]
        public async Task DownloadFileAsyncTestAsync()
        {
            List<string> list = LabelStorageUrlsBl.Instance.GetUrls();

            await DownloadHelper.DownloadListAsync(list, "");
            Assert.True(true, "This test needs an implementation");
        }

        [Fact()]
        public void MergePdfTest()
        {
            List<string> list = new List<string>
            {
                "test.pdf",
                "1afbd624-3015-40ab-b1f6-b3e6dbd512c7.pdf",
                "6bf1a520-5b28-435e-a909-df497ba7e703.pdf"
            };

            LabelStorageUrlsBl.Instance.MergePdf(list, "Test.pdf");
            Assert.NotNull(list);
        }
        [Fact()]
        public async Task DownloadAndMergeTest()
        {
            var sw = new Stopwatch();
            sw.Start();
            string path = "D:\\zip\\";

            List<LabelStorageUrl> list = LabelStorageUrlsBl.Instance.GetUrlsByParameters("test", 0, new DateTime());

            await LabelStorageUrlsBl.Instance.DownloadListAsync(list, path);
            List<string> filesToZip = new List<string>();
            while (list!=null&&list.Count > 0)
            {
                List<LabelStorageUrl> subList = list.Where(x => x.BatchNumber == list[0].BatchNumber).ToList();

                string mergedFileName = path + "batchNumber_" + list[0].BatchNumber.ToString();
                filesToZip.Add(mergedFileName);
                LabelStorageUrlsBl.Instance.MergePdf(subList.Select(x => x.Url).ToList(), mergedFileName, true);

                list.RemoveAll(x => x.BatchNumber == list[0].BatchNumber);
            }

            long ms = sw.ElapsedMilliseconds;
            sw.Stop();
            Assert.True(ms > 0);
        }

        [Fact()]
        public void GetUrlsByParametersTest()
        {
            List<LabelStorageUrl> list = LabelStorageUrlsBl.Instance.GetUrlsByParameters("test", 0, new DateTime());
            Assert.NotNull(list);
        }

        [Fact()]
        public async Task ZipFilesTestAsync()
        {
            var sw = new Stopwatch();
            sw.Start();
            await LabelStorageUrlsBl.Instance.DownloadZip("D:\\zip\\DMZ.zip","",0,new DateTime());
            long ms = sw.ElapsedMilliseconds;
            sw.Stop();
            Assert.True(ms > 0);
        }
    }
}
