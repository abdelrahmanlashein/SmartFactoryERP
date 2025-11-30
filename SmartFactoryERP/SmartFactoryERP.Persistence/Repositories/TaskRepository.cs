using Microsoft.EntityFrameworkCore;
using SmartFactoryERP.Domain.Entities.Performance___Task_Management;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using SmartFactoryERP.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Persistence.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task AddTaskAsync(WorkTask task, CancellationToken token)
        {
            await _context.WorkTasks.AddAsync(task, token);
        }

        public async Task<WorkTask> GetTaskByIdAsync(int id, CancellationToken token)
        {
            return await _context.WorkTasks.FindAsync(new object[] { id }, token);
        }

        public async Task<List<WorkTask>> GetTasksByEmployeeIdAsync(int employeeId, CancellationToken token)
        {
            return await _context.WorkTasks
                .Where(t => t.AssignedEmployeeId == employeeId)
                .Include(t => t.AssignedEmployee)
                .ToListAsync(token);
        }

        public async Task<List<WorkTask>> GetAllTasksAsync(CancellationToken token)
        {
            return await _context.WorkTasks
                .Include(t => t.AssignedEmployee)
                .OrderBy(t => t.DueDate)
                .ToListAsync(token);
        }
    }
}
