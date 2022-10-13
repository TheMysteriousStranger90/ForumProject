using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface ITopicService
    {
        Task<TopicDTO> CreateAsync(TopicDTO model);
        Task<IEnumerable<TopicDTO>> GetAllTopics();
        Task<TopicDTO> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<TopicDTO>> GetAllWithDetailsAsync();
        Task<TopicDTO> GetByIdAsync(int id);
        IEnumerable<TopicDTO> GetByUserId(int userId);
        IEnumerable<TopicDTO> GetByCategoryId(int categoryId);
        Task UpdateAsync(TopicDTO model, int id);
        Task DeleteAsync(int id);

        

    }
}