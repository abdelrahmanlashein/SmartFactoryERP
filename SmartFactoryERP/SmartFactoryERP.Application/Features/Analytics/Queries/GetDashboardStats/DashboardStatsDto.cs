using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Analytics.Queries.GetDashboardStats
{
    public class DashboardStatsDto
    {
        // Inventory Stats
        public int TotalMaterialsCount { get; set; } // How many types of materials?
        public int LowStockItemsCount { get; set; }  // How many need reordering?

        // Sales Stats
        public int PendingSalesOrders { get; set; }  // Confirmed but not Shipped
        public decimal PotentialRevenue { get; set; } // Total $$ of active orders

        // Production Stats
        public int ActiveProductionOrders { get; set; } // Started or Planned
    }
}
