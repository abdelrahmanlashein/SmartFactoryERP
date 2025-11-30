using MediatR;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Tasks.Queries.GetTasks
{
    public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, List<TaskDto>>
    {
        private readonly ITaskRepository _taskRepository;

        public GetTasksQueryHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<List<TaskDto>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.GetAllTasksAsync(cancellationToken);

            return tasks.Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                Status = t.Status.ToString(),
                Priority = t.Priority.ToString(),
                AssignedEmployeeId = t.AssignedEmployeeId,
                AssignedEmployeeName = t.AssignedEmployee?.FullName ?? "Unassigned"
            }).ToList();
        }
    }
}
