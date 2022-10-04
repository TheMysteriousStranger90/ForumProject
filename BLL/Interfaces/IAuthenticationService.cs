using System.Threading.Tasks;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IAuthenticationService
    {
        public Task SignUpAsync(SignUpDTO signUpDto);
        
        public Task<UserDTO> LoginAsync(LoginDTO loginDto);
    }
}