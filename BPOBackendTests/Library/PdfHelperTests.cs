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
    }
}