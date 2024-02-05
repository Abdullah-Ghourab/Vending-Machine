namespace FlapKap.Core.Models
{
    public class Product : BaseEntity
    {
        public int AmountAvailable { get; set; }
        public int Cost { get; set; }
        public string ProductName { get; set; } = null!;
        public string? SellerId { get; set; }
        public User? Seller { get; set; }
    }
}
