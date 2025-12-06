using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.HR.Queries.GetEmployee
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string DepartmentName { get; set; } // 👈 تأكد أن هذه موجودة
    }
}
