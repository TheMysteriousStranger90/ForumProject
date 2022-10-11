using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.DTO;
using DAL.Entities;

namespace BLL.Interfaces
{
    public interface IMessageService
    {
        Task<MessageDTO> CreateAsync(MessageDTO model);
        IEnumerable<MessageDTO> GetAllMessages();
        Task<MessageDTO> GetByIdAsync(int id);
        IEnumerable<MessageDTO> FindAllByUserId(int userId);
        IEnumerable<MessageDTO> FindByUserId(int userId);
        Task UpdateAsync(MessageDTO model, int id);
        Task DeleteAsync(int id);
        
        Task<IEnumerable<MessageDTO>> GetByTopicIdWithDetailsAsync(int topicId);
    }
}