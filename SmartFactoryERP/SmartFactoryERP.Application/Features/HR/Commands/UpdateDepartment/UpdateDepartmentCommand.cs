using MediatR;

namespace SmartFactoryERP.Application.Features.HR.Commands.UpdateDepartment
{
    public class UpdateDepartmentCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}