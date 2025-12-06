using App.Business.DTOs.Auth;
using App.Business.Services.Interfaces;
using App.Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace App.API.Controllers
{
    [Route("api/admin/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IClaimService _claimService;

        public AuthController(IAuthService authService, IClaimService claimService)
        {
            _authService = authService;
            _claimService = claimService;
        }

        /// <summary>
        /// Login with email and password
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);
            
            if (!result.Success)
            {
                return BadRequest(new { success = false, message = result.Message });
            }

            return Ok(new { success = true, data = result.Data, message = result.Message });
        }

        /// <summary>
        /// Refresh access token
        /// </summary>
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var result = await _authService.RefreshTokenAsync(refreshToken);
            
            if (!result.Success)
            {
                return BadRequest(new { success = false, message = result.Message });
            }

            return Ok(new { success = true, data = result.Data, message = result.Message });
        }

        /// <summary>
        /// Get current authenticated user info
        /// </summary>
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = _claimService.GetUserId();
            var result = await _authService.GetCurrentUserAsync(userId);
            
            if (!result.Success)
            {
                return BadRequest(new { success = false, message = result.Message });
            }

            return Ok(new { success = true, data = result.Data });
        }

        /// <summary>
        /// Update user profile (name and avatar)
        /// </summary>
        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDTO updateProfileDto)
        {
            var userId = _claimService.GetUserId();
            var result = await _authService.UpdateProfileAsync(
                userId, 
                updateProfileDto.FirstName, 
                updateProfileDto.LastName, 
                updateProfileDto.Avatar
            );
            
            if (!result.Success)
            {
                return BadRequest(new { success = false, message = result.Message });
            }

            return Ok(new { success = true, message = result.Message });
        }

        /// <summary>
        /// Change password (requires current password)
        /// </summary>
        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDto)
        {
            var userId = _claimService.GetUserId();
            var result = await _authService.ChangePasswordAsync(
                userId, 
                changePasswordDto.CurrentPassword, 
                changePasswordDto.NewPassword
            );
            
            if (!result.Success)
            {
                return BadRequest(new { success = false, message = result.Message });
            }

            return Ok(new { success = true, message = result.Message });
        }

 
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost("admin/reset-password")]
        public async Task<IActionResult> AdminResetPassword([FromBody] AdminResetPasswordDTO adminResetDto)
        {
            var result = await _authService.AdminResetPasswordAsync(
                adminResetDto.Email,
                adminResetDto.NewPassword
            );
            
            if (!result.Success)
            {
                return BadRequest(new { success = false, message = result.Message });
            }

            return Ok(new { success = true, message = result.Message });
        }
    }
}
