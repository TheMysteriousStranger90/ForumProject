using System;

namespace BLL.DTO
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int UserId { get; set; }
        public int TopicId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}