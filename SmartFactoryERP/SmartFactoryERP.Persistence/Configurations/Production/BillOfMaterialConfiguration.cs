using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFactoryERP.Domain.Entities.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Persistence.Configurations.Production
{
    public class BillOfMaterialConfiguration : IEntityTypeConfiguration<BillOfMaterial>
    {
        public void Configure(EntityTypeBuilder<BillOfMaterial> builder)
        {
            builder.ToTable("BillOfMaterials", "Production");
            builder.HasKey(x => x.Id);

            // العلاقة: المنتج النهائي (Product)
            builder.HasOne(b => b.Product)
                   .WithMany()
                   .HasForeignKey(b => b.ProductId)
                   .OnDelete(DeleteBehavior.Restrict); // لا تحذف المنتج لو له وصفة

            // العلاقة: المكون (Component)
            builder.HasOne(b => b.Component)
                   .WithMany()
                   .HasForeignKey(b => b.ComponentId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
