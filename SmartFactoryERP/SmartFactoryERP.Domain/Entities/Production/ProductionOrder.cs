using SmartFactoryERP.Domain.Entities.Inventory;
using SmartFactoryERP.Domain.Entities.Shared;
using SmartFactoryERP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartFactoryERP.Domain.Entities.Production
{
    public class ProductionOrder : BaseAuditableEntity, IAggregateRoot
    {
        public string OrderNumber { get; private set; }
        public int ProductId { get; private set; }
        public int Quantity { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public ProductionStatus Status { get; private set; }
        public string Notes { get; private set; }

        // ✅ 1. خاصية الأولوية (جديدة)
        public string Priority { get; private set; }

        public virtual Material Product { get; private set; }

        // ✅ 2. قائمة الخامات (عشان الـ Include يشتغل وميديش Error)
        private readonly List<ProductionOrderItem> _items = new();
        public virtual IReadOnlyCollection<ProductionOrderItem> Items => _items.AsReadOnly();

        private ProductionOrder() { }

        public static ProductionOrder Create(int productId, int quantity, DateTime startDate, string notes, string priority = "Medium")
        {
            if (quantity <= 0) throw new Exception("Production quantity must be > 0.");

            return new ProductionOrder
            {
                OrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}",
                ProductId = productId,
                Quantity = quantity,
                StartDate = startDate,
                Status = ProductionStatus.Planned,
                Notes = notes,
                Priority = priority, // ✅ تعيين الأولوية
                CreatedDate = DateTime.UtcNow
            };
        }

        // ✅ 3. دالة لإضافة أو تحديث خامة (مهمة عشان الـ Wizard يملأ الخامات)
        public void AddOrUpdateItem(int materialId, decimal quantity)
        {
            var existingItem = _items.FirstOrDefault(i => i.MaterialId == materialId);
            if (existingItem != null)
            {
                existingItem.UpdateQuantity(quantity);
            }
            else
            {
                _items.Add(new ProductionOrderItem(materialId, quantity));
            }
        }

        public void StartProduction()
        {
            if (Status != ProductionStatus.Planned)
                throw new Exception("Can only start planned orders.");

            Status = ProductionStatus.Started;
        }

        public void CompleteProduction()
        {
            if (Status != ProductionStatus.Started)
                throw new Exception("Can only complete started orders.");

            Status = ProductionStatus.Completed;
            EndDate = DateTime.UtcNow;
        }
    }
}