using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartFactoryERP.Application.Interfaces.Identity;
using SmartFactoryERP.Domain.Interfaces.AI;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using SmartFactoryERP.Infrastructure.Services.AI;
using SmartFactoryERP.Persistence.Context;
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
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))); // 3. سنضيف هذا

            // 2. إضافة الـ Repositories (ربط الـ Interface بالتنفيذ)
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPurchasingRepository, PurchasingRepository>();
            services.AddScoped<ISalesRepository, SalesRepository>();
            services.AddScoped<IProductionRepository, ProductionRepository>();
            services.AddScoped<IAnalyticsRepository, AnalyticsRepository>();
            services.AddScoped<IHRRepository, HRRepository>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IForecastingService, DemandForecastingService>();
            services.AddScoped<IAuthService, AuthService>();
            return services;
        }
    }
}
