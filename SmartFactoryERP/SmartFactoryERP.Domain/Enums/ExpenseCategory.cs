using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Enums
{
    public enum ExpenseCategory
    {
        Utilities = 1,   // مرافق (كهرباء/مياه)
        Rent = 2,        // إيجار
        Salaries = 3,    // رواتب
        Maintenance = 4, // صيانة
        OfficeSupplies = 5, // أدوات مكتبية
        Other = 6
    }
}
