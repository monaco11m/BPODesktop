using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BPOBackend
{
    public class BatchLabelsTextBl
    {
        private static BatchLabelsTextBl instance = null;
        public static BatchLabelsTextBl Instance
        {
            get
            {
                return instance ?? new BatchLabelsTextBl();
            }
        }
        public BatchLabelsText GetBatchLabelsText(string userId, long batchId, string batchNotes)
        {
            try
            {
                string webResult = WebRequestHandler.ApiGet(SettingsHandler.Instance.GetFromAppSetting("MainDomain") + "getBatchLabelsText?" +
                    string.Format("userId={0}&batchId={1}&batchNotes={2}", userId, batchId, batchNotes));
                return JsonConvert.DeserializeObject<BatchLabelsText>(webResult);
            }
            catch (Exception ex)
            {
                return new BatchLabelsText();
            }
        }
    }
}
