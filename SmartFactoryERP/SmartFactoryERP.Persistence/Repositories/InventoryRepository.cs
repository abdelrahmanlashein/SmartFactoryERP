using Microsoft.EntityFrameworkCore;
using SmartFactoryERP.Domain.Entities.Inventory;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using SmartFactoryERP.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task AddMaterialAsync(Material material, CancellationToken cancellationToken)
        {
            // فقط أضفه للـ context. الـ UnitOfWork هو من سيحفظ.
            await _context.Materials.AddAsync(material, cancellationToken);
        }

        public async Task<Material?> GetMaterialByCodeAsync(string code, CancellationToken cancellationToken)
        {
            return await _context.Materials
                .FirstOrDefaultAsync(m => m.MaterialCode == code, cancellationToken);
        }

        public async Task<Material?> GetMaterialByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Materials.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task AddStockTransactionAsync(StockTransaction transaction, CancellationToken cancellationToken)
        {
            await _context.StockTransactions.AddAsync(transaction, cancellationToken);
        }
        public async Task<List<Material>> GetAllMaterialsAsync(CancellationToken cancellationToken)
        {
            return await _context.Materials
                .AsNoTracking() // مهم: للقراءة فقط عشان الأداء
                .OrderBy(m => m.MaterialCode) // نرتبهم مثلاً
                .ToListAsync(cancellationToken);
        }
        // --- التنفيذ الجديد ---
        public async Task<List<StockTransaction>> GetTransactionsForMaterialAsync(int materialId, CancellationToken cancellationToken)
        {
            return await _context.StockTransactions
                .Where(t => t.MaterialID == materialId) // فلترة بالمادة المحددة
                .AsNoTracking()
                .OrderByDescending(t => t.TransactionDate) // الأحدث أولاً
                .ToListAsync(cancellationToken);
        }
    }
}
