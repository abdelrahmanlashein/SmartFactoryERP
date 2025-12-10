using SmartFactoryERP.Domain.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading; // ✅ ضروري لاستخدام CancellationToken
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Interfaces.Repositories
{
    public interface IInventoryRepository
    {
        // ... (الدوال القديمة)
        Task AddMaterialAsync(Material material, CancellationToken cancellationToken);
        Task<Material?> GetMaterialByCodeAsync(string code, CancellationToken cancellationToken);
        Task<Material?> GetMaterialByIdAsync(int id, CancellationToken cancellationToken);

        Task AddStockTransactionAsync(StockTransaction transaction, CancellationToken cancellationToken);
        Task<List<Material>> GetAllMaterialsAsync(CancellationToken cancellationToken);
        Task<List<StockTransaction>> GetTransactionsForMaterialAsync(int materialId, CancellationToken cancellationToken);

        // ✅✅ الإضافة الجديدة: دالة خصم المخزون (Inventory Deduction) ✅✅
        Task DeductStockAsync(int materialId, decimal quantity, CancellationToken cancellationToken);
    }
}