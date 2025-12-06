using App.Business.DTOs.Auth;
using App.Business.DTOs.Commons;

namespace App.Business.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResult<TokenDTO>> LoginAsync(LoginDTO loginDto);
        Task<ServiceResult<TokenDTO>> RefreshTokenAsync(string refreshToken);
        Task<ServiceResult<UserInfoDTO>> GetCurrentUserAsync(string userId);
        Task<ServiceResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<ServiceResult> UpdateProfileAsync(string userId, string firstName, string lastName, string avatar);
        
        // Admin operations - no token required
        Task<ServiceResult> AdminResetPasswordAsync(string email, string newPassword);
    }
}
