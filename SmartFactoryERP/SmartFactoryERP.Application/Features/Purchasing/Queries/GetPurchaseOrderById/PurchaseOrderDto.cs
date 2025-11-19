using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Purchasing.Queries.GetPurchaseOrderById
{
    public class PurchaseOrderDto
    {
        public int Id { get; set; }
        public string PONumber { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }

        // Nested collection
        public List<PurchaseOrderItemDto> Items { get; set; }
    }
}
