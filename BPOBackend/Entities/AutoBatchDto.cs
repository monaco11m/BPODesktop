namespace BPOBackend
{
    public class AutoBatchDto
    {
        public string ItemName { get; set; }        // Currently first item name only
        public string ItemDetailList { get; set; }  // All items & quantity in json format      
        public string ItemSku { get; set; }         // Currently first item sku only
        public string FulfillSku { get; set; }      // Currently first item fulfillment sku only
        public int Quantity { get; set; }
        public string BatchNotes { get; set; }

    }
}
