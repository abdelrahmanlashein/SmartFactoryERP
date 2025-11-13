using SmartFactoryERP.Domain.Entities.HR___Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.Expenses
{
    public class Expense
    {
        public int ExpenseID { get; set; } // (PK)
        public DateTime ExpenseDate { get; set; }
        public int CategoryID { get; set; } // (FK)
        public int DepartmentID { get; set; } // (FK)
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; } // (مؤقتاً string)
        public DateTime CreatedDate { get; set; }
        public string AttachmentPath { get; set; } // (مسار المرفق)

        // Navigation Properties
        public virtual ExpenseCategory Category { get; set; }
        public virtual Department Department { get; set; }
    }
}
