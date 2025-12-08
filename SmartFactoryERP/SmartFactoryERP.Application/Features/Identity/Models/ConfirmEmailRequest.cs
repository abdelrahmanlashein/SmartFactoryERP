using System.ComponentModel.DataAnnotations;

namespace SmartFactoryERP.Application.Features.Identity.Models
{
    public class ConfirmEmailRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }
    }
}