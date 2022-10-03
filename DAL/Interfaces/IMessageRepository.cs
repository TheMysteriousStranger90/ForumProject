using System.Collections.Generic;
using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IMessageRepository : IRepository<Message>
    {
        IEnumerable<Message> FindAllByUserId(int userId);
    }
}