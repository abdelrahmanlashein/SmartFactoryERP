using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.Expenses
{
    public class ExpenseCategory
    {
        public int CategoryID { get; set; } // (PK)
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }
}
