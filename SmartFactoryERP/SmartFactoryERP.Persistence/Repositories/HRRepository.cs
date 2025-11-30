using Microsoft.EntityFrameworkCore;
using SmartFactoryERP.Domain.Entities.HR;
using SmartFactoryERP.Domain.Entities.HR___Departments;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using SmartFactoryERP.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Persistence.Repositories
{
    public class HRRepository : IHRRepository
    {
        private readonly ApplicationDbContext _context;

        public HRRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddDepartmentAsync(Department department, CancellationToken cancellationToken)
        {
            await _context.Departments.AddAsync(department, cancellationToken);
        }

        public async Task<List<Department>> GetAllDepartmentsAsync(CancellationToken cancellationToken)
        {
            return await _context.Departments.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task AddEmployeeAsync(Employee employee, CancellationToken cancellationToken)
        {
            await _context.Employees.AddAsync(employee, cancellationToken);
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Employees
                .Include(e => e.Department) // نجيب اسم القسم مع الموظف
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<List<Employee>> GetAllEmployeesAsync(CancellationToken cancellationToken)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
