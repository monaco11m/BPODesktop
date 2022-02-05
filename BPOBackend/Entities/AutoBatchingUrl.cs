using System.Collections.Generic;

namespace BPOBackend
{
    public class AutoBatchingUrl
    {
        public long BatchNumber { get; set; }
        public string LabelFileName { get; set; }
        public string SummaryFileName { get; set; }
        public AutoBatchDto AutoBatchDto { get; set; }
        public List<string> UrlList { get; set; }
        public string SummaryPage { get; set; }

    }

    public class AutoBatchingUrlResponse
    {
        public string ZipFileName { get; set; }
        public List<AutoBatchingUrl> AutoBatchingUrl { get; set; }
    }
}
