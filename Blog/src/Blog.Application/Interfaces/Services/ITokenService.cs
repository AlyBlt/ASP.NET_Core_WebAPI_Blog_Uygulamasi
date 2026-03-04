namespace Blog.Application.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateJwtToken(string username, string role, int userId);
    }
}
