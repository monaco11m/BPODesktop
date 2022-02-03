using System;

namespace BPOBackend
{
    public class AutomateLabel
    {
        public long Id { get; set; }
        public long AutomateId { get; set; }
        public string UserId { get; set; }
        public string SessionId { get; set; }
        public DateTime CreatedDate { get; set; }
        public long BatchNumber { get; set; }
        public int RateCardId { get; set; }
        public int OrderType { get; set; }
        public int BatchCount { get; set; }
        public string BatchNotes { get; set; }
        public string ItemName { get; set; }
        public string ItemSku { get; set; }
        public string ItemDetails { get; set; }
        public int ItemQuantity { get; set; }
        public int ProcessedCount { get; set; }
        public int FailedCount { get; set; }
        public int Status { get; set; }
        public string TaskJobId { get; set; }
        public DateTime ScheduledTime { get; set; }
        public string JsonMessage { get; set; }
    }
}
