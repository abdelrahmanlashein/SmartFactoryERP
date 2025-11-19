using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Purchasing.Commands.CreatePurchaseOrder
{
    // DTO for a single line item within the Purchase Order command
    public class CreatePurchaseOrderItemDto
    {
        public int MaterialId { get; set; } // The Inventory Material ID
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
