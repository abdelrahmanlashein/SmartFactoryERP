namespace SmartFactoryERP.Application.Features.UserManagement.Commands.RegisterUser
{
    public class RegisterUserResponse
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int? EmployeeId { get; set; }
        public List<string> Roles { get; set; } = new();
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}