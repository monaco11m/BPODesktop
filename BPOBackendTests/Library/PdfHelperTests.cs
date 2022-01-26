using Xunit;
using BPOBackend;
using System;
using System.Collections.Generic;
using System.Text;

namespace BPOBackend.Tests
{
    public class PdfHelperTests
    {
        [Fact()]
        public void SaveImageAsPdfTest()
        {
            String path = LabelStorageUrlsBl.Instance.GetPathFromAppSetting();
            PdfHelper.Instance.SaveImageAsPdf(path + "00a06c4e-4bea-40ca-afe9-8a4879408b26.png", path + "converted.pdf");

            Assert.True(true, "This test needs an implementation");
        }
    }
}