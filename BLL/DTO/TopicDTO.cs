using System;
using System.Collections.Generic;

namespace BLL.DTO
{
    public class TopicDTO
    {
        public int Id { get; set; }
       
        public string Title { get; set; }
        
        public DateTime CreateTime { get; set; }
        
        public int CategoryId { get; set; }
        
        public int UserId { get; set; }
        public ICollection<int> MessagesId { get; set; }
        
        public TopicDTO()
        {
            MessagesId = new List<int>();
        }
    }
}