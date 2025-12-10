using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Production.Queries.GetBomByProductId
{
    public class BomDto
    {
        public int ProductId { get; set; }
        // قائمة المكونات
        public List<BomLineDto> Components { get; set; } = new();
    }

    public class BomLineDto
    {
        public int ComponentId { get; set; }
        public string ComponentName { get; set; }
        public decimal Quantity { get; set; }
    }
}
