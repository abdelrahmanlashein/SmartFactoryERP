using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Inventory.Commands.UpdateMaterial
{
    public class UpdateMaterialCommand : IRequest<Unit> // Unit = void
    {
        public int Id { get; set; } // هذا الـ Id سيأتي من الـ Route
        public string MaterialName { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal UnitPrice { get; set; }
        public int MinimumStockLevel { get; set; }
    }
}
