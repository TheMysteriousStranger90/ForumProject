using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDTO> CreateAsync(CategoryDTO model);
        Task<IEnumerable<CategoryDTO>> GetAllCategory();
        Task<CategoryDTO> GetByIdAsync(int id);
        Task UpdateAsync(CategoryDTO model, int id);
        Task DeleteAsync(int id);
    }
}