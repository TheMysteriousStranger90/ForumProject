using System;
using System.Threading.Tasks;
using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ForumProjectContext _context;
        public UserManager<User> UserManager { get; }

        public UnitOfWork(ForumProjectContext context,
            UserManager<User> userManager)
        {
            _context = context;
            UserManager = userManager;
        }
        
        private ICategoryRepository _categoryRepository;
        private ITopicRepository _topicRepository;
        private IMessageRepository _messageRepository;
        
        public ICategoryRepository CategoryRepository => _categoryRepository ??= new CategoryRepository(_context);
        public ITopicRepository TopicRepository => _topicRepository ??= new TopicRepository(_context);
        public IMessageRepository MessageRepository => _messageRepository ??= new MessageRepository(_context);
        
        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
        
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}