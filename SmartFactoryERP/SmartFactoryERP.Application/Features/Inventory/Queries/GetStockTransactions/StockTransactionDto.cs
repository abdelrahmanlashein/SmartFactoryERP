using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Inventory.Queries.GetStockTransactions
{
    public class StockTransactionDto
    {
        public long Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; } // e.g., "Adjustment"
        public int Quantity { get; set; } // موجب أو سالب
        public string ReferenceType { get; set; } // e.g., "ManualAdjustment"
        public string Notes { get; set; }
    }
}
