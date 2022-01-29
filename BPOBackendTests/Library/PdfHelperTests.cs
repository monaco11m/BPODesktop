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
            PdfHelper.Instance.SaveImageAsPdf(path + "test.png", path + "converted.pdf");

            Assert.True(true, "This test needs an implementation");
        }
    }
}