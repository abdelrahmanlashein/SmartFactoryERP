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
    public class MaterialConfiguration : IEntityTypeConfiguration<Material>
    {
        public void Configure(EntityTypeBuilder<Material> builder)
        {
            // تحديد اسم الجدول (اختياري لكن جيد)
            builder.ToTable("Materials", "Inventory");

            // تحديد الـ Primary Key (إذا لم يكن اسمه Id)
            // بما أننا ورثنا من BaseEntity الذي به Id، فهذا السطر اختياري
            builder.HasKey(m => m.Id);

            // القاعدة الأهم: كود المادة لا يتكرر
            builder.HasIndex(m => m.MaterialCode)
                   .IsUnique();

            // قواعد أخرى
            builder.Property(m => m.MaterialCode)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(m => m.MaterialName)
                   .IsRequired()
                   .HasMaxLength(200);

            // تحويل الـ Enum لـ string لسهولة القراءة في الداتابيز
            builder.Property(m => m.MaterialType)
                   .HasConversion<string>()
                   .HasMaxLength(50);

            // --- الإضافة الجديدة ---
            // هذا الفلتر سيطبق "تلقائياً" على كل استعلامات EF Core
            // التي تستهدف جدول Materials
            builder.HasQueryFilter(m => !m.IsDeleted);
        }
    }
}
