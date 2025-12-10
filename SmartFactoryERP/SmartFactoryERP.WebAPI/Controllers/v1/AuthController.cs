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

        // ✅ تجديد الـ Token
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
        {
            var response = await _authService.RefreshToken(request);
            return Ok(response);
        }

        // ✅ تسجيل الخروج (إبطال جميع Tokens)
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

        // ✅ نسيت كلمة المرور
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
        {
            await _authService.ForgotPassword(request);
            return Ok(new
            {
                message = "If your email is registered, you will receive a password reset link shortly."
            });
        }

        // ✅ إعادة تعيين كلمة المرور
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            await _authService.ResetPassword(request);
            return Ok(new { message = "Password has been reset successfully. You can now login with your new password." });
        }

        // ✅ تأكيد البريد الإلكتروني
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailRequest request)
        {
            await _authService.ConfirmEmail(request);
            return Ok(new { message = "Email confirmed successfully" });
        }

        // ✅ إعادة إرسال بريد تأكيد الحساب (Admin/SuperAdmin فقط)
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

        // ✅ تغيير كلمة المرور
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

        // ✅ جلب معلومات أمان الحساب
        [HttpGet("account-security")]
        [Authorize]
        public async Task<IActionResult> GetAccountSecurity()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            return Ok(await _authService.GetAccountSecurity(userId));
        }

        // ✅ فك قفل حساب (Admin فقط)
        [HttpPost("unlock-account/{userId}")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Admin}")]
        public async Task<IActionResult> UnlockAccount(string userId)
        {
            await _authService.UnlockAccount(userId);
            return Ok(new { message = "Account unlocked successfully" });
        }

        // ✅ إسناد صلاحية (Admin فقط)
        [HttpPost("assign-role")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Admin}")]
        public async Task<IActionResult> AssignRole(AssignRoleRequest request)
        {
            return Ok(await _authService.AssignRoleToUser(request));
        }

        // ✅ إزالة صلاحية (Admin فقط)
        [HttpPost("remove-role")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Admin}")]
        public async Task<IActionResult> RemoveRole(AssignRoleRequest request)
        {
            return Ok(await _authService.RemoveRoleFromUser(request));
        }

        // ✅ جلب صلاحيات مستخدم
        [HttpGet("user-roles/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            return Ok(await _authService.GetUserRoles(userId));
        }

        // ✅ جلب جميع الصلاحيات
        [HttpGet("roles")]
        [Authorize]
        public async Task<IActionResult> GetAllRoles()
        {
            return Ok(await _authService.GetAllRoles());
        }
    }
}

