namespace BPOBackend.Bl
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
    }
}
