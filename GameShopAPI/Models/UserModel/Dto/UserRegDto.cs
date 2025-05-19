using System.ComponentModel.DataAnnotations;

namespace GameShopAPI.Models.UserModel.Dto
{
    public class UserRegDto
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(25, ErrorMessage = "Name should not exceed 25 characters")]
        [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "Username can only contain letters, numbers and the following special characters: . _ - ")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be atleast 8 characters long")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[!@$%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Password must contain at least one letter, one number, and one special character.")]
        public string? Password { get; set; }
        public string? Avatar { get; set; }
    }
}
