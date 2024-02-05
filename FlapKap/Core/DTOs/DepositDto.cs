using System.ComponentModel.DataAnnotations;

namespace FlapKap.Core.DTOs
{
    public class DepositDto
    {
        [Required]
        public int Amount { get; set; }
    }
}
