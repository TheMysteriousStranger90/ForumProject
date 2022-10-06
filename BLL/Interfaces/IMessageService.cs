using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IMessageService
    {
        Task<MessageDTO> CreateAsync(MessageDTO model);
        IEnumerable<MessageDTO> GetAllMessages();
        Task<MessageDTO> GetByIdAsync(int id);
        IEnumerable<MessageDTO> FindByUserId(int userId);
        Task UpdateAsync(MessageDTO model);
        Task DeleteAsync(int id);
    }
}