using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Purchasing.Queries.GetGoodsReceiptById
{
    // DTO for a single item within the Goods Receipt response
    public class GoodsReceiptItemDto
    {
        public int PoItemId { get; set; } // The ID of the Purchase Order line item
        public int ReceivedQuantity { get; set; }
        public int RejectedQuantity { get; set; }
    }
}
