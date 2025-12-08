namespace SmartFactoryERP.Application.Features.Identity.Models
{
    /// <summary>
    /// „⁄·Ê„«  √„«‰ «·Õ”«»
    /// </summary>
    public class AccountSecurityResponse
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public bool IsLocked { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public int AccessFailedCount { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
    }
}