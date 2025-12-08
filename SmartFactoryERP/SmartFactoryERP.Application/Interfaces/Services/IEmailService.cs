namespace SmartFactoryERP.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
        Task SendPasswordResetEmailAsync(string toEmail, string resetToken, string userName);
        Task SendEmailConfirmationAsync(string toEmail, string confirmationToken, string userName);
    }
}