using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.HR___Departments
{
    public class Employee
    {
        public int EmployeeID { get; set; } // (PK)
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int DepartmentID { get; set; } // (FK)
        public string Position { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; }

        // (FK -> User for login) - هذا سيربط بجدول المستخدمين (مثل AspNetUsers)
        // سأفترضه string (Guid) أو int حسب نظام الهوية المستخدم
        public string UserID { get; set; }

        // Navigation Properties
        public virtual Department Department { get; set; }

        // (Navigation property for Manager in Department)
        public virtual Department ManagedDepartment { get; set; }
    }
}
