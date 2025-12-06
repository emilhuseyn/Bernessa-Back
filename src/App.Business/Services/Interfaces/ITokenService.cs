namespace App.Business.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(string userId, string email, string role);
        string GenerateRefreshToken();
        bool ValidateRefreshToken(string refreshToken);
    }
}
