using Microsoft.Extensions.Configuration;
using SmartFactoryERP.Application.Interfaces.Services;
using System.Net;
using System.Net.Mail;

namespace SmartFactoryERP.Infrastructure.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");
            
            var fromEmail = emailSettings["FromEmail"];
            var fromName = emailSettings["FromName"];
            var smtpServer = emailSettings["SmtpServer"];
            var smtpPort = int.Parse(emailSettings["SmtpPort"]);
            var smtpUsername = emailSettings["SmtpUsername"];
            var smtpPassword = emailSettings["SmtpPassword"];
            var enableSsl = bool.Parse(emailSettings["EnableSsl"]);

            using var message = new MailMessage();
            message.From = new MailAddress(fromEmail, fromName);
            message.To.Add(new MailAddress(toEmail));
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using var smtpClient = new SmtpClient(smtpServer, smtpPort);
            smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
            smtpClient.EnableSsl = enableSsl;

            try
            {
                await smtpClient.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                // Log the error («” Œœ„ ILogger ›Ì «·≈‰ «Ã)
                throw new Exception($"Failed to send email: {ex.Message}");
            }
        }

        public async Task SendPasswordResetEmailAsync(string toEmail, string resetToken, string userName)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");
            var resetUrl = $"{emailSettings["ResetPasswordUrl"]}?token={Uri.EscapeDataString(resetToken)}&email={Uri.EscapeDataString(toEmail)}";

            var subject = "Reset Your Password - Smart Factory ERP";
            var body = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .button {{ 
                            background-color: #4CAF50; 
                            color: white; 
                            padding: 12px 24px; 
                            text-decoration: none; 
                            border-radius: 4px;
                            display: inline-block;
                            margin: 20px 0;
                        }}
                        .footer {{ color: #666; font-size: 12px; margin-top: 30px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h2>Password Reset Request</h2>
                        <p>Hello {userName},</p>
                        <p>We received a request to reset your password. Click the button below to reset it:</p>
                        <a href='{resetUrl}' class='button'>Reset Password</a>
                        <p>Or copy and paste this link into your browser:</p>
                        <p style='word-break: break-all;'>{resetUrl}</p>
                        <p><strong>This link will expire in 1 hour.</strong></p>
                        <p>If you didn't request a password reset, please ignore this email.</p>
                        <div class='footer'>
                            <p>Smart Factory ERP Team</p>
                            <p>This is an automated message, please do not reply.</p>
                        </div>
                    </div>
                </body>
                </html>
            ";

            await SendEmailAsync(toEmail, subject, body);
        }

        public async Task SendEmailConfirmationAsync(string toEmail, string confirmationToken, string userName)
        {
            var confirmUrl = $"http://localhost:4200/confirm-email?token={Uri.EscapeDataString(confirmationToken)}&email={Uri.EscapeDataString(toEmail)}";

            var subject = "Confirm Your Email - Smart Factory ERP";
            var body = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .button {{ 
                            background-color: #2196F3; 
                            color: white; 
                            padding: 12px 24px; 
                            text-decoration: none; 
                            border-radius: 4px;
                            display: inline-block;
                            margin: 20px 0;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h2>Welcome to Smart Factory ERP!</h2>
                        <p>Hello {userName},</p>
                        <p>Please confirm your email address by clicking the button below:</p>
                        <a href='{confirmUrl}' class='button'>Confirm Email</a>
                        <p>Or copy and paste this link into your browser:</p>
                        <p style='word-break: break-all;'>{confirmUrl}</p>
                    </div>
                </body>
                </html>
            ";

            await SendEmailAsync(toEmail, subject, body);
        }
    }
}