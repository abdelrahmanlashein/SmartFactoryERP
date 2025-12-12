using Microsoft.EntityFrameworkCore;
using SmartFactoryERP.Domain.Entities.HR;
using SmartFactoryERP.Domain.Entities.HR___Departments;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using SmartFactoryERP.Persistence.Context;

namespace SmartFactoryERP.Persistence.Repositories
{
    public class HRRepository : IHRRepository
    {
        private readonly ApplicationDbContext _context;

        public HRRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===========================
        // DEPARTMENTS
        // ===========================
        
        public async Task AddDepartmentAsync(Department department, CancellationToken cancellationToken)
        {
            await _context.Departments.AddAsync(department, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Department> GetDepartmentByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        }

        public async Task<List<Department>> GetAllDepartmentsAsync(CancellationToken cancellationToken)
        {
            return await _context.Departments
                .Include(d => d.Employees)
                .OrderBy(d => d.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateDepartmentAsync(Department department, CancellationToken cancellationToken)
        {
            _context.Departments.Update(department);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteDepartmentAsync(int id, CancellationToken cancellationToken)
        {
            var department = await _context.Departments.FindAsync(new object[] { id }, cancellationToken);
            if (department != null)
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        // ===========================
        // EMPLOYEES
        // ===========================

        public async Task AddEmployeeAsync(Employee employee, CancellationToken cancellationToken)
        {
            await _context.Employees.AddAsync(employee, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<List<Employee>> GetAllEmployeesAsync(CancellationToken cancellationToken)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .OrderBy(e => e.FullName)
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateEmployeeAsync(Employee employee, CancellationToken cancellationToken)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteEmployeeAsync(int id, CancellationToken cancellationToken)
        {
            var employee = await _context.Employees.FindAsync(new object[] { id }, cancellationToken);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
