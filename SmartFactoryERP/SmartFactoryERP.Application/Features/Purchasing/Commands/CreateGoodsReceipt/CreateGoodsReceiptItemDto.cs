using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Purchasing.Commands.CreateGoodsReceipt
{
    // DTO for a single received item
    public class CreateGoodsReceiptItemDto
    {
        public int POItemId { get; set; } // ID of the Purchase Order Line Item
        public int ReceivedQuantity { get; set; }
        public int RejectedQuantity { get; set; }
    }
}
