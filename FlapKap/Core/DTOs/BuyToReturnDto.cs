using FlapKap.Core.Models;

namespace FlapKap.Core.DTOs
{
    public class BuyToReturnDto
    {
        public int Total { get; set; }
        public Product Product { get; set; } = new();
    }
}
