using Xunit;
using BPOBackend;
using System;
using System.Collections.Generic;
using System.Text;

namespace BPOBackend.Tests
{
    public class AspNetUserBlTests
    {
        [Fact()]
        public void GetAspNetUserTest()
        {
            List<AspNetUser> list = AspNetUserBl.Instance.GetAspNetUser();
            Assert.NotNull(list);
        }
    }
}