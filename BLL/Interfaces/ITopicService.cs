using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface ITopicService
    {
        Task<TopicDTO> CreateAsync(TopicDTO model);
        IEnumerable<TopicDTO> GetAllTopics();
        Task<TopicDTO> GetByIdAsync(int id);
        IEnumerable<TopicDTO> GetByUserId(int userId);
        IEnumerable<TopicDTO> GetByCategoryId(int categoryId);
        Task UpdateAsync(TopicDTO model, int id);
        Task DeleteAsync(int id);
    }
}