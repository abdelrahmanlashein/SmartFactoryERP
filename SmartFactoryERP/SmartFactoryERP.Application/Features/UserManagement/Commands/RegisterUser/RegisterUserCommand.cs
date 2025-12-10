using MediatR;

namespace SmartFactoryERP.Application.Features.UserManagement.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<RegisterUserResponse>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? PhoneNumber { get; set; }
        public int? EmployeeId { get; set; }
        public List<string> Roles { get; set; } = new();
        public bool SendWelcomeEmail { get; set; } = true;
    }
}