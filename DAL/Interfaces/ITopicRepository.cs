using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Interfaces
{
    public interface ITopicRepository : IRepository<Topic>
    {
        IEnumerable<Topic> FindAllWithDetails();

        Task<Topic> GetByIdWithDetailsAsync(int id);
    }
}