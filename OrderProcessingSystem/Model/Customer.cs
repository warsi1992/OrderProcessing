using System.Text.Json.Serialization;

namespace OrderProcessingSystem.Model
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        //[JsonIgnore]
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
