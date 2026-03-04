using Blog.Application.DTOs;
using Blog.Application.Helpers;
using Blog.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Blog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;  // IUserService sınıfını DI ile alıyoruz
        private readonly ITokenService _tokenService;  // TokenService'i DI ile alıyoruz
        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (string.IsNullOrWhiteSpace(registerDto.Username) || string.IsNullOrWhiteSpace(registerDto.Password))
                return BadRequest(new { message = "Username ve Password zorunludur." });

            // Servis üzerinden kayıt işlemi yapılacak
            var createdUser = await _userService.RegisterUserAsync(registerDto);
            if (createdUser == null)
                return BadRequest(new { message = "Kullanıcı oluşturulamadı." });

            return Ok(new
            {
                message = "Kullanıcı başarıyla kaydedildi.",
                user = createdUser // UserReadDto dönecek
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (string.IsNullOrWhiteSpace(loginDto.Username) || string.IsNullOrWhiteSpace(loginDto.Password))
                return BadRequest(new { message = "Username ve Password zorunludur." });

            // Servis üzerinden authentication
            var username = loginDto.Username.Trim().ToLower();
            var user = await _userService.AuthenticateUserAsync(username, loginDto.Password);
            if (user == null)
                return Unauthorized(new { message = "Geçersiz kullanıcı adı veya şifre." });

            // Null-safe role
            var role = string.IsNullOrWhiteSpace(user.Role) ? Roles.User : user.Role;

            // Token'ı oluştur
            var token = _tokenService.GenerateJwtToken(user.Username, role, user.Id);
            return Ok(new
            {
                token,
                user // UserReadDto dönecek
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update-role/{userId}")]
        public async Task<IActionResult> UpdateUserRole(int userId, [FromBody] string newRole)
        {
            if (string.IsNullOrEmpty(newRole))
            {
                return BadRequest(new { message = "Rol boş bırakılamaz." });
            }
            newRole = newRole.CapitalizeFirstLetter();
            var allowedRoles = new[] { "Admin", "Author", "User" };
            if (!allowedRoles.Contains(newRole))
                return BadRequest(new { message = "Geçersiz rol." });

            var updatedUser = await _userService.UpdateUserRoleAsync(userId, newRole);
            if (updatedUser == null)
                return NotFound(new { message = "Kullanıcı bulunamadı." });

            return Ok(new
            {
                message = "Kullanıcının rolü güncellendi.",
                user = updatedUser // UserReadDto dönecek
            });

        }
    }
}
