using Microsoft.AspNetCore.Http;
using SmartFactoryERP.Application.Interfaces.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Infrastructure.Services.Identity
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        public int? EmployeeId
        {
            get
            {
                // بنقرا الـ Claim اللي اسمه "EmployeeId" اللي حطيناه في التوكن
                var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("EmployeeId");
                if (claim != null && int.TryParse(claim.Value, out int id))
                {
                    return id;
                }
                return null;
            }
        }
    }
}
