using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BLL.DTO
{
    public class SignUpDTO
    {
        [Required(ErrorMessage = "First Name is required"), NotNull]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        [StringLength(70, MinimumLength = 2, ErrorMessage = "The First Name value cannot exceed 70 characters. ")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required"), NotNull]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        [StringLength(70, MinimumLength = 2, ErrorMessage = "The Last Name value cannot exceed 70 characters. ")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Login is required")]
        [StringLength(70, MinimumLength = 5, ErrorMessage = "The Login value cannot exceed 70 characters. ")]
        public string Login { get; set; }

        [EmailAddress] public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password do not match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}