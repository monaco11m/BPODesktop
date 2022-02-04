using Xunit;
using BPOBackend;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BPOBackend.Tests
{
    public class AutoBatchingUrlBlTests
    {
        [Fact()]
        public void GetUrlListAutoBatchingTest()
        {
            List<AutoBatchingUrl> result = AutoBatchingUrlBl.Instance.GetUrlListAutoBatching("fe69add2-2149-44dc-9415-cdd640b36925", 55);
            Assert.NotNull(result);
        }

        [Fact()]
        public async Task DownloadZipTestAsync()
        {
            var sw = new Stopwatch();
            sw.Start();
            bool result= await AutoBatchingUrlBl.Instance.DownloadZip("D:\\zip\\DMZ.zip", "fe69add2-2149-44dc-9415-cdd640b36925", 55);
            long ms = sw.ElapsedMilliseconds;
            sw.Stop();
            Assert.True(result&&ms > 0);
        }
        [Fact()]
        public void GetUrlListAutoBatchingQuantityTest()
        {
            long quantity = 0;
            List<AutoBatchingUrl> result = AutoBatchingUrlBl.Instance.GetUrlListAutoBatching("fe69add2-2149-44dc-9415-cdd640b36925", 55);
            foreach(AutoBatchingUrl autoBatchingUrl in result)
            {
                quantity += autoBatchingUrl.UrlList.Count;
            }
            Assert.True(quantity>0);
        }
    }
}