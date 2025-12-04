using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            // 👇 2. أضف هذا السطر
            services.AddScoped<PdfService>();

            return services;
        }
    }
}
