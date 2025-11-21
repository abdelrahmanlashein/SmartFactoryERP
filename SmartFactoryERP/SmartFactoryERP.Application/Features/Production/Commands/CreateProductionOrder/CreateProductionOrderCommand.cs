using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SmartFactoryERP.Application.Features.Production.Commands.CreateProductionOrder
{
    public class CreateProductionOrderCommand : IRequest<int>
    {
        public int ProductId { get; set; }      // The Finished Good ID
        public int Quantity { get; set; }       // How many to produce?
        public DateTime StartDate { get; set; } // Planned start date
        public string Notes { get; set; }
    }
}
