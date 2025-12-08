using Microsoft.AspNetCore.Http;
using SmartFactoryERP.Application.Interfaces.Identity;
using System.Security.Claims;

namespace SmartFactoryERP.Infrastructure.Services.Identity
{
    /// <summary>
    /// خدمة للحصول على معلومات المستخدم الحالي من JWT Token
    /// </summary>
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// الحصول على UserId من الـ Token (Claim: NameIdentifier)
        /// </summary>
        public string UserId
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return userId ?? string.Empty;
            }
        }

        /// <summary>
        /// الحصول على EmployeeId من الـ Token (Claim: "EmployeeId")
        /// </summary>
        public int? EmployeeId
        {
            get
            {
                var employeeIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("EmployeeId")?.Value;
                
                if (string.IsNullOrEmpty(employeeIdClaim))
                    return null;

                if (int.TryParse(employeeIdClaim, out var employeeId))
                    return employeeId;

                return null;
            }
        }
    }
}
