using Xunit;
using BPOBackend;
using System;
using System.Collections.Generic;
using System.Text;

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