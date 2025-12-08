using System.ComponentModel.DataAnnotations;

namespace SmartFactoryERP.Application.Features.Identity.Models
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}