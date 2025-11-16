using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.Shared
{
    public class BaseAuditableEntity : BaseEntity
    {
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; } // اسم المستخدم الذي أنشأ
        public DateTime? LastUpdated { get; set; }
        public string? LastUpdatedBy { get; set; } // اسم المستخدم الذي عدل
    }
}
