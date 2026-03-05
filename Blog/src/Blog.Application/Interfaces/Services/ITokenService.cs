using Blog.Domain.Enums;

namespace Blog.Application.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateJwtToken(string username, UserRole role, int userId);
    }
}
