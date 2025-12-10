namespace SmartFactoryERP.Application.Features.UserManagement.Queries.GetAllUsers
{
    public class UserListDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int? EmployeeId { get; set; }
        public List<string> Roles { get; set; } = new();
        public bool IsLocked { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public bool EmailConfirmed { get; set; }
        public int AccessFailedCount { get; set; }
    }
}