using AutoMapper;
using Blog.Application.DTOs;
using Blog.Application.Interfaces.Repositories;
using Blog.Application.Interfaces.Services;
using Blog.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


namespace Blog.Application.Services
{
    public class UserService : IUserService // IUserService implement edildi
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

        public async Task<UserReadDto> AuthenticateUserAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return null;
            var normalizedUsername = username.Trim().ToLower();
            var user = await _userRepository.GetByUsernameAsync(normalizedUsername);
            if (user == null)
            {
                return null; // Kullanıcı bulunamadı
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success ? _mapper.Map<UserReadDto>(user) : null;
        }

        public async Task<UserReadDto> RegisterUserAsync(RegisterDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
                return null;

            var userName = dto.Username.Trim().ToLower();
            // Şifreyi hash'lemek için önce user nesnesi oluşturulmalı
            var user = new UserEntity(userName, string.Empty, dto.Email, "User"); //Placeholder role, email burada gerçek değer olacak

            // Şifreyi hash'le ve User nesnesine ekle
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            await _userRepository.AddAsync(user); // Veritabanına ekleyelim
            return _mapper.Map<UserReadDto>(user);
        }

        public async Task<UserReadDto> UpdateUserRoleAsync(int userId, string newRole)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            user.Role = newRole;
            await _userRepository.UpdateAsync(user);

            return _mapper.Map<UserReadDto>(user);
        }

        public async Task<UserReadDto> GetCurrentUserAsync(HttpContext httpContext)
        {
            var username = httpContext.User?.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(username))
                return null;

            var user = await _userRepository.GetByUsernameAsync(username);
            return user == null ? null : _mapper.Map<UserReadDto>(user);
        }
    }
}
