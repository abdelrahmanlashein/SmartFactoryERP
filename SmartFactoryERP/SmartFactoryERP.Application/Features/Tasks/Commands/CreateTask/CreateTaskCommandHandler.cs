using MediatR;
using SmartFactoryERP.Domain.Entities.Performance___Task_Management;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, int>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IHRRepository _hrRepository; // للتأكد أن الموظف موجود
        private readonly IUnitOfWork _unitOfWork;

        public CreateTaskCommandHandler(
            ITaskRepository taskRepository,
            IHRRepository hrRepository,
            IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _hrRepository = hrRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            // 1. التحقق من وجود الموظف (إذا تم اختياره)
            if (request.AssignedEmployeeId.HasValue)
            {
                var employee = await _hrRepository.GetEmployeeByIdAsync(request.AssignedEmployeeId.Value, cancellationToken);
                if (employee == null)
                    throw new Exception($"Employee with ID {request.AssignedEmployeeId} not found.");
            }

            // 2. إنشاء المهمة
            var task = WorkTask.Create(
                request.Title,
                request.Description,
                request.DueDate,
                request.Priority,
                request.AssignedEmployeeId
            );

            // 3. الإضافة والحفظ
            await _taskRepository.AddTaskAsync(task, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return task.Id;
        }
    }
}
