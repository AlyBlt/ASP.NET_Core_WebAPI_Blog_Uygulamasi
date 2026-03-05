using Blog.Application.DTOs.Auth;
using Blog.Application.Interfaces.Services;
using Blog.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (string.IsNullOrWhiteSpace(registerDto.UserName) || string.IsNullOrWhiteSpace(registerDto.Password))
                return BadRequest(new { message = "Username ve Password zorunludur." });

            var createdUser = await _userService.RegisterUserAsync(registerDto);
            if (createdUser == null)
                return BadRequest(new { message = "Kullanıcı oluşturulamadı." });

            return Ok(new
            {
                message = "Kullanıcı başarıyla kaydedildi.",
                user = createdUser
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (string.IsNullOrWhiteSpace(loginDto.UserName) || string.IsNullOrWhiteSpace(loginDto.Password))
                return BadRequest(new { message = "Username ve Password zorunludur." });

            var username = loginDto.UserName.Trim().ToLower();
            var user = await _userService.AuthenticateUserAsync(username, loginDto.Password);
            if (user == null)
                return Unauthorized(new { message = "Geçersiz kullanıcı adı veya şifre." });

            // Role artık enum, null-safe kontrol gerekmez
            var token = _tokenService.GenerateJwtToken(user.UserName, user.Role, user.Id);

            return Ok(new
            {
                token,
                user
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update-role/{userId}")]
        public async Task<IActionResult> UpdateUserRole(int userId, [FromBody] string newRole)
        {
            if (string.IsNullOrWhiteSpace(newRole))
                return BadRequest(new { message = "Rol boş bırakılamaz." });

            // Enum parsing
            if (!Enum.TryParse<UserRole>(newRole, true, out var roleEnum))
                return BadRequest(new { message = "Geçersiz rol." });

            var updatedUser = await _userService.UpdateUserRoleAsync(userId, roleEnum);
            if (updatedUser == null)
                return NotFound(new { message = "Kullanıcı bulunamadı." });

            return Ok(new
            {
                message = "Kullanıcının rolü güncellendi.",
                user = updatedUser
            });
        }
    }
}