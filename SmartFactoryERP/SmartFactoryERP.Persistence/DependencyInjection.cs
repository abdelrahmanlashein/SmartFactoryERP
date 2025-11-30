using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using SmartFactoryERP.Persistence.Context;
using SmartFactoryERP.Persistence.Repositories;
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
            services.AddScoped<ITaskRepository, TaskRepository>(); // 👈 غالباً نسيت دي
            return services;
        }
    }
}
