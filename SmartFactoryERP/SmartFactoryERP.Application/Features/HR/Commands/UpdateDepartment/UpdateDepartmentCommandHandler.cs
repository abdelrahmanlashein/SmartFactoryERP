using MediatR;
using SmartFactoryERP.Domain.Interfaces.Repositories;

namespace SmartFactoryERP.Application.Features.HR.Commands.UpdateDepartment
{
    public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, bool>
    {
        private readonly IHRRepository _hrRepo;

        public UpdateDepartmentCommandHandler(IHRRepository hrRepo)
        {
            _hrRepo = hrRepo;
        }

        public async Task<bool> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = await _hrRepo.GetDepartmentByIdAsync(request.Id, cancellationToken);
            
            if (department == null)
                throw new Exception($"Department with ID {request.Id} not found");

            department.UpdateDetails(request.Name, request.Code, request.Description);
            
            await _hrRepo.UpdateDepartmentAsync(department, cancellationToken);
            
            return true;
        }
    }
}