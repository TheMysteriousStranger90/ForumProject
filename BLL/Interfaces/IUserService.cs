using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IUserService
    {
        IEnumerable<UserDTO> GetAllUsers();
        Task<IEnumerable<string>> GetUserRoles(string email);
        Task<UserDTO> GetUserByEmailAsync(string email);
        Task<UserDTO> GetByIdAsync(int id);
        Task SetUserRoleAsync(UserRoleDTO userRoleDto);
        Task UpdateAsync(UserDTO user, int userId);
        Task DeleteByIdAsync(int id);
    }
}