using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartFactoryERP.Application.Features.Identity.Models;
using SmartFactoryERP.Application.Interfaces.Identity;
using SmartFactoryERP.Domain.Entities.Identity;
using SmartFactoryERP.Domain.Common;
using SmartFactoryERP.Persistence.Context;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using SmartFactoryERP.Application.Interfaces.Services;

namespace SmartFactoryERP.Persistence.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            ApplicationDbContext context,
            IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _emailService = emailService;
        }

        public async Task<AuthResponse> Login(AuthRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new Exception($"User with email {request.Email} not found.");

            // ✅ التحقق من قفل الحساب
            if (await _userManager.IsLockedOutAsync(user))
            {
                var lockoutEnd = user.LockoutEnd?.ToString("yyyy-MM-dd HH:mm:ss");
                throw new Exception($"Account is locked until {lockoutEnd}. Please try again later.");
            }

            // ✅ التحقق من قفل الحساب
            if (await _userManager.IsLockedOutAsync(user))
            {
                var lockoutEnd = user.LockoutEnd?.ToString("yyyy-MM-dd HH:mm:ss");
                throw new Exception($"Account is locked until {lockoutEnd}. Please try again later.");
            }

            // ✅ التحقق من قفل الحساب
            if (await _userManager.IsLockedOutAsync(user))
            {
                var lockoutEnd = user.LockoutEnd?.ToString("yyyy-MM-dd HH:mm:ss");
                throw new Exception($"Account is locked until {lockoutEnd}. Please try again later.");
            }

            // ✅ التحقق من قفل الحساب
            if (await _userManager.IsLockedOutAsync(user))
            {
                var lockoutEnd = user.LockoutEnd?.ToString("yyyy-MM-dd HH:mm:ss");
                throw new Exception($"Account is locked until {lockoutEnd}. Please try again later.");
            }

            var result = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!result)
                throw new Exception($"Invalid credentials for {request.Email}.");

            var roles = await _userManager.GetRolesAsync(user);
            var jwtToken = await GenerateJwtToken(user, roles);
            var refreshToken = await GenerateRefreshToken(user, jwtToken.JwtId);

            return new AuthResponse
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Token = jwtToken.Token,
                EmployeeId = user.EmployeeId,
                Roles = roles.ToList(),
                RefreshToken = refreshToken.Token,
                TokenExpiresAt = jwtToken.ExpiresAt
            };
        }

        public async Task<AuthResponse> Register(RegistrationRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                throw new Exception($"User with email {request.Email} already exists.");

            var user = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.Email,
                FullName = request.FullName,
                EmployeeId = request.EmployeeId
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Registration failed: {errors}");
            }

            await _userManager.AddToRoleAsync(user, Roles.Employee);

            var roles = await _userManager.GetRolesAsync(user);
            var jwtToken = await GenerateJwtToken(user, roles);
            var refreshToken = await GenerateRefreshToken(user, jwtToken.JwtId);

            return new AuthResponse
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Token = jwtToken.Token,
                EmployeeId = user.EmployeeId,
                Roles = roles.ToList(),
                RefreshToken = refreshToken.Token,
                TokenExpiresAt = jwtToken.ExpiresAt
            };
        }

        // ✅ تجديد الـ Token
        public async Task<TokenResponse> RefreshToken(RefreshTokenRequest request)
        {
            // 1. التحقق من صحة الـ JWT Token (حتى لو منتهي)
            var principal = GetPrincipalFromExpiredToken(request.Token);
            if (principal == null)
                throw new Exception("Invalid token");

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                throw new Exception("Invalid token");

            // 2. جلب الـ Refresh Token من قاعدة البيانات
            var storedRefreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken && rt.UserId == userId);

            if (storedRefreshToken == null)
                throw new Exception("Refresh token not found");

            // 3. التحقق من صحة الـ Refresh Token
            if (storedRefreshToken.IsUsed)
                throw new Exception("Refresh token has already been used");

            if (storedRefreshToken.IsRevoked)
                throw new Exception("Refresh token has been revoked");

            if (storedRefreshToken.ExpiryDate < DateTime.UtcNow)
                throw new Exception("Refresh token has expired");

            if (storedRefreshToken.JwtId != principal.FindFirst(JwtRegisteredClaimNames.Jti)?.Value)
                throw new Exception("Token mismatch");

            // 4. تعليم الـ Refresh Token القديم كمستخدم
            storedRefreshToken.IsUsed = true;
            _context.RefreshTokens.Update(storedRefreshToken);
            await _context.SaveChangesAsync();

            // 5. إنشاء Token جديد
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            var newJwtToken = await GenerateJwtToken(user, roles);
            var newRefreshToken = await GenerateRefreshToken(user, newJwtToken.JwtId);

            return new TokenResponse
            {
                Token = newJwtToken.Token,
                RefreshToken = newRefreshToken.Token,
                ExpiresAt = newJwtToken.ExpiresAt
            };
        }

        // ✅ إبطال الـ Refresh Tokens (عند Logout)
        public async Task<bool> RevokeToken(string userId)
        {
            var tokens = await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && !rt.IsRevoked)
                .ToListAsync();

            if (!tokens.Any())
                return false;

            foreach (var token in tokens)
            {
                token.IsRevoked = true;
            }

            _context.RefreshTokens.UpdateRange(tokens);
            await _context.SaveChangesAsync();

            return true;
        }

        // ✅ إنشاء JWT Token
        private async Task<(string Token, string JwtId, DateTime ExpiresAt)> GenerateJwtToken(
            ApplicationUser user, 
            IList<string> roles)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);
            var jwtId = Guid.NewGuid().ToString();
            var expiresAt = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["DurationInMinutes"]));

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, jwtId),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("FullName", user.FullName ?? ""),
                new Claim("EmployeeId", user.EmployeeId?.ToString() ?? "")
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiresAt,
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return (tokenHandler.WriteToken(token), jwtId, expiresAt);
        }

        // ✅ إنشاء Refresh Token
        private async Task<RefreshToken> GenerateRefreshToken(ApplicationUser user, string jwtId)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var refreshTokenDays = double.Parse(jwtSettings["RefreshTokenDurationInDays"]);

            var refreshToken = new RefreshToken
            {
                Token = GenerateRandomToken(),
                JwtId = jwtId,
                UserId = user.Id,
                CreatedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(refreshTokenDays),
                IsUsed = false,
                IsRevoked = false
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken;
        }

        // ✅ إنشاء Random Token آمن
        private string GenerateRandomToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        // ✅ الحصول على Principal من Token منتهي
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false, // ✅ هنا نتجاهل انتهاء الصلاحية
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                
                if (validatedToken is not JwtSecurityToken jwtToken ||
                    !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        // باقي الميثودز (AssignRoleToUser, RemoveRoleFromUser, GetUserRoles, GetAllRoles)
        // نفس الكود السابق...
        
        public async Task<UserRolesResponse> AssignRoleToUser(AssignRoleRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                throw new Exception($"User with ID {request.UserId} not found.");

            if (!await _roleManager.RoleExistsAsync(request.RoleName))
                throw new Exception($"Role '{request.RoleName}' does not exist.");

            var result = await _userManager.AddToRoleAsync(user, request.RoleName);
            if (!result.Succeeded)
                throw new Exception($"Failed to assign role: {string.Join(", ", result.Errors.Select(e => e.Description))}");

            var roles = await _userManager.GetRolesAsync(user);
            return new UserRolesResponse
            {
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Roles = roles.ToList()
            };
        }

        public async Task<UserRolesResponse> RemoveRoleFromUser(AssignRoleRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                throw new Exception($"User with ID {request.UserId} not found.");

            var result = await _userManager.RemoveFromRoleAsync(user, request.RoleName);
            if (!result.Succeeded)
                throw new Exception($"Failed to remove role: {string.Join(", ", result.Errors.Select(e => e.Description))}");

            var roles = await _userManager.GetRolesAsync(user);
            return new UserRolesResponse
            {
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Roles = roles.ToList()
            };
        }

        public async Task<UserRolesResponse> GetUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception($"User with ID {userId} not found.");

            var roles = await _userManager.GetRolesAsync(user);
            return new UserRolesResponse
            {
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Roles = roles.ToList()
            };
        }

        public async Task<List<string>> GetAllRoles()
        {
            return await Task.FromResult(Roles.AllRoles.ToList());
        }

        // أضف هذه الميثودز في نهاية الكلاس

        // ✅ تغيير كلمة المرور
        public async Task<bool> ChangePassword(string userId, ChangePasswordRequest request)
        {
            // التحقق من تطابق كلمة المرور الجديدة
            if (request.NewPassword != request.ConfirmPassword)
                throw new Exception("New password and confirmation do not match");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception($"User with ID {userId} not found");

            // تغيير كلمة المرور
            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to change password: {errors}");
            }

            // ✅ إبطال جميع Refresh Tokens بعد تغيير كلمة المرور
            await RevokeToken(userId);

            return true;
        }

        // ✅ جلب معلومات أمان الحساب
        public async Task<AccountSecurityResponse> GetAccountSecurity(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception($"User with ID {userId} not found");

            return new AccountSecurityResponse
            {
                UserId = user.Id,
                Email = user.Email,
                IsLocked = await _userManager.IsLockedOutAsync(user),
                LockoutEnd = user.LockoutEnd,
                AccessFailedCount = user.AccessFailedCount,
                TwoFactorEnabled = user.TwoFactorEnabled,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed
            };
        }

        // ✅ فك قفل الحساب (Admin فقط)
        public async Task<bool> UnlockAccount(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception($"User with ID {userId} not found");

            // إعادة تعيين عداد المحاولات الفاشلة
            var result = await _userManager.ResetAccessFailedCountAsync(user);
            if (!result.Succeeded)
                throw new Exception("Failed to reset access failed count");

            // فك القفل
            result = await _userManager.SetLockoutEndDateAsync(user, null);
            if (!result.Succeeded)
                throw new Exception("Failed to unlock account");

            return true;
        }

        // نسيت كلمة المرور - إرسال رابط الاستعادة
        public async Task<bool> ForgotPassword(ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            
            // ⚠️ لأسباب أمنية، لا نخبر المستخدم إذا كان البريد غير موجود
            if (user == null)
                return true; // نرجع true حتى لا نكشف وجود البريد من عدمه

            // إنشاء Token للاستعادة
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            // إرسال البريد الإلكتروني
            try
            {
                await _emailService.SendPasswordResetEmailAsync(user.Email, resetToken, user.FullName);
            }
            catch (Exception ex)
            {
                // Log the error but don't reveal it to the user
                throw new Exception("Failed to send password reset email. Please try again later.");
            }

            return true;
        }

        // إعادة تعيين كلمة المرور
        public async Task<bool> ResetPassword(ResetPasswordRequest request)
        {
            if (request.NewPassword != request.ConfirmPassword)
                throw new Exception("Passwords do not match");

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new Exception("Invalid request");

            // إعادة تعيين كلمة المرور باستخدام Token
            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to reset password: {errors}");
            }

            // ✅ إبطال جميع Refresh Tokens بعد إعادة تعيين كلمة المرور
            await RevokeToken(user.Id);

            return true;
        }

        // تأكيد البريد الإلكتروني
        public async Task<bool> ConfirmEmail(ConfirmEmailRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new Exception("Invalid request");

            var result = await _userManager.ConfirmEmailAsync(user, request.Token);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to confirm email: {errors}");
            }

            return true;
        }
    }
}