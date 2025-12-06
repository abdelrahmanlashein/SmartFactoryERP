using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Models.Analytics
{
    // الحاوية الكبيرة لكل الرسوم البيانية
    public class DashboardChartsDto
    {
        public List<DailySalesDto> SalesTrend { get; set; }      // Line Chart
        public List<TopProductDto> TopProducts { get; set; }     // Bar Chart
        public List<OrderStatusDto> OrdersStatus { get; set; }   // Pie Chart
    }

    // 1. Line Chart: المبيعات اليومية (آخر 7 أيام)
    public class DailySalesDto
    {
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
    }

    // 2. Bar Chart: أكثر المنتجات مبيعاً
    public class TopProductDto
    {
        public string ProductName { get; set; }
        public int QuantitySold { get; set; }
    }

    // 3. Pie Chart: حالة أوامر البيع
    public class OrderStatusDto
    {
        public string Status { get; set; }
        public int Count { get; set; }
    }
}
