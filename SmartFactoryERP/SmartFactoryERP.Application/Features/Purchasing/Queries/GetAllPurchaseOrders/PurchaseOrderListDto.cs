using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Purchasing.Queries.GetAllPurchaseOrders
{
    // Simplified DTO for listing orders
    public class PurchaseOrderListDto
    {
        public int Id { get; set; }
        public string PONumber { get; set; }
        public string SupplierName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
    }
}
