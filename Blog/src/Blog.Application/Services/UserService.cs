using AutoMapper;
using Blog.Application.DTOs.Auth;
using Blog.Application.DTOs.User;
using Blog.Application.Interfaces.Repositories;
using Blog.Application.Interfaces.Services;
using Blog.Domain.Entities;
using Blog.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Blog.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<UserEntity> _passwordHasher;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IPasswordHasher<UserEntity> passwordHasher, IMapper mapper)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<UserReadDto?> AuthenticateUserAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return null;

            var normalizedUsername = username.Trim().ToLower();
            var user = await _userRepository.GetByUsernameAsync(normalizedUsername);
            if (user == null)
                return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success ? _mapper.Map<UserReadDto>(user) : null;
        }

        public async Task<UserReadDto> RegisterUserAsync(RegisterDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var userName = dto.UserName.Trim().ToLower();
            var email = dto.Email.Trim().ToLower();

            var user = new UserEntity(userName, string.Empty, email, UserRole.User);
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            await _userRepository.AddAsync(user);
            return _mapper.Map<UserReadDto>(user);
        }

        public async Task<UserReadDto?> UpdateUserRoleAsync(int userId, UserRole newRole)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            user.Role = newRole;
            await _userRepository.UpdateAsync(user);

            return _mapper.Map<UserReadDto>(user);
        }

        public async Task<UserReadDto?> GetCurrentUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            var username = claimsPrincipal?.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(username))
                return null;

            var user = await _userRepository.GetByUsernameAsync(username);
            return user == null ? null : _mapper.Map<UserReadDto>(user);
        }
    }
}