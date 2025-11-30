using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Purchasing.Commands.CreateGoodsReceipt
{
    public class CreateGoodsReceiptCommand : IRequest<int>
    {
        public int PurchaseOrderId { get; set; }
        public int ReceivedById { get; set; }
        public string Notes { get; set; }

        // List of items received
        public List<CreateGoodsReceiptItemDto> Items { get; set; } 
    }
}
