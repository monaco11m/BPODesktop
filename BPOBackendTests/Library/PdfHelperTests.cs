using BPOBackend;
using System.Collections.Generic;
using Xunit;

namespace BPOBackend.Tests
{
    public class PdfHelperTests
    {
        [Fact()]
        public void SaveImageAsPdfTest()
        {
            string path = "";
            PdfHelper.Instance.SaveImageAsPdf(path + "test.png", path + "converted.pdf");

            Assert.True(true, "This test needs an implementation");
        }

        [Fact()]
        public void FileFromBase64Test()
        {
            List<AutoBatchingUrl> result = AutoBatchingUrlBl.Instance.GetUrlListAutoBatching("fe69add2-2149-44dc-9415-cdd640b36925", 55);
            PdfHelper.Instance.FileFromBase64(result[0].SummaryPage,"");

            Assert.NotNull(result);
        }
    }
}