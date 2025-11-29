using SmartFactoryERP.Domain.Entities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.HR___Departments
{
    public class Department : BaseEntity
    {
        public string Name { get; private set; }
        public string Code { get; private set; } // e.g., "SALES", "INV"
        public string Description { get; private set; }

        // Navigation Property (قسم واحد -> موظفين كتير)
        private readonly List<Employee> _employees = new();
        public virtual IReadOnlyCollection<Employee> Employees => _employees.AsReadOnly();

        private Department() { }

        public static Department Create(string name, string code, string description)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new Exception("Department Name is required.");
            if (string.IsNullOrWhiteSpace(code)) throw new Exception("Department Code is required.");

            return new Department
            {
                Name = name,
                Code = code.ToUpper(),
                Description = description
            };
        }
    }
}
