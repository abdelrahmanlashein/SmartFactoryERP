using System;
using System.Collections.Generic; // ✅ ضروري

namespace SmartFactoryERP.Application.Features.Production.Queries.GetProductionOrders
{
    public class ProductionOrderDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }

        // ✅ خاصية الأولوية
        public string Priority { get; set; }

        // ✅ قائمة الخامات (هتتملي في صفحة التفاصيل GetById)
        public List<ProductionOrderItemDto> Items { get; set; } = new();
    }

    public class ProductionOrderItemDto
    {
        public int Id { get; set; }

        // ✅✅ لازم السطر ده يكون موجود عشان حساب الـ Stock يشتغل ✅✅
        public int MaterialId { get; set; }

        public string MaterialName { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
    }
}