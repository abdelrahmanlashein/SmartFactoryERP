using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Inventory.Queries.GetMaterialsList
{
    public class MaterialDto
    {
        public int Id { get; set; } // كنا مسميينه MaterialID في الـ entity القديم
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public string MaterialType { get; set; } // هنرجعه string عشان الـ UI
        public string UnitOfMeasure { get; set; }
        public decimal UnitPrice { get; set; }
        public int CurrentStockLevel { get; set; }
        public int MinimumStockLevel { get; set; }
    }
}
