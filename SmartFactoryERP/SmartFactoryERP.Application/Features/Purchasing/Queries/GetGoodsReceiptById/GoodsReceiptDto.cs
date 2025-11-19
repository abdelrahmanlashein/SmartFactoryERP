using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Purchasing.Queries.GetGoodsReceiptById
{
    // DTO for the main Goods Receipt header and items
    public class GoodsReceiptDto
    {
        public int Id { get; set; }
        public int PurchaseOrderId { get; set; }
        public DateTime ReceiptDate { get; set; }
        public string ReceivedBy { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }

        // Nested items list
        public List<GoodsReceiptItemDto> Items { get; set; }
    }
}
