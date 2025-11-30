using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Analytics.Queries.GetDashboardStats
{
    public class DashboardStatsDto
    {
        // ... (الحقول القديمة: Inventory, Production) ...
        public int TotalMaterialsCount { get; set; }
        public int LowStockItemsCount { get; set; }
        public int ActiveProductionOrders { get; set; }

        // --- التحديث المالي ---
        public int PendingSalesOrders { get; set; }
        public decimal TotalRevenue { get; set; }   // إجمالي المبيعات (Confirmed/Invoiced)
        public decimal TotalExpenses { get; set; }  // إجمالي المصروفات
        public decimal NetProfit { get; set; }      // صافي الربح (Revenue - Expenses)
    }
}
