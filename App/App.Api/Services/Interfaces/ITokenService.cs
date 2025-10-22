namespace App.Api.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateJwtToken(string username, string role);
    }
}
