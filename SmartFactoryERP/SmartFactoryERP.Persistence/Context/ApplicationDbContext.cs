using Microsoft.EntityFrameworkCore;
using SmartFactoryERP.Domain.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        // 1. تعريف الجداول
        public DbSet<Material> Materials { get; set; }
        public DbSet<StockTransaction> StockTransactions { get; set; }
        public DbSet<StockAlert> StockAlerts { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // 2. هذه الميثود ستقرأ ملفات الـ Configuration تلقائياً
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
