using SmartFactoryERP.Domain.Entities.Inventory;
using SmartFactoryERP.Domain.Entities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.Purchasing
{
    public class PurchaseOrderItem : BaseEntity
    {
        public int PurchaseOrderID { get; private set; }
        public int MaterialID { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        // خاصية محسوبة (Computed Property) لا تخزن في الداتابيز عادة، أو تحسب وقت الحفظ
        public decimal TotalPrice => Quantity * UnitPrice;

        public virtual PurchaseOrder PurchaseOrder { get; private set; }
        public virtual Material Material { get; private set; }

        private PurchaseOrderItem() { }

        internal static PurchaseOrderItem Create(int materialId, int quantity, decimal unitPrice)
        {
            if (quantity <= 0) throw new Exception("Quantity must be > 0");

            return new PurchaseOrderItem
            {
                MaterialID = materialId,
                Quantity = quantity,
                UnitPrice = unitPrice
            };
        }

        internal void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0) throw new Exception("Quantity must be > 0");
            Quantity = newQuantity;
        }
    }
}
