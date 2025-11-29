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
    public class GoodsReceiptConfiguration : IEntityTypeConfiguration<GoodsReceipt>
    {
        public void Configure(EntityTypeBuilder<GoodsReceipt> builder)
        {
            builder.ToTable("GoodsReceipts", "Purchasing");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Status)
                   .HasConversion<string>();

            // علاقة مع أمر الشراء
            builder.HasOne(gr => gr.PurchaseOrder)
                   .WithMany()
                   .HasForeignKey(gr => gr.PurchaseOrderID)
                   .OnDelete(DeleteBehavior.Restrict); // أيضاً Restrict هنا أفضل

            builder.HasOne(gr => gr.ReceivedBy) // الكيان المرتبط (Employee)
                   .WithMany()                  // الموظف الواحد يستلم بضائع كتير
                   .HasForeignKey(gr => gr.ReceivedById)
                   .OnDelete(DeleteBehavior.Restrict); // مهم: ممنوع حذف موظف قام باستلام بضاعة سابقاً
        }
    }
}
