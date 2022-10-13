using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly ForumProjectContext _context;
        protected readonly DbSet<TEntity> _set;

        protected Repository(ForumProjectContext context)
        {
            _context = context;
            _set = _context.Set<TEntity>();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _set.FindAsync(id).AsTask();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _set.AsNoTracking().ToListAsync();
        }

        public async Task CreateAsync(TEntity entity)
        {
            await _set.AddAsync(entity);
        }

        public void Remove(TEntity entity)
        {
            _set.Remove(entity);
        }

        public void Update(TEntity entity)
        {
            _set.Update(entity);
        }
    }
}