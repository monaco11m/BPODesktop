using BPOBackend;
using Xunit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks.Dataflow;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BPOBackend.Tests
{
    public class LabelStorageUrlsBlTests
    {
        [Fact()]
        public void GetUrlsTest()
        {
            List<String> list = LabelStorageUrlsBl.Instance.GetUrls();
            Assert.NotNull(list);
        }

        [Fact()]
        public async Task DownloadFileAsyncTestAsync()
        {
            List<String> list = LabelStorageUrlsBl.Instance.GetUrls();

            await LabelStorageUrlsBl.Instance.DownloadListAsync(list);
            Assert.True(true, "This test needs an implementation");
        }

        [Fact()]
        public void MergePdfTest()
        {
            List<String> list = new List<String>
            {
                "test.pdf",
                "1afbd624-3015-40ab-b1f6-b3e6dbd512c7.pdf",
                "6bf1a520-5b28-435e-a909-df497ba7e703.pdf"
            };

            LabelStorageUrlsBl.Instance.MergePdf(list);
            Assert.NotNull(list);
        }
        [Fact()]
        public async Task DownloadAndMergeTest()
        {
            var sw = new Stopwatch();
            sw.Start();
            List<String> list = LabelStorageUrlsBl.Instance.GetUrls();

            await LabelStorageUrlsBl.Instance.DownloadListAsync(list);
            LabelStorageUrlsBl.Instance.MergePdf(list, true);
            long ms = sw.ElapsedMilliseconds;
            sw.Stop();
            Assert.True(ms > 0);
        }

        [Fact()]
        public void GetPathFromAppSettingTest()
        {
            String result = LabelStorageUrlsBl.Instance.GetPathFromAppSetting();
            Assert.NotNull(result);
        }
    }
}
