using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFactoryERP.Domain.Entities.HR___Departments;

namespace SmartFactoryERP.Persistence.Configurations.HR
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departments", "HR");
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Name).IsRequired().HasMaxLength(100);
            builder.Property(d => d.Code).IsRequired().HasMaxLength(20);
            builder.HasIndex(d => d.Code).IsUnique();

            // ✅ قول لـ EF Core يستخدم الـ backing field "_employees"
            builder.Metadata
                .FindNavigation(nameof(Department.Employees))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}