using MediatR;

namespace SmartFactoryERP.Application.Features.HR.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int DepartmentId { get; set; }
    }
}