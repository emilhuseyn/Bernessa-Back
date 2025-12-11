using App.Business.DTOs.Auth;
using App.Business.DTOs.Commons;
using App.Business.Services.Interfaces;
using App.Core.Entities.Identity;
using App.Core.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace App.Business.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<ServiceResult<TokenDTO>> LoginAsync(LoginDTO loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                return ServiceResult<TokenDTO>.FailureResult("Email və ya şifrə yanlışdır");
            }

            if (user.IsDisabled)
            {
                return ServiceResult<TokenDTO>.FailureResult("Hesabınız deaktiv edilib. Dəstək ilə əlaqə saxlayın");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: true);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    return ServiceResult<TokenDTO>.FailureResult("Hesab müvəqqəti olaraq bloklanıb. Zəhmət olmasa bir qədər sonra yenidən cəhd edin");
                }
                return ServiceResult<TokenDTO>.FailureResult("Email və ya şifrə yanlışdır");
            }

            var azerbaijanNow = DateTimeHelper.GetAzerbaijanNow();
            user.LastLoginAt = DateTimeHelper.ToUtcFromAzerbaijan(azerbaijanNow);
            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "User";

            var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Email, role);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var tokenDto = new TokenDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTimeHelper.GetAzerbaijanNow().AddMinutes(60),
                User = new UserInfoDTO
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Avatar = user.Avatar,
                    Role = role
                }
            };

            return ServiceResult<TokenDTO>.SuccessResult(tokenDto, "Uğurla daxil oldunuz");
        }

        public async Task<ServiceResult<TokenDTO>> RefreshTokenAsync(string refreshToken)
        {
            if (!_tokenService.ValidateRefreshToken(refreshToken))
            {
                return ServiceResult<TokenDTO>.FailureResult("Token etibarsızdır");
            }

            return ServiceResult<TokenDTO>.FailureResult("Refresh token funksionallığı hələ tətbiq edilməyib");
        }

        public async Task<ServiceResult<UserInfoDTO>> GetCurrentUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return ServiceResult<UserInfoDTO>.FailureResult("İstifadəçi tapılmadı");
            }

            if (user.IsDisabled)
            {
                return ServiceResult<UserInfoDTO>.FailureResult("Hesabınız deaktiv edilib");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "User";

            var userInfo = new UserInfoDTO
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Avatar = user.Avatar,
                Role = role
            };

            return ServiceResult<UserInfoDTO>.SuccessResult(userInfo);
        }

        public async Task<ServiceResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return ServiceResult.FailureResult("İstifadəçi tapılmadı");
            }

            if (user.IsDisabled)
            {
                return ServiceResult.FailureResult("Hesabınız deaktiv edilib");
            }

            if (string.IsNullOrWhiteSpace(currentPassword))
            {
                return ServiceResult.FailureResult("Cari şifrə daxil edilməlidir");
            }

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                return ServiceResult.FailureResult("Yeni şifrə daxil edilməlidir");
            }

            if (newPassword.Length < 6)
            {
                return ServiceResult.FailureResult("Yeni şifrə ən azı 6 simvol olmalıdır");
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(user, currentPassword);
            if (!passwordCheck)
            {
                return ServiceResult.FailureResult("Cari şifrə yanlışdır");
            }

            if (currentPassword == newPassword)
            {
                return ServiceResult.FailureResult("Yeni şifrə cari şifrədən fərqli olmalıdır");
            }

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ServiceResult.FailureResult($"Şifrə dəyişdirilmədi: {errors}");
            }

            return ServiceResult.SuccessResult("Şifrə uğurla dəyişdirildi");
        }

        public async Task<ServiceResult> UpdateProfileAsync(string userId, string firstName, string lastName, string avatar)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return ServiceResult.FailureResult("İstifadəçi tapılmadı");
            }

            if (user.IsDisabled)
            {
                return ServiceResult.FailureResult("Hesabınız deaktiv edilib");
            }

            if (string.IsNullOrWhiteSpace(firstName))
            {
                return ServiceResult.FailureResult("Ad daxil edilməlidir");
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                return ServiceResult.FailureResult("Soyad daxil edilməlidir");
            }

            user.FirstName = firstName.Trim();
            user.LastName = lastName.Trim();

            if (!string.IsNullOrEmpty(avatar))
            {
                user.Avatar = avatar.Trim();
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ServiceResult.FailureResult($"Profil yenilənmədi: {errors}");
            }

            return ServiceResult.SuccessResult("Profil uğurla yeniləndi");
        }

        public async Task<ServiceResult> AdminResetPasswordAsync(string email, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return ServiceResult.FailureResult("İstifadəçi tapılmadı");
            }

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                return ServiceResult.FailureResult("Yeni şifrə daxil edilməlidir");
            }

            if (newPassword.Length < 6)
            {
                return ServiceResult.FailureResult("Şifrə ən azı 6 simvol olmalıdır");
            }

            var removePasswordResult = await _userManager.RemovePasswordAsync(user);
            if (!removePasswordResult.Succeeded)
            {
                return ServiceResult.FailureResult("Şifrə yenilənmədi");
            }

            var addPasswordResult = await _userManager.AddPasswordAsync(user, newPassword);
            if (!addPasswordResult.Succeeded)
            {
                var errors = string.Join(", ", addPasswordResult.Errors.Select(e => e.Description));
                return ServiceResult.FailureResult($"Şifrə təyin edilmədi: {errors}");
            }

            return ServiceResult.SuccessResult("İstifadəçinin şifrəsi uğurla dəyişdirildi");
        }
    }
}