using SmartFactoryERP.Domain.Entities.Inventory;
using SmartFactoryERP.Domain.Entities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.Production
{
    // BOM: The Recipe (e.g., 1 Table = 4 Legs + 1 Top)
    public class BillOfMaterial : BaseEntity
    {
        public int ProductId { get; private set; } // المنتج النهائي
        public int ComponentId { get; private set; } // المادة الخام
        public decimal Quantity { get; private set; } // الكمية المطلوبة

        // Navigation Properties
        public virtual Material Product { get; private set; }
        public virtual Material Component { get; private set; }

        private BillOfMaterial() { }

        public static BillOfMaterial Create(int productId, int componentId, decimal quantity)
        {
            if (quantity <= 0) throw new Exception("Quantity must be positive.");
            if (productId == componentId) throw new Exception("Product cannot be a component of itself.");

            return new BillOfMaterial
            {
                ProductId = productId,
                ComponentId = componentId,
                Quantity = quantity
            };
        }
    }
}
