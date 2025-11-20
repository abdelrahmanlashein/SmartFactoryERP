using SmartFactoryERP.Domain.Entities.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Interfaces.Repositories
{
    public interface IProductionRepository
    {
        // BOM Methods
        Task AddBillOfMaterialAsync(BillOfMaterial bom, CancellationToken cancellationToken);
        Task<List<BillOfMaterial>> GetBOMForProductAsync(int productId, CancellationToken cancellationToken);

        // Production Order Methods
        Task AddProductionOrderAsync(ProductionOrder order, CancellationToken cancellationToken);
        Task<ProductionOrder> GetProductionOrderByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<ProductionOrder>> GetAllProductionOrdersAsync(CancellationToken cancellationToken);
    }
}
