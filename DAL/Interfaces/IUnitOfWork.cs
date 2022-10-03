using System;
using System.Threading.Tasks;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository CategoryRepository { get; }
        ITopicRepository TopicRepository { get; }
        IMessageRepository MessageRepository { get; }
        UserManager<User> UserManager { get; }
        Task SaveAsync();
    }
}