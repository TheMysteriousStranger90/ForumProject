using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BLL.DTO
{
    public class TopicCreateDTO
    {
        [Required(ErrorMessage = "Title is required and shouldn't be null"), NotNull]
        public string Title { get; set; }
        
        public int CategoryId { get; set; }
    }
}