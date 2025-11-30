using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Sales.Commands.CreateSalesOrder
{
    public class CreateSalesOrderCommand : IRequest<int>
    {
        public int CustomerId { get; set; }
        public List<CreateSalesOrderItemDto> Items { get; set; }
    }
}
