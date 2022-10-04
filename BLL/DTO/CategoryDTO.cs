using System.Collections.Generic;

namespace BLL.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<int> TopicsId { get; set; }
        
        public CategoryDTO()
        {
            TopicsId= new List<int>();
        }
    }
}