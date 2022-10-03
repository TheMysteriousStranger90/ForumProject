using DAL.Configurations;
using DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    public class ForumProjectContext : IdentityDbContext<User, UserRole, int>
    {
        public ForumProjectContext(){}

        public ForumProjectContext(DbContextOptions<ForumProjectContext> options) : base(options)
        {
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ForumProjectDb;MultipleActiveResultSets=true");
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new TopicConfiguration());
            modelBuilder.ApplyConfiguration(new MessageConfiguration());
        }
        
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Topic> Topics { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
    }
}