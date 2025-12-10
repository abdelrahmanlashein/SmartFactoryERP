using SmartFactoryERP.Domain.Entities.Shared;
using SmartFactoryERP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.Inventory
{
    public class StockTransaction : BaseEntity
    {
        public new long Id { get; private set; }

        public int MaterialID { get; private set; }
        public Material Material { get; private set; }

        public TransactionType TransactionType { get; private set; }
        public decimal Quantity { get; private set; }
        public DateTime TransactionDate { get; private set; }
        public long? ReferenceID { get; private set; }
        public ReferenceType? ReferenceType { get; private set; }
        public string? Notes { get; set; }

        private StockTransaction() { }

        // 1. الدالة العامة (General Factory Method)
        public static StockTransaction Create(
            int materialId,
            TransactionType type,
            decimal quantity,
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
                TransactionDate = DateTime.UtcNow
            };
        }

        // ----------------------------------------------------------------------
        // ✅✅ دوال المصنع المتخصصة (المصححة) ✅✅
        // ----------------------------------------------------------------------

        /// <summary>
        /// Creates a transaction for receiving stock (positive movement).
        /// </summary>
        public static StockTransaction CreateReceipt(
            int materialId,
            decimal quantity,
            string notes,
            ReferenceType? refType = null,
            long? refId = null)
        {
            if (quantity <= 0)
                throw new ArgumentException("Receipt quantity must be positive.", nameof(quantity));

            // ✅✅ تم التصحيح هنا: تمرير قيمة الـ Enum بشكل صحيح ✅✅
            return Create(
                materialId,
                TransactionType.Purchase, // Use the correct enum value for receipt
                quantity,
                refType,
                refId,
                notes);
        }

        /// <summary>
        /// Creates a transaction for stock usage (negative movement).
        /// </summary>
        public static StockTransaction CreateUsage(
            int materialId,
            decimal quantity,
            string notes,
            ReferenceType? refType = null,
            long? refId = null)
        {
            if (quantity <= 0)
                throw new ArgumentException("Usage quantity must be positive.", nameof(quantity));

            // ✅✅ FIX: Use TransactionType.Consumption instead of TransactionType.Usage ✅✅
            return Create(
                materialId,
                TransactionType.Consumption,
                -quantity, // الكمية تكون سالبة (صرف)
                refType,
                refId,
                notes);
        }
    }
}