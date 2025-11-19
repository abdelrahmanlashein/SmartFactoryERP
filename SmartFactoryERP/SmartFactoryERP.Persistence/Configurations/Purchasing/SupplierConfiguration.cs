using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFactoryERP.Domain.Entities.Purchasing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Persistence.Configurations.Purchasing
{
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            // Table Name
            builder.ToTable("Suppliers", "Purchasing");

            // Primary Key
            builder.HasKey(s => s.Id);

            // Properties Configuration
            builder.Property(s => s.SupplierCode)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(s => s.SupplierName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(s => s.Email)
                   .HasMaxLength(100);

            // Indexes
            // Ensure SupplierCode is unique in the database
            builder.HasIndex(s => s.SupplierCode)
                   .IsUnique();
        }
    }
}
