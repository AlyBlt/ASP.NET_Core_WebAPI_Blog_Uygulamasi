using Blog.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Blog.Application.Helpers
{
    public class PasswordHasher : IPasswordHasher<UserEntity>  // IPasswordHasher<User> arayüzünü implement ediyoruz
    {
        public string HashPassword(UserEntity user, string password)
        {
            // Şifreyi hash'lemek için kullanacağımız yöntem
            using (var sha256 = SHA256.Create())  // SHA256 algoritması kullanarak şifre hash'lenir
            {
                var bytes = Encoding.UTF8.GetBytes(password);  // Şifreyi byte dizisine çeviririz
                var hash = sha256.ComputeHash(bytes);  // Hash hesaplanır
                return Convert.ToBase64String(hash);  // Hash'i Base64 string olarak döneriz
            }
        }
        public PasswordVerificationResult VerifyHashedPassword(UserEntity user, string hashedPassword, string providedPassword)
        {
            var hashedProvidedPassword = HashPassword(user, providedPassword); // Şifreyi hash'le
            return hashedPassword == hashedProvidedPassword ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }
}
