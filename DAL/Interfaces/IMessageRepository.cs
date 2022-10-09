using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IMessageRepository : IRepository<Message>
    {
        IEnumerable<Message> GetAll();
        IEnumerable<Message> FindAllByUserId(int userId);
        
        Task<IEnumerable<Message>> GetByTopicIdWithDetailsAsync(int topicId);
    }
}