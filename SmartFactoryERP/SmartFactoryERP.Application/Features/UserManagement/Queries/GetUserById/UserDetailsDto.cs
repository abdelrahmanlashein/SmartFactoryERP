namespace SmartFactoryERP.Application.Features.UserManagement.Queries.GetUserById
{
    public class UserDetailsDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int? EmployeeId { get; set; }
        public List<string> Roles { get; set; } = new();
        public bool IsLocked { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }
}