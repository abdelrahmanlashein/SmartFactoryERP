using Microsoft.EntityFrameworkCore;
using SmartFactoryERP.Domain.Entities.Inventory;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using SmartFactoryERP.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartFactoryERP.Persistence.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly ApplicationDbContext _context;

        public InventoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // ... (الدوال القديمة)
        public async Task AddMaterialAsync(Material material, CancellationToken cancellationToken)
        {
            await _context.Materials.AddAsync(material, cancellationToken);
        }

        public async Task<Material?> GetMaterialByCodeAsync(string code, CancellationToken cancellationToken)
        {
            return await _context.Materials
                .FirstOrDefaultAsync(m => m.MaterialCode == code, cancellationToken);
        }

        public async Task<Material?> GetMaterialByIdAsync(int id, CancellationToken cancellationToken)
        {
            // ✅ مهم: لا نستخدم FindAsync هنا، بل نستخدم FirstOrDefaultAsync ونعمل تتبع (Tracking)
            // لكي يتم تحديث الرصيد الحالي (CurrentStockLevel)
            return await _context.Materials
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AddStockTransactionAsync(StockTransaction transaction, CancellationToken cancellationToken)
        {
            await _context.StockTransactions.AddAsync(transaction, cancellationToken);
        }

        public async Task<List<Material>> GetAllMaterialsAsync(CancellationToken cancellationToken)
        {
            return await _context.Materials
                .AsNoTracking()
                .OrderBy(m => m.MaterialCode)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<StockTransaction>> GetTransactionsForMaterialAsync(int materialId, CancellationToken cancellationToken)
        {
            return await _context.StockTransactions
                .Where(t => t.MaterialID == materialId)
                .AsNoTracking()
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync(cancellationToken);
        }

        // ----------------------------------------------------------------------
        // ✅✅ التنفيذ الجديد: DeductStockAsync (الخصم الفعلي للمخزون) ✅✅
        // ----------------------------------------------------------------------
        public async Task DeductStockAsync(int materialId, decimal quantity, CancellationToken cancellationToken)
        {
            // 1. جلب المادة مع تتبعها
            var material = await GetMaterialByIdAsync(materialId, cancellationToken);

            if (material == null)
            {
                throw new Exception($"Material with ID {materialId} not found in inventory.");
            }

            // 2. التحقق من الرصيد والخصم (Domain Logic)
            // هذه الدالة ستحدث CurrentStockLevel في الـ Material Entity وترمي Exception لو الرصيد غير كافٍ
            material.DecreaseStock(quantity);

            // 3. تسجيل حركة المخزون
            var transaction = StockTransaction.CreateUsage(
                materialId: material.Id,
                quantity: quantity,
                notes: $"Usage for Production Order"
            );

            await AddStockTransactionAsync(transaction, cancellationToken);

            // لا نحفظ التغييرات هنا، بل الـ UnitOfWork هو من سيتولى حفظ تحديث Material وتحديث StockTransaction.
        }
    }
}