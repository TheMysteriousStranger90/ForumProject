using System.ComponentModel.DataAnnotations;

namespace BLL.DTO
{
    public class LoginDTO
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Please, enter valid email address")]
        [StringLength(40, ErrorMessage = "Must be between 5 and 40 characters", MinimumLength = 5)]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required")]
        [StringLength(30, ErrorMessage = "Must be between 5 and 30 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}