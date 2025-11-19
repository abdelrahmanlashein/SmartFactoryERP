using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Purchasing.Commands.ConfirmPurchaseOrder
{
    public class ConfirmPurchaseOrderCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}

