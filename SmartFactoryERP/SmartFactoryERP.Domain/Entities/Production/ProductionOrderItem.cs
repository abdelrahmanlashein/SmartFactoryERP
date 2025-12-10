using SmartFactoryERP.Domain.Entities.Inventory;
using SmartFactoryERP.Domain.Entities.Shared;
using System;

namespace SmartFactoryERP.Domain.Entities.Production
{
    // هذا الكلاس يمثل سطر خامة داخل أمر التصنيع (نسخة من الـ BOM خاصة بهذا الأوردر)
    public class ProductionOrderItem : BaseEntity
    {
        public int ProductionOrderId { get; private set; } // الأب (أمر التصنيع)
        public int MaterialId { get; private set; } // المادة الخام
        public decimal Quantity { get; private set; } // الكمية المطلوبة

        // Navigation Properties
        public virtual Material Material { get; private set; }

        // Constructor فارغ لـ EF Core
        private ProductionOrderItem() { }

        // Constructor للاستخدام
        public ProductionOrderItem(int materialId, decimal quantity)
        {
            if (quantity <= 0) throw new Exception("Item quantity must be positive.");

            MaterialId = materialId;
            Quantity = quantity;
        }

        // دالة لتحديث الكمية (لو المهندس حب يعدل الكمية قبل البدء)
        public void UpdateQuantity(decimal newQuantity)
        {
            if (newQuantity <= 0) throw new Exception("Quantity must be positive.");
            Quantity = newQuantity;
        }
    }
}