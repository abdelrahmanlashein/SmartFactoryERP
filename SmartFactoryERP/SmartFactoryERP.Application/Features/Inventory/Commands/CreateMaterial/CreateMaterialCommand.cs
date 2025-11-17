using MediatR;
using SmartFactoryERP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Inventory.Commands.CreateMaterial
{
    public class CreateMaterialCommand: IRequest<int>
    {
        public string MaterialCode { get; set; }    
        public string MaterialName { get; set; }
        public MaterialType MaterialType { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal UnitPrice { get; set; }
        public int MinimumStockLevel { get; set; }
    }
}
