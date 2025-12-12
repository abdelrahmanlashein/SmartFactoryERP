using MediatR;

namespace SmartFactoryERP.Application.Features.HR.Commands.DeleteDepartment
{
    public class DeleteDepartmentCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}