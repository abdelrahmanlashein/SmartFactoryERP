using MediatR;
using SmartFactoryERP.Domain.Interfaces.Repositories;

namespace SmartFactoryERP.Application.Features.HR.Commands.DeleteDepartment
{
    public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand, bool>
    {
        private readonly IHRRepository _hrRepo;

        public DeleteDepartmentCommandHandler(IHRRepository hrRepo)
        {
            _hrRepo = hrRepo;
        }

        public async Task<bool> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = await _hrRepo.GetDepartmentByIdAsync(request.Id, cancellationToken);
            
            if (department == null)
                throw new Exception($"Department with ID {request.Id} not found");

            // Check if department has employees
            if (department.Employees != null && department.Employees.Any())
            {
                throw new Exception("Cannot delete department with existing employees. Please reassign employees first.");
            }

            await _hrRepo.DeleteDepartmentAsync(request.Id, cancellationToken);
            
            return true;
        }
    }
}