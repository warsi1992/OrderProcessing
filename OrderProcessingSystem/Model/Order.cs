namespace OrderProcessingSystem.Model
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public bool IsFulfilled { get; set; }
        public DateTime OrderDate { get; set; }

        // Calculate total price of the order
        public decimal TotalPrice => Products.Sum(p => p.Price);
    }
}
