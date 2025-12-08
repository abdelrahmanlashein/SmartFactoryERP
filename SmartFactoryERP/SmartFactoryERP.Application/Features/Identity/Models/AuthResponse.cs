using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Identity.Models
{
    public class AuthResponse
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Token { get; set; }
        public int? EmployeeId { get; set; }
        public List<string> Roles { get; set; } = new();
        
        // ✅ إضافة RefreshToken
        public string RefreshToken { get; set; }
        public DateTime TokenExpiresAt { get; set; }
    }
}
