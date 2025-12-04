using Microsoft.AspNetCore.Identity; // 1. هام جداً
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartFactoryERP.Application.Interfaces.Identity;
using SmartFactoryERP.Domain.Interfaces.AI; // تأكد من الـ Namespaces دي
using SmartFactoryERP.Domain.Interfaces.Repositories;
using SmartFactoryERP.Infrastructure.Services.AI; // لو DemandForecastingService هنا
using SmartFactoryERP.Persistence.Context;
using SmartFactoryERP.Persistence.Identity; // 2. هام عشان ApplicationUser
using SmartFactoryERP.Persistence.Repositories;
using SmartFactoryERP.Persistence.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. إضافة DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // 👇👇 2. تسجيل خدمات Identity (الجزء الناقص) 👇👇
            services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            // 👆👆 نهاية الجزء الناقص 👆👆

            // 3. إضافة الـ Repositories والخدمات الأخرى
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPurchasingRepository, PurchasingRepository>();
            services.AddScoped<ISalesRepository, SalesRepository>();
            services.AddScoped<IProductionRepository, ProductionRepository>();
            services.AddScoped<IAnalyticsRepository, AnalyticsRepository>();
            services.AddScoped<IHRRepository, HRRepository>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();

            // تأكد إن DemandForecastingService موجود في الـ Infrastructure أو Persistence حسب مكانك الحالي
            // لو هو في Infrastructure، المفروض يتسجل هناك، بس لو نقلته هنا ماشي
            services.AddScoped<IForecastingService, DemandForecastingService>();

            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}