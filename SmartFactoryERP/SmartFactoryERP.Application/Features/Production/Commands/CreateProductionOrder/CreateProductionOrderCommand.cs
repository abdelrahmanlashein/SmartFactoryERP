using System;
using System.Collections.Generic; // ✅ ضروري لقائمة الخامات
using MediatR;

namespace SmartFactoryERP.Application.Features.Production.Commands.CreateProductionOrder
{
    public class CreateProductionOrderCommand : IRequest<int>
    {
        public int ProductId { get; set; }      // The Finished Good ID
        public int Quantity { get; set; }       // How many to produce?
        public DateTime StartDate { get; set; } // Planned start date
        public string Notes { get; set; }
        public string Priority { get; set; }

        // ✅✅ الإضافة الجديدة: قائمة الخامات التي أدخلها المستخدم ✅✅
        public List<OrderItemInputDto> Items { get; set; }

    }

    // ✅✅ DTO للخامة الواحدة المرسلة في الـ Command ✅✅
    public class OrderItemInputDto
    {
        public int MaterialId { get; set; }
        // الكمية المطلوبة النهائية (quantity of raw material needed)
        public decimal Quantity { get; set; }
    }
}