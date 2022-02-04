using Xunit;
using System.Collections.Generic;

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