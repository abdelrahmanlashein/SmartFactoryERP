using MediatR;
using SmartFactoryERP.Domain.Interfaces.Repositories;

namespace SmartFactoryERP.Application.Features.HR.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, bool>
    {
        private readonly IHRRepository _hrRepo;

        public UpdateEmployeeCommandHandler(IHRRepository hrRepo)
        {
            _hrRepo = hrRepo;
        }

        public async Task<bool> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _hrRepo.GetEmployeeByIdAsync(request.Id, cancellationToken);
            
            if (employee == null)
                throw new Exception($"Employee with ID {request.Id} not found");

            // Verify department exists
            var department = await _hrRepo.GetDepartmentByIdAsync(request.DepartmentId, cancellationToken);
            if (department == null)
                throw new Exception($"Department with ID {request.DepartmentId} not found");

            employee.UpdateDetails(request.FullName, request.JobTitle, request.PhoneNumber, request.DepartmentId);
            
            await _hrRepo.UpdateEmployeeAsync(employee, cancellationToken);
            
            return true;
        }
    }
}