using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Production.Commands.CreateBOM
{
    public class CreateBillOfMaterialCommand : IRequest<int>
    {
        public int ProductId { get; set; }      // What are we making? (e.g., Table)
        public int ComponentId { get; set; }    // What are we using? (e.g., Wood Leg)
        public decimal Quantity { get; set; }   // How many? (e.g., 4)
    }
}
