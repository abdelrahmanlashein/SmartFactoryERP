using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.HR___Departments
{
    public class Department
    {
        public int DepartmentID { get; set; } // (PK)
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }

        // (FK -> Employee) - nullable في حالة كان القسم ليس له مدير بعد
        public int? ManagerID { get; set; }

        public bool IsActive { get; set; }

        // Navigation Properties
        public virtual Employee Manager { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
