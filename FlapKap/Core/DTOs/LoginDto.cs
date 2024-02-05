using System.ComponentModel.DataAnnotations;

namespace FlapKap.Core.DTOs
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$",
            ErrorMessage = "Password must have 1 uppercase,1 lowercase, 1 number, 1 non alphanumerice and at least 8 characters")]
        public string Password { get; set; } = null!;
    }
}
