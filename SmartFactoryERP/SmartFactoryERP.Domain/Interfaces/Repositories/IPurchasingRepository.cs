using SmartFactoryERP.Domain.Entities.Purchasing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Interfaces.Repositories
{
    public interface IPurchasingRepository
    {
        // دوال خاصة بالموردين (Suppliers)
        Task<Supplier> GetSupplierByIdAsync(int id, CancellationToken cancellationToken);

        Task AddSupplierAsync(Supplier supplier, CancellationToken cancellationToken);

        Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken);

        Task<List<Supplier>> GetAllSuppliersAsync(CancellationToken cancellationToken);
        Task AddPurchaseOrderAsync(PurchaseOrder order, CancellationToken cancellationToken);
        Task<bool> IsSupplierCodeUniqueAsync(string code, CancellationToken cancellationToken);
        Task<PurchaseOrder> GetPurchaseOrderWithItemsAsync(int id, CancellationToken cancellationToken);
        Task<List<PurchaseOrder>> GetAllPurchaseOrdersAsync(CancellationToken cancellationToken);
        Task AddGoodsReceiptAsync(GoodsReceipt receipt, CancellationToken cancellationToken);
        Task<GoodsReceipt> GetGoodsReceiptWithItemsAsync(int id, CancellationToken cancellationToken);
    }
}
