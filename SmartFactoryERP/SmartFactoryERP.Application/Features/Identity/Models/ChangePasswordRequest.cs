namespace SmartFactoryERP.Application.Features.Identity.Models
{
    /// <summary>
    /// ÿ·»  €ÌÌ— ﬂ·„… «·„—Ê—
    /// </summary>
    public class ChangePasswordRequest
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}