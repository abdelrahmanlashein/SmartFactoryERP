using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartFactoryERP.Application.Interfaces.Identity;
using SmartFactoryERP.Infrastructure.Services.Identity;
using SmartFactoryERP.Infrastructure.Services.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // ... (أي خدمات أخرى مسجلة هنا مثل EmailService أو DateTimeService) ...

            services.AddScoped<PdfService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            return services;
        }
    }
}
