using App.Api.DTOs;
using App.Api.Helpers;  // PasswordHasher sınıfını kullanabilmek için ekleyin
using App.Api.Models;
using App.Api.Repositories.Interfaces;
using App.Api.Services.Implementations;
using App.Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace App.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;  // IPasswordHasher<User> sınıfını DI ile alıyoruz
        private readonly IUserService _userService;  // IUserService sınıfını DI ile alıyoruz
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;  // TokenService'i DI ile alıyoruz
        public UserController(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, IUserService userService,
                              IConfiguration configuration, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _userService = userService;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            // Şifreyi hash'le
            var passwordHash = _passwordHasher.HashPassword(null, registerDto.Password); // `null` çünkü User nesnesi yeni oluşturulacak

            // Yeni kullanıcıyı oluştur
            var user = new User(registerDto.Username, passwordHash, registerDto.Email, registerDto.Role);

            // Kullanıcıyı veritabanına ekle
            await _userRepository.AddAsync(user);

            return Ok(new { message = "Kullanıcı başarıyla kaydedildi." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userService.AuthenticateUserAsync(loginDto.Username, loginDto.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Geçersiz kullanıcı adı veya şifre." });
            }
            // Token'ı oluştur
            var token = _tokenService.GenerateJwtToken(user.Username, user.Role ?? "User");

            return Ok(new { Token = token });
        }
    }
}
