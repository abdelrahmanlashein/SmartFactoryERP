using SmartFactoryERP.Domain.Entities.Performance___Task_Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Interfaces.Repositories
{
    public interface ITaskRepository
    {
        System.Threading.Tasks.Task AddTaskAsync(WorkTask task, CancellationToken token);
        Task<WorkTask> GetTaskByIdAsync(int id, CancellationToken token);
        Task<List<WorkTask>> GetTasksByEmployeeIdAsync(int employeeId, CancellationToken token);
        Task<List<WorkTask>> GetAllTasksAsync(CancellationToken token);
    }
}
