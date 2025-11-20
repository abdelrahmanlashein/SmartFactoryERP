using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFactoryERP.Domain.Entities.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Persistence.Configurations.Sales
{
    public class SalesOrderConfiguration : IEntityTypeConfiguration<SalesOrder>
    {
        public void Configure(EntityTypeBuilder<SalesOrder> builder)
        {
            builder.ToTable("SalesOrders", "Sales");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OrderNumber).IsRequired().HasMaxLength(50);
            builder.HasIndex(x => x.OrderNumber).IsUnique();

            builder.Property(x => x.Status).HasConversion<string>();

            // Relationship with Items (Cascade delete is usually okay for Draft orders)
            builder.HasMany(x => x.Items)
                   .WithOne(i => i.SalesOrder)
                   .HasForeignKey(i => i.SalesOrderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
