using System;
using BLL.Injections;

namespace BLL.DTO
{
    public class UserDTO : AuthResult
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
    }
}