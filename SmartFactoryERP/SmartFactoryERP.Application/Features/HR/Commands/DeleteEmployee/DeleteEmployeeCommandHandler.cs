using MediatR;
using SmartFactoryERP.Domain.Interfaces.Repositories;

namespace SmartFactoryERP.Application.Features.HR.Commands.DeleteEmployee
{
    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, bool>
    {
        private readonly IHRRepository _hrRepo;

        public DeleteEmployeeCommandHandler(IHRRepository hrRepo)
        {
            _hrRepo = hrRepo;
        }

        public async Task<bool> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _hrRepo.GetEmployeeByIdAsync(request.Id, cancellationToken);
            
            if (employee == null)
                throw new Exception($"Employee with ID {request.Id} not found");

            await _hrRepo.DeleteEmployeeAsync(request.Id, cancellationToken);
            
            return true;
        }
    }
}