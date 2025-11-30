using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFactoryERP.Domain.Entities.HR;
using SmartFactoryERP.Domain.Entities.HR___Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Persistence.Configurations.HR
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees", "HR");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.FullName).IsRequired().HasMaxLength(200);
            builder.Property(e => e.JobTitle).HasMaxLength(100);

            // العلاقة: الموظف يتبع قسماً واحداً
            builder.HasOne(e => e.Department)
                   .WithMany(d => d.Employees)
                   .HasForeignKey(e => e.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict); // لا تحذف القسم لو فيه موظفين
        }
    }
}
