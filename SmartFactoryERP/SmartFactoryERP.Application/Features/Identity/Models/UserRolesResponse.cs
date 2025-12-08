namespace SmartFactoryERP.Application.Features.Identity.Models
{
    /// <summary>
    /// ’·«ÕÌ«  «·„” Œœ„
    /// </summary>
    public class UserRolesResponse
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}