using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(ForumProjectContext context) : base(context)
        {
        }
        public IEnumerable<Message> GetAll()
        {
            return _context.Set<Message>();
        }

        public IEnumerable<Message> FindAllByUserId(int userId)
        {
            return _context.Set<Message>().AsNoTracking().Where(m => m.UserId == userId);
        }

        public async Task<IEnumerable<Message>> GetByTopicIdWithDetailsAsync(int topicId)
        {
            return await _context.Set<Message>().AsNoTracking()
                .Include(m => m.User)
                .Where(m => m.TopicId == topicId)
                .OrderBy(m => m.CreateTime)
                .ToListAsync();
        }
    }
}