using System.ComponentModel.DataAnnotations;

namespace FlapKap.Core.DTOs
{
    public class BuyDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int AmountOfProducts { get; set; }
    }
}
