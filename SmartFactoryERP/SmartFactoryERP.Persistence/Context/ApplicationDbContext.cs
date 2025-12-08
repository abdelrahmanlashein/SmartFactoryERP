using Microsoft.EntityFrameworkCore;
using SmartFactoryERP.Domain.Entities.Expenses;
using SmartFactoryERP.Domain.Entities.HR;
using SmartFactoryERP.Domain.Entities.HR___Departments;
using SmartFactoryERP.Domain.Entities.Inventory;
using SmartFactoryERP.Domain.Entities.Performance___Task_Management;
using SmartFactoryERP.Domain.Entities.Production;
using SmartFactoryERP.Domain.Entities.Purchasing;
using SmartFactoryERP.Domain.Entities.Sales;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SmartFactoryERP.Domain.Entities.Identity;
using System.Reflection;

namespace SmartFactoryERP.Persistence.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Material> Materials { get; set; }
        public DbSet<StockTransaction> StockTransactions { get; set; }
        public DbSet<StockAlert> StockAlerts { get; set; }
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
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<WorkTask> WorkTasks { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        
        // ✅ إضافة جدول RefreshTokens
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
            // ✅ تكوين جدول RefreshTokens
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("RefreshTokens", "Identity");
                entity.HasKey(rt => rt.Id);
                entity.Property(rt => rt.Token).IsRequired().HasMaxLength(500);
                entity.Property(rt => rt.JwtId).IsRequired().HasMaxLength(500);
                entity.HasIndex(rt => rt.Token).IsUnique();
                
                // العلاقة مع ApplicationUser
                entity.HasOne(rt => rt.User)
                      .WithMany()
                      .HasForeignKey(rt => rt.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
