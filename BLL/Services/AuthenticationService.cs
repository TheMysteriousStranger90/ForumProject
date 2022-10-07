using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using BLL.Exceptions;
using BLL.Injections;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Entities.Roles;
using DAL.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BLL.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly JwtConfig _jwtConfig;

        public AuthenticationService(IUnitOfWork unitOfWork, IMapper mapper,
            IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        public async Task SignUpAsync(SignUpDTO signUpDto)
        {
            var user = _mapper.Map<User>(signUpDto);
            var result = await _unitOfWork.UserManager.CreateAsync(user, signUpDto.Password);

            if (!result.Succeeded) throw new RegisterException(result.Errors?.FirstOrDefault()?.Description);

            await _unitOfWork.UserManager.AddToRoleAsync(user, RoleTypes.User);
            _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> LoginAsync(LoginDTO loginDto)
        {
            var user = await _unitOfWork.UserManager.FindByEmailAsync(loginDto.Email);

            if (user == null) throw new AccessDeniedException("Email doesn't exist");

            if (!await _unitOfWork.UserManager.CheckPasswordAsync(user, loginDto.Password))
                throw new AccessDeniedException("Password is incorrect");

            var jwtToken = await GenerateJwtToken(user);

            return new UserDTO()
            {
                Success = true,
                Token = jwtToken.ToString()
            };
        }

        private async Task<object> GenerateJwtToken(User user)
        {
            var utcNow = DateTime.UtcNow;
            var roles = await _unitOfWork.UserManager.GetRolesAsync(user);
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret));

            var claims = new[]
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, utcNow.ToString(CultureInfo.InvariantCulture))
            };

            var claimsIdentity = new ClaimsIdentity(claims);
            var expires = DateTime.Now.AddDays(1);

            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                signingCredentials: signingCredentials,
                claims: claimsIdentity.Claims,
                notBefore: utcNow,
                expires: expires
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}