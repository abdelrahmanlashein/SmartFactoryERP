using Microsoft.EntityFrameworkCore;
using SmartFactoryERP.Domain.Entities.Inventory;
using SmartFactoryERP.Domain.Entities.Production;
using SmartFactoryERP.Domain.Entities.Purchasing;
using SmartFactoryERP.Domain.Entities.Sales;
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
        // --- Purchasing (New) ---
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public DbSet<GoodsReceipt> GoodsReceipts { get; set; }
        public DbSet<GoodsReceiptItem> GoodsReceiptItems { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<SalesOrder> SalesOrders { get; set; }
        public DbSet<SalesOrderItem> SalesOrderItems { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<ProductionOrder> ProductionOrders { get; set; }
        public DbSet<BillOfMaterial> BillOfMaterials { get; set; }
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
