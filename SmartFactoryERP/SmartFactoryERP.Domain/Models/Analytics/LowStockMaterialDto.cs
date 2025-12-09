using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Models.Analytics
{
    public class LowStockMaterialDto
    {
        public int MaterialId { get; set; }
        public string Name { get; set; }
        public decimal CurrentStock { get; set; }
        public decimal ReorderLevel { get; set; }
        public string Unit { get; set; }
        public decimal Shortage => ReorderLevel - CurrentStock; // الكمية المطلوبة للوصول للحد الآمن
    }
}
