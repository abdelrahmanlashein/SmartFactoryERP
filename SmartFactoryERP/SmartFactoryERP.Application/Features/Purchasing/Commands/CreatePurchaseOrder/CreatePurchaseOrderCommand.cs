using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Purchasing.Commands.CreatePurchaseOrder
{
    // Command to create a new Purchase Order
    public class CreatePurchaseOrderCommand : IRequest<int>
    {
        public int SupplierId { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public string PONumber { get; set; } // Will be generated or provided

        // The list of items attached to the order
        public List<CreatePurchaseOrderItemDto> Items { get; set; }
    }
}
