using MediatR;

namespace SmartFactoryERP.Application.Features.HR.Commands.DeleteEmployee
{
    public class DeleteEmployeeCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}