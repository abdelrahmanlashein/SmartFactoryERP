using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.Identity
{
    // íÑË ãä IdentityUser áíÃÎĞ ÎÕÇÆÕ ãËá Email, PasswordHash, PhoneNumber
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        // åĞÇ åæ ÇáÑÇÈØ ÇáÓÍÑí ãÚ ãæÏíæá HR
        // ßá "íæÒÑ" ãÑÊÈØ ÈÜ "ãæÙİ" æÇÍÏ
        public int? EmployeeId { get; set; }
    }
}