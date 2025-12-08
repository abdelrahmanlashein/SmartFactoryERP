using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartFactoryERP.Application.Interfaces.Services;
using SmartFactoryERP.Application.Interfaces.Identity;
using SmartFactoryERP.Infrastructure.Services.Email;
using SmartFactoryERP.Infrastructure.Services.Identity;
using SmartFactoryERP.Domain.Entities;

namespace SmartFactoryERP.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Email Service
            services.AddScoped<IEmailService, EmailService>();
            
            // JWT Token Generator
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            // ✅ Current User Service (NEW)
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }
    }
}
