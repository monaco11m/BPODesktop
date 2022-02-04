using Xunit;
using BPOBackend;
using System;
using System.Collections.Generic;
using System.Text;

namespace BPOBackend.Tests
{
    public class MyStringExtensionTests
    {
        [Fact()]
        public void GetFileFormatTest()
        {
            string str = "https://bukupostv1asset.blob.core.windows.net/labels/e3b6405f-d0b5-40e5-bd3a-861d388738c6.pdf";
            Assert.True(str.GetFileFormat().Equals("pdf"));
        }

        [Fact()]
        public void GetFileNameTest()
        {
            string str = "https://bukupostv1asset.blob.core.windows.net/labels/e3b6405f-d0b5-40e5-bd3a-861d388738c6.pdf";
            Assert.True(str.GetFileName().Equals("e3b6405f-d0b5-40e5-bd3a-861d388738c6"));
        }
    }
}