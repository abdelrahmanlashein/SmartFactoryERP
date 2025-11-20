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
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers", "Sales");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.CustomerName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(c => c.Email)
                   .HasMaxLength(100);

            builder.Property(c => c.CreditLimit)
                   .HasColumnType("decimal(18,2)");
        }
    }
}
