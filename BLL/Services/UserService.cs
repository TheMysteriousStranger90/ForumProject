using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using BLL.Exceptions;
using BLL.Injections;
using BLL.Interfaces;
using DAL.Entities.Roles;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        
        public IEnumerable<UserDTO> GetAllUsers()
        {
            var users = _unitOfWork.UserManager.Users.ToList();
            if (users == null) throw new NotFoundException($"Not found!");
            
            var result = _mapper.Map<IEnumerable<UserDTO>>(users);
            return result;
        }

        public async Task<IEnumerable<string>> GetUserRoles(string email)
        {
            var user = await _unitOfWork.UserManager.FindByEmailAsync(email);
            if (user == null) throw new NotFoundException("User with such email doesn't exist");
            
            return await _unitOfWork.UserManager.GetRolesAsync(user);
        }

        public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            var user = await _unitOfWork.UserManager.FindByEmailAsync(email);
            if (user == null) throw new NotFoundException($"Not found!");
            
            var result = _mapper.Map<UserDTO>(user);
            return result;
        }

        public async Task<UserDTO> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ForumProjectException("Value of id must be positive");
            var user =  await _unitOfWork.UserManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            
            if (user == null) throw new NotFoundException("This user wasn't found");
            return _mapper.Map<UserDTO>(user);
        }

        public async Task SetUserRoleAsync(UserRoleDTO userRoleDto)
        {
            var user = await _unitOfWork.UserManager.Users.FirstOrDefaultAsync(u => u.Id == userRoleDto.Id);

            if (user == null) throw new NotFoundException("User not found");
            if (!string.Equals(userRoleDto.Role, RoleTypes.User) && !string.Equals(userRoleDto.Role, RoleTypes.Moderator)) throw new ForumProjectException("User role is invalid");
            
            var userRoles = await _unitOfWork.UserManager.GetRolesAsync(user);
            await _unitOfWork.UserManager.RemoveFromRolesAsync(user ,userRoles);
            await _unitOfWork.UserManager.AddToRoleAsync(user, userRoleDto.Role);

            var result = await _unitOfWork.UserManager.UpdateAsync(user);
            
            if (!result.Succeeded) throw new ForumProjectException(result.Errors?.FirstOrDefault()?.Description);
                
        }

        public async Task UpdateAsync(UserDTO user, int userId)
        {
            if (user.Id != userId) throw new ForumProjectException("Only this user can update his data");
            
            var userUpdate = await _unitOfWork.UserManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            
            if (userUpdate == null) throw new NotFoundException("User not found");
            
            userUpdate.FirstName = user.FirstName;
            userUpdate.LastName = user.LastName;
            userUpdate.Email = user.Email;
            userUpdate.UserName = user.UserName;

            var result = await _unitOfWork.UserManager.UpdateAsync(userUpdate);
            if (!result.Succeeded) throw new ForumProjectException(result.Errors?.FirstOrDefault()?.Description);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var user = await _unitOfWork.UserManager.Users.Include(u => u.Topics).ThenInclude(u => u.Messages)
                .FirstOrDefaultAsync(u => u.Id == id);
            
            if (user == null) throw new NotFoundException("User not found");
            
            var messages = _unitOfWork.MessageRepository.GetAll().Where(t => t.UserId == id);
            if (messages.Count() != 0)
            {
                foreach (var item in messages)
                {
                    _unitOfWork.MessageRepository.Remove(item);
                }
            }
            
            var topics =  _unitOfWork.TopicRepository.GetAll().Where(t => t.UserId == id);
            if (topics.Count() != 0)
            {
                foreach (var item in topics)
                {
                    _unitOfWork.TopicRepository.Remove(item);
                }
            }
            
            await _unitOfWork.UserManager.DeleteAsync(user);
        }
    }
}