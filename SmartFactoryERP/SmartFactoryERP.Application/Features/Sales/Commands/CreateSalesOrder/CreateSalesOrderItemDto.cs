using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Sales.Commands.CreateSalesOrder
{
    public class CreateSalesOrderItemDto
    {
        public int MaterialId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    } 
}
