using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Persistence.Identity
{
    // يرث من IdentityUser ليأخذ خصائص مثل Email, PasswordHash, PhoneNumber
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        // هذا هو الرابط السحري مع موديول HR
        // كل "يوزر" مرتبط بـ "موظف" واحد
        public int? EmployeeId { get; set; }
    }
}
