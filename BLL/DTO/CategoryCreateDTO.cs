using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BLL.DTO
{
    public class CategoryCreateDTO
    {
        [Required(ErrorMessage = "Name is required and shouldn't be null"), NotNull]
        [StringLength(200, ErrorMessage = "Must be between 5 and 40 characters", MinimumLength = 2)]
        public string Name { get; set; }
    }
}