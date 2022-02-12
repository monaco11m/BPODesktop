using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace BPOBackend.Bl.Tests
{
    public class BatchLabelsTextBlTests
    {
        [Fact()]
        public void GetBatchLabelsTextTest()
        {
            BatchLabelsText list = BatchLabelsTextBl.Instance.GetBatchLabelsText("54e85c54-14e5-414c-b442-5d6c54da8d9c", 3060, "PL6-1(1)");
            Assert.True(list!=null);
        }
        [Fact()]
        public async Task GetBatchLabelsTextAndSaveTestAsync()
        {
            BatchLabelsText batchLabelsText = BatchLabelsTextBl.Instance.GetBatchLabelsText("54e85c54-14e5-414c-b442-5d6c54da8d9c", 3060, "PL6-1(1)");
            if (batchLabelsText != null)
            {
                await File.WriteAllTextAsync(@"D:\zpl\WriteText.zpl", batchLabelsText.Label);
                await File.WriteAllTextAsync(@"D:\zpl\WriteText.txt", batchLabelsText.Summary);
            }
            Assert.True(batchLabelsText != null);
        }
    }
}