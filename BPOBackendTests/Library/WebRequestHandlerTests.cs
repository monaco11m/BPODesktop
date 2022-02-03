using Xunit;

namespace BPOBackend.Tests
{
    public class WebRequestHandlerTests
    {
        [Fact()]
        public void ApiGetTest()
        {
            string result = WebRequestHandler.ApiGet("https://localhost:44377/api/v1/getAutoBatchingUsers");
            Assert.NotNull(result);
        }
    }
}