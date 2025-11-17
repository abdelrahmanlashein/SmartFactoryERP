using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Inventory.Commands.AdjustStock
{
    public class AdjustStockCommand : IRequest<Unit>
    {
        public int MaterialId { get; set; }
        public int Quantity { get; set; } // موجب للإضافة، سالب للخصم
        public string Notes { get; set; }
    }
}
