using SmartFactoryERP.Domain.Entities.Inventory;
using SmartFactoryERP.Domain.Entities.Shared;
using SmartFactoryERP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.Production
{

    public class ProductionOrder : BaseAuditableEntity, IAggregateRoot
    {
        public string OrderNumber { get; private set; }
        public int ProductId { get; private set; } // ماذا سنصنع؟
        public int Quantity { get; private set; } // كم قطعة؟
        public DateTime StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public ProductionStatus Status { get; private set; }
        public string Notes { get; private set; }

        public virtual Material Product { get; private set; }

        private ProductionOrder() { }

        public static ProductionOrder Create(int productId, int quantity, DateTime startDate, string notes)
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
                CreatedDate = DateTime.UtcNow
            };
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
