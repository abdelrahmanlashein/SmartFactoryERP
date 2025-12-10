using SmartFactoryERP.Domain.Entities.HR;
using SmartFactoryERP.Domain.Entities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartFactoryERP.Domain.Entities.HR___Departments
{
    public class Department : BaseEntity
    {
        public string Name { get; private set; }
        public string Code { get; private set; }
        public string Description { get; private set; }

        // ✅ خلي الـ backing field private بس مش readonly
        private List<Employee> _employees = new();

        // ✅ Property للقراءة فقط (DDD Pattern)
        public IReadOnlyCollection<Employee> Employees => _employees.AsReadOnly();

        private Department() { }

        public static Department Create(string name, string code, string description)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new Exception("Department Name is required.");
            if (string.IsNullOrWhiteSpace(code)) throw new Exception("Department Code is required.");

            return new Department
            {
                Name = name,
                Code = code.ToUpper(),
                Description = description,
                _employees = new List<Employee>() // ✅ Initialize here
            };
        }
    }
}