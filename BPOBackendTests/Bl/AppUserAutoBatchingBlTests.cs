using Xunit;
using System.Collections.Generic;

namespace BPOBackend.Tests
{
    public class AppUserAutoBatchingBlTests
    {
        [Fact()]
        public void GetAutoBatchingUsersTest()
        {
            List<AppUserAutoBatching> list = AppUserAutoBatchingBl.Instance.GetAutoBatchingUsers();
            Assert.NotNull(list);
        }
    }
}