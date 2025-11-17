using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFactoryERP.Domain.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Persistence.Configurations.Inventory
{
    public class StockTransactionConfiguration : IEntityTypeConfiguration<StockTransaction>
    {
        public void Configure(EntityTypeBuilder<StockTransaction> builder)
        {
            builder.ToTable("StockTransactions", "Inventory");

            // تحديد العلاقة مع جدول Material
            // (تحديد أن المادة الواحدة لها حركات كثيرة)
            builder.HasOne(t => t.Material)
                   .WithMany() // ليس من الضروري إضافة List<StockTransaction> في Material
                   .HasForeignKey(t => t.MaterialID)
                   .OnDelete(DeleteBehavior.Restrict); // امنع حذف مادة لها حركات

            builder.Property(t => t.TransactionType)
                   .HasConversion<string>()
                   .HasMaxLength(50);

            builder.Property(t => t.ReferenceType)
                   .HasConversion<string>()
                   .HasMaxLength(50);
        }
    }
}
