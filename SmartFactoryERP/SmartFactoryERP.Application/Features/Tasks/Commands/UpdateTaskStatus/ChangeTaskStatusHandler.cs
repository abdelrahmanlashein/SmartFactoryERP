using MediatR;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Tasks.Commands.UpdateTaskStatus
{
    public class ChangeTaskStatusHandler : IRequestHandler<ChangeTaskStatusCommand, Unit>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeTaskStatusHandler(ITaskRepository taskRepository, IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(ChangeTaskStatusCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetTaskByIdAsync(request.Id, cancellationToken);
            if (task == null) throw new Exception("Task not found");

            if (request.NewStatus == "Start")
            {
                task.StartTask(); // Domain Logic
            }
            else if (request.NewStatus == "Complete")
            {
                task.CompleteTask(); // Domain Logic
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
