using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Sales.Commands.GenerateInvoice
{
    public class GenerateInvoiceCommand : IRequest<int>
    {
        public int SalesOrderId { get; set; }
        public DateTime DueDate { get; set; }
    }
}
