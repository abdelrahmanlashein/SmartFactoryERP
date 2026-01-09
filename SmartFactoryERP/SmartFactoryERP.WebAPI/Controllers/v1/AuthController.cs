using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFactoryERP.Application.Features.Identity.Models;
using SmartFactoryERP.Application.Interfaces.Identity;
using SmartFactoryERP.Domain.Common;
using System.Security.Claims;

namespace SmartFactoryERP.WebAPI.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationRequest request)
        {
            return Ok(await _authService.Register(request));
        } 

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthRequest request)
        {
            return Ok(await _authService.Login(request));
        }

        // ✅ Refresh token
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
        {
            var response = await _authService.RefreshToken(request);
            return Ok(response);
        }

        // ✅ Logout (revoke all tokens)
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            await _authService.RevokeToken(userId);
            return Ok(new { message = "Logged out successfully" });
        }

        // ✅ Forgot password
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
        {
            await _authService.ForgotPassword(request);
            return Ok(new
            {
                message = "If your email is registered, you will receive a password reset link shortly."
            });
        }

        // ✅ Reset password
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            await _authService.ResetPassword(request);
            return Ok(new { message = "Password has been reset successfully. You can now login with your new password." });
        }

        // ✅ Confirm email
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailRequest request)
        {
            await _authService.ConfirmEmail(request);
            return Ok(new { message = "Email confirmed successfully" });
        }

        // ✅ Resend confirmation email (Admin/SuperAdmin only)
        [HttpPost("resend-confirmation-email/{userId}")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Admin}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ResendConfirmationEmail(string userId)
        {
            try
            {
                await _authService.ResendConfirmationEmail(userId);
                return Ok(new { message = "Confirmation email sent successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ✅ Change password
        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            await _authService.ChangePassword(userId, request);
            return Ok(new { message = "Password changed successfully" });
        }

        // ✅ Get account security information
        [HttpGet("account-security")]
        [Authorize]
        public async Task<IActionResult> GetAccountSecurity()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            return Ok(await _authService.GetAccountSecurity(userId));
        }

        // ✅ Unlock account (Admin only)
        [HttpPost("unlock-account/{userId}")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Admin}")]
        public async Task<IActionResult> UnlockAccount(string userId)
        {
            await _authService.UnlockAccount(userId);
            return Ok(new { message = "Account unlocked successfully" });
        }

        // ✅ Assign role (Admin only)
        [HttpPost("assign-role")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Admin}")]
        public async Task<IActionResult> AssignRole(AssignRoleRequest request)
        {
            return Ok(await _authService.AssignRoleToUser(request));
        }

        // ✅ Remove role (Admin only)
        [HttpPost("remove-role")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Admin}")]
        public async Task<IActionResult> RemoveRole(AssignRoleRequest request)
        {
            return Ok(await _authService.RemoveRoleFromUser(request));
        }

        // ✅ Get user roles
        [HttpGet("user-roles/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            return Ok(await _authService.GetUserRoles(userId));
        }

        // ✅ Get all roles
        [HttpGet("roles")]
        [Authorize]
        public async Task<IActionResult> GetAllRoles()
        {
            return Ok(await _authService.GetAllRoles());
        }
    }
}

