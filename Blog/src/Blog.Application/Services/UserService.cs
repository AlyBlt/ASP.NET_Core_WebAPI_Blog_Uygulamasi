using Blog.Application.Interfaces.Services;
using Blog.Application.Interfaces.Repositories;
using Blog.Domain.Entities;
using Microsoft.AspNetCore.Identity;


namespace Blog.Application.Services
{
    public class UserService : IUserService // IUserService implement edildi
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<UserEntity> _passwordHasher;

        public UserService(IUserRepository userRepository, IPasswordHasher<UserEntity> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserEntity> AuthenticateUserAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
            {
                return null; // Kullanıcı bulunamadı
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result == PasswordVerificationResult.Failed)
            {
                return null; // Şifre yanlış
            }

            return user; // Kullanıcı doğrulandı
        }

        public async Task RegisterUserAsync(string username, string password, string email)
        {
            // Şifreyi hash'lemek için önce user nesnesi oluşturulmalı
            var user = new UserEntity(username, string.Empty, email, "User"); //Placeholder role, email burada gerçek değer olacak

            // Şifreyi hash'le ve User nesnesine ekle
            user.PasswordHash = _passwordHasher.HashPassword(user, password);

            await _userRepository.AddAsync(user); // Veritabanına ekleyelim
        }
    }
}
