using SmartFactoryERP.Domain.Entities.Shared;
using SmartFactoryERP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.Inventory
{
  
    
        public class Material : BaseAuditableEntity, IAggregateRoot
        {
            public string MaterialCode { get; private set; }
            public string MaterialName { get; private set; }
            public MaterialType MaterialType { get; private set; }
            public string UnitOfMeasure { get; private set; }
            public decimal UnitPrice { get; private set; }
            public int MinimumStockLevel { get; private set; }
            public int CurrentStockLevel { get; private set; }

            // لإجبار EF Core على استخدامه
            private Material() { }

            // "Factory Method" لضمان إنشاء سليم
            public static Material CreateNew(
                string code,
                string name,
                MaterialType type,
                string uom,
                decimal unitPrice,
                int minStock)
            {
                // هنا تضع قواعد البيزنس
                if (string.IsNullOrWhiteSpace(code))
                    throw new Exception("Material Code is required."); // استخدم DomainException المخصص

                if (unitPrice < 0)
                    throw new Exception("Unit Price cannot be negative.");

                var material = new Material
                {
                    MaterialCode = code,
                    MaterialName = name,
                    MaterialType = type,
                    UnitOfMeasure = uom,
                    UnitPrice = unitPrice,
                    MinimumStockLevel = minStock,
                    CurrentStockLevel = 0 // القاعدة: أي مادة جديدة رصيدها صفر
                };
                return material;
            }

            // Business Logic Methods
            public void UpdatePrice(decimal newPrice)
            {
                if (newPrice < 0)
                    throw new Exception("Unit Price cannot be negative.");

                UnitPrice = newPrice;
            }

            public void IncreaseStock(int quantity)
            {
                if (quantity <= 0)
                    throw new Exception("Quantity to increase must be positive.");

                CurrentStockLevel += quantity;
            }

            public void DecreaseStock(int quantity)
            {
                if (quantity <= 0)
                    throw new Exception("Quantity to decrease must be positive.");

                if (CurrentStockLevel - quantity < 0)
                    throw new Exception("Insufficient stock."); // استخدم InsufficientStockException

                CurrentStockLevel -= quantity;
            }
        }
    }

