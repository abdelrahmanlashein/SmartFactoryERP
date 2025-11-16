using SmartFactoryERP.Domain.Entities.Shared;
using SmartFactoryERP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.Inventory
{

    
    public class StockTransaction:BaseEntity
    {
        public new long Id { get; private set; } // (PK)

        public int MaterialID { get; private set; } // (FK)
        public Material Material { get; private set; } // Navigation Property

        public TransactionType TransactionType { get; private set; }
        public int Quantity { get; private set; } // موجب للإضافة، سالب للصرف
        public DateTime TransactionDate { get; private set; }
        public long? ReferenceID { get; private set; }
        public ReferenceType? ReferenceType { get; private set; }
        public string? Notes { get; private set; }

        private StockTransaction() { }

        public static StockTransaction Create(
            int materialId,
            TransactionType type,
            int quantity,
            ReferenceType? refType,
            long? refId,
            string? notes)
        {
            if (quantity == 0)
                throw new Exception("Transaction quantity cannot be zero.");

            return new StockTransaction
            {
                MaterialID = materialId,
                TransactionType = type,
                Quantity = quantity,
                ReferenceType = refType,
                ReferenceID = refId,
                Notes = notes,
                TransactionDate = DateTime.UtcNow // مؤقتاً، الأفضل حقن IDateTime
            };
        }
    }
}

