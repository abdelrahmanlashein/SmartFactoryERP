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
        public double CurrentStock { get; set; }
        public double ReorderLevel { get; set; }
        public string Unit { get; set; }
        public double Shortage => ReorderLevel - CurrentStock; // الكمية المطلوبة للوصول للحد الآمن
    }
}
