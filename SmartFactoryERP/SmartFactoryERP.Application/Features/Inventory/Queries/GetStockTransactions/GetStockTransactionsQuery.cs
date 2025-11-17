using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Inventory.Queries.GetStockTransactions
{
    // الأمر هيرجع "قائمة" من الـ DTO
    public class GetStockTransactionsQuery : IRequest<List<StockTransactionDto>>
    {
        public int MaterialId { get; set; }
    }
}
