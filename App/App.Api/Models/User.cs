using Microsoft.AspNetCore.Identity;
using System.Data;

namespace App.Api.Models
{
    public class User
    {
        public int Id { get; set; }  // User ID
        public string Username { get; set; }  // Kullanıcı adı
        public string PasswordHash { get; set; }  // Şifre (şifre hash'lenmiş olacak)
        public string Email { get; set; }  // Kullanıcı e-posta
        public string Role { get; set; }  // Kullanıcı rolü (örneğin, "Admin", "User" vb.)
        public DateTime CreatedAt { get; set; }  // Hesap oluşturulma tarihi

        // Constructor, şifre hash'lenmiş olarak alınır
        public User(string username, string passwordHash, string email, string role)
        {
            Username = username;
            PasswordHash = passwordHash;
            Email = email;
            Role = role;
            CreatedAt = DateTime.UtcNow;  // Hesap oluşturulma tarihini şu anki zaman olarak set eder
           
        }
    }
}
