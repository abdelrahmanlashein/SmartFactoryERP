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
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.ToTable("Invoices", "Sales");
            builder.HasKey(i => i.Id);

            builder.HasIndex(i => i.InvoiceNumber).IsUnique();

            builder.Property(i => i.TotalAmount).HasColumnType("decimal(18,2)");

            // Relationship: Invoice belongs to one Order
            builder.HasOne(i => i.SalesOrder)
                   .WithMany()
                   .HasForeignKey(i => i.SalesOrderId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
