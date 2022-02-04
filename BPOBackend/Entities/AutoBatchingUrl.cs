using System.Collections.Generic;

namespace BPOBackend
{
    public class AutoBatchingUrl
    {
        public long BatchNumber { get; set; }
        public AutoBatchDto AutoBatchDto { get; set; }
        public List<string> UrlList { get; set; }
        public string SummaryPage { get; set; }

    }
}
