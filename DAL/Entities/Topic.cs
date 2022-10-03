using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Topic : BaseEntity
    {
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
        public DateTime CreateTime { get; set; }
        public ICollection<Message> Messages { get; set; }

        public Topic()
        {
            Messages = new List<Message>();
        }
    }
}