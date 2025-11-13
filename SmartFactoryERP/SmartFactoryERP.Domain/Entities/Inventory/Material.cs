using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.Inventory
{
    public enum MaterialType
    {
        RawMaterial,
        FinishedGood
    }
    public class Material
    {
        public int MaterialID { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public MaterialType MaterialType { get; set; }
        public string UnitOfMeasure { get; set; } 
        public int MinimumStockLevel { get; set; }
        public int CurrentStockLevel { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? LastUpdated { get; set; }
    }
}
