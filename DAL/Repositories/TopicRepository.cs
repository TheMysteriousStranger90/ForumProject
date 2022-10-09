using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class TopicRepository : Repository<Topic>, ITopicRepository
    {
        public TopicRepository(ForumProjectContext context) : base(context)
        {
        }
        
        public IEnumerable<Topic> GetAll()
        {
            return _context.Set<Topic>().Include(x => x.Messages);
        }

        public async Task<IEnumerable<Topic>> GetAllWithDetailsAsync()
        {
            return await _context.Set<Topic>()
                .AsNoTracking()
                .Include(t => t.User)
                .OrderByDescending(t => t.CreateTime)
                .ToListAsync();
        }

        public async Task<Topic> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Set<Topic>().AsNoTracking()
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}