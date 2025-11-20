using SmartFactoryERP.Domain.Entities.Inventory;
using SmartFactoryERP.Domain.Entities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SmartFactoryERP.Domain.Entities.Sales
{
    public class SalesOrderItem : BaseEntity
    {
        public int SalesOrderId { get; private set; }
        public int MaterialId { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        // Computed property
        public decimal TotalPrice => Quantity * UnitPrice;

        // Navigation Props
        public virtual SalesOrder SalesOrder { get; private set; }
        public virtual Material Material { get; private set; }

        private SalesOrderItem() { }

        internal static SalesOrderItem Create(int materialId, int quantity, decimal unitPrice)
        {
            if (quantity <= 0) throw new Exception("Quantity must be > 0");

            return new SalesOrderItem
            {
                MaterialId = materialId,
                Quantity = quantity,
                UnitPrice = unitPrice
            };
        }
    }
}
