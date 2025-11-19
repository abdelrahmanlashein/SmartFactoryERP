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
    public class GoodsReceiptItemConfiguration : IEntityTypeConfiguration<GoodsReceiptItem>
    {
        public void Configure(EntityTypeBuilder<GoodsReceiptItem> builder)
        {
            builder.ToTable("GoodsReceiptItems", "Purchasing");

            builder.HasKey(x => x.Id);

            // 1. تحديد العلاقة مع GoodsReceipt (الاستلام)
            // عند حذف إذن الاستلام، نحذف تفاصيله (Cascade OK)
            builder.HasOne(gri => gri.GoodsReceipt)
                   .WithMany(gr => gr.Items)
                   .HasForeignKey(gri => gri.ReceiptID)
                   .OnDelete(DeleteBehavior.Cascade);

            // 2. تحديد العلاقة مع PurchaseOrderItem (سطر أمر الشراء)
            // ⚠️ هذا هو الحل: نستخدم Restrict بدلاً من Cascade
            // عند حذف سطر أمر الشراء، امنع الحذف لو كان له استلامات مخزنية
            builder.HasOne(gri => gri.PurchaseOrderItem)
                   .WithMany() // علاقة من طرف واحد
                   .HasForeignKey(gri => gri.POItemID)
                   .OnDelete(DeleteBehavior.Restrict); // 👈 الحل السحري
        }
    }
}
