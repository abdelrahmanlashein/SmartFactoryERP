using SmartFactoryERP.Domain.Entities.HR;
using SmartFactoryERP.Domain.Entities.HR___Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Interfaces.Repositories
{
    public interface IHRRepository
    {
        // Departments
        Task AddDepartmentAsync(Department department, CancellationToken cancellationToken);
        Task<Department> GetDepartmentByIdAsync(int id, CancellationToken cancellationToken); // ✅ Added
        Task<List<Department>> GetAllDepartmentsAsync(CancellationToken cancellationToken);
        Task UpdateDepartmentAsync(Department department, CancellationToken cancellationToken); // ✅ Added
        Task DeleteDepartmentAsync(int id, CancellationToken cancellationToken); // ✅ Added

        // Employees
        Task AddEmployeeAsync(Employee employee, CancellationToken cancellationToken);
        Task<Employee> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<Employee>> GetAllEmployeesAsync(CancellationToken cancellationToken);
        Task UpdateEmployeeAsync(Employee employee, CancellationToken cancellationToken); // ✅ Added
        Task DeleteEmployeeAsync(int id, CancellationToken cancellationToken); // ✅ Added
    }
}
