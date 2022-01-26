using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPODesktop.Models
{
    public class LabelStorageUrls
    {
        public String Url { get; set; }
    }
    public class LabelStorageUrlsContext: DbContext
    {
        public List<LabelStorageUrls> Data { get; set; }
    }
}
