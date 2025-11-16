using SmartFactoryERP.Domain.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Interfaces.Repositories
{
    public interface IInventoryRepository
    {
        Task AddMaterialAsync(Material material, CancellationToken cancellationToken);
        Task<Material?> GetMaterialByCodeAsync(string code, CancellationToken cancellationToken);
        Task<Material?> GetMaterialByIdAsync(int id, CancellationToken cancellationToken);

        Task AddStockTransactionAsync(StockTransaction transaction, CancellationToken cancellationToken);
    }
}
