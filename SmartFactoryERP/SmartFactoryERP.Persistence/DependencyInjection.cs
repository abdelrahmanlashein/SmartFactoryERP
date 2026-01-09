using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartFactoryERP.Application.Interfaces.Identity;
using SmartFactoryERP.Domain.Entities.Identity;
using SmartFactoryERP.Domain.Interfaces.AI;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using SmartFactoryERP.Infrastructure.Services.AI;
using SmartFactoryERP.Infrastructure.Services.Pdf;
using SmartFactoryERP.Persistence.Context;
using SmartFactoryERP.Persistence.Repositories;
using SmartFactoryERP.Persistence.Services;
using System;

namespace SmartFactoryERP.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    // تفعيل إعادة المحاولة التلقائية عند أخطاء مؤقتة (مثال 40613 = Azure SQL unavailable, 1205 = deadlock)
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: new[] { 40613, 1205 } // ضع null إذا أردت الاعتماد على الافتراضيات
                    );

                    // زيادة مهلة تنفيذ الأوامر (ثواني)
                    sqlOptions.CommandTimeout(60);
                }));

            services.AddIdentityCore<ApplicationUser>(options =>
            {
                // Password Settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 4;

                // Lockout Settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User Settings
                options.User.RequireUniqueEmail = true;

                // Sign-in Settings
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // باقي الخدمات...
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPurchasingRepository, PurchasingRepository>();
            services.AddScoped<ISalesRepository, SalesRepository>();
            services.AddScoped<IProductionRepository, ProductionRepository>();
            services.AddScoped<IAnalyticsRepository, AnalyticsRepository>();
            services.AddScoped<IHRRepository, HRRepository>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IAttendanceRepository, AttendanceRepository>();
            services.AddScoped<IForecastingService, DemandForecastingService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<PdfService>();
            return services;
        }
    }
}
