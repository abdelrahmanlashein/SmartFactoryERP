using SmartFactoryERP.Domain.Entities.HR___Departments;
using SmartFactoryERP.Domain.Entities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.HR
{

    public class Employee : BaseAuditableEntity, IAggregateRoot
    {
        public string FullName { get; private set; }
        public string JobTitle { get; private set; } // e.g., "Sales Agent", "Warehouse Keeper"
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public DateTime HireDate { get; private set; }
        public bool IsActive { get; private set; }

        // Foreign Key
        public int DepartmentId { get; private set; }
        public virtual Department Department { get; private set; }

        private Employee() { }

        public static Employee Create(string fullName, string jobTitle, string email, string phone, int departmentId)
        {
            if (string.IsNullOrWhiteSpace(fullName)) throw new Exception("Name is required.");
            if (departmentId <= 0) throw new Exception("Department is required.");

            return new Employee
            {
                FullName = fullName,
                JobTitle = jobTitle,
                Email = email,
                PhoneNumber = phone,
                DepartmentId = departmentId,
                HireDate = DateTime.UtcNow,
                IsActive = true
            };
        }

        public void UpdateDetails(string fullName, string jobTitle, string phone, int departmentId)
        {
            FullName = fullName;
            JobTitle = jobTitle;
            PhoneNumber = phone;
            DepartmentId = departmentId;
        }
    }
}


