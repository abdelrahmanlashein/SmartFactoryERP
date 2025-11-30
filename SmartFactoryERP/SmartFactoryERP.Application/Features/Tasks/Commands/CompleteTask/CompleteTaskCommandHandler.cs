using MediatR;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Tasks.Commands.CompleteTask
{
    public class CompleteTaskCommandHandler : IRequestHandler<CompleteTaskCommand, Unit>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CompleteTaskCommandHandler(ITaskRepository taskRepository, IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CompleteTaskCommand request, CancellationToken cancellationToken)
        {
            // 1. جلب المهمة
            var task = await _taskRepository.GetTaskByIdAsync(request.Id, cancellationToken);
            if (task == null)
            {
                throw new Exception($"Task with ID {request.Id} not found.");
            }

            // 2. تغيير حالتها لـ Completed (باستخدام الدالة اللي عملناها في الـ Domain)
            task.CompleteTask();

            // 3. الحفظ
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
