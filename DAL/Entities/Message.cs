using System;

namespace DAL.Entities
{
    public class Message : BaseEntity
    {
        public string Text { get; set; }
        public int TopicId { get; set; }
        public Topic Topic { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
        public DateTime CreateTime { get; set; }
    }
}