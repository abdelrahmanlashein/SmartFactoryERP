using SmartFactoryERP.Application.Features.Identity.Models;

namespace SmartFactoryERP.Application.Interfaces.Identity
{
    public interface IAuthService
    {
        Task<AuthResponse> Login(AuthRequest request);
        Task<AuthResponse> Register(RegistrationRequest request);
        Task<UserRolesResponse> AssignRoleToUser(AssignRoleRequest request);
        Task<UserRolesResponse> RemoveRoleFromUser(AssignRoleRequest request);
        Task<UserRolesResponse> GetUserRoles(string userId);
        Task<List<string>> GetAllRoles();
        Task<TokenResponse> RefreshToken(RefreshTokenRequest request);
        Task<bool> RevokeToken(string userId);
        Task<bool> ChangePassword(string userId, ChangePasswordRequest request);
        Task<AccountSecurityResponse> GetAccountSecurity(string userId);
        Task<bool> UnlockAccount(string userId);
        
        // ✅ Password Reset Methods
        Task<bool> ForgotPassword(ForgotPasswordRequest request);
        Task<bool> ResetPassword(ResetPasswordRequest request);
        Task<bool> ConfirmEmail(ConfirmEmailRequest request);
    }
}   