namespace OrderProcessingSystem.Helper
{
    public class OrderRequest
    {
        public int CustomerId { get; set; }
        public List<int> ProductIds { get; set; }
    }
}
