using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // 1. إضافة MediatR (سيجد كل الـ Handlers تلقائياً)
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            // 2. إضافة FluentValidation (سيجد كل الـ Validators تلقائياً)
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // 3. إضافة AutoMapper (للمستقبل)
            // services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
