using Microsoft.EntityFrameworkCore;
using SmartFactoryERP.Domain.Entities.Purchasing;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using SmartFactoryERP.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Persistence.Repositories
{
    public class PurchasingRepository : IPurchasingRepository
    {
        private readonly ApplicationDbContext _context;

        public PurchasingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- Supplier Methods ---

        public async Task AddSupplierAsync(Supplier supplier, CancellationToken cancellationToken)
        {
            await _context.Suppliers.AddAsync(supplier, cancellationToken);
        }

        public async Task<Supplier> GetSupplierByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Suppliers
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<List<Supplier>> GetAllSuppliersAsync(CancellationToken cancellationToken)
        {
            return await _context.Suppliers
                .AsNoTracking() // For read-only performance
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> IsSupplierCodeUniqueAsync(string code, CancellationToken cancellationToken)
        {
            // Returns TRUE if the code does NOT exist (it is unique)
            // Returns FALSE if the code already exists
            return !await _context.Suppliers.AnyAsync(s => s.SupplierCode == code, cancellationToken);
        }

        // --- Purchase Order Methods (Placeholder for now) ---

        public async Task AddPurchaseOrderAsync(PurchaseOrder order, CancellationToken cancellationToken)
        {
            await _context.PurchaseOrders.AddAsync(order, cancellationToken);
        }

        public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken)
        {
            // لو الإيميل فاضي، اعتبره فريد (لأننا سمحنا بإيميل فارغ في الـ Validator)
            if (string.IsNullOrEmpty(email)) return true;

            // رجع true لو الإيميل مش موجود في الداتابيز
            return !await _context.Suppliers.AnyAsync(s => s.Email == email, cancellationToken);
        }
        // --- NEW IMPLEMENTATION ---
        public async Task<PurchaseOrder> GetPurchaseOrderWithItemsAsync(int id, CancellationToken cancellationToken)
        {
            // Use Include() to load the collection of items and the Supplier navigation property
            return await _context.PurchaseOrders
                .Include(po => po.Items)
                .Include(po => po.Supplier) // Assuming Supplier is needed for SupplierName in DTO
                //.AsNoTracking() // For read-only query performance
                .FirstOrDefaultAsync(po => po.Id == id, cancellationToken);
        }
        // --- NEW IMPLEMENTATION ---
        public async Task<List<PurchaseOrder>> GetAllPurchaseOrdersAsync(CancellationToken cancellationToken)
        {
            // Fetch orders and include the Supplier to get the SupplierName for the list DTO
            return await _context.PurchaseOrders
                .Include(po => po.Supplier) // To get the name for the list
                .AsNoTracking()
                .OrderByDescending(po => po.OrderDate)
                .ToListAsync(cancellationToken);
        }
        public async Task AddGoodsReceiptAsync(GoodsReceipt receipt, CancellationToken cancellationToken)
        {
            // Note: PurchaseOrders property name in DbContext is usually PurchaseOrders or GoodsReceipts
            await _context.GoodsReceipts.AddAsync(receipt, cancellationToken);
        }
        public async Task<GoodsReceipt> GetGoodsReceiptWithItemsAsync(int id, CancellationToken cancellationToken)
        {
            // Use Include() to load the nested collection of items
            return await _context.GoodsReceipts
                .Include(gr => gr.Items)
                .AsNoTracking() // For read-only query performance
                .FirstOrDefaultAsync(gr => gr.Id == id, cancellationToken);
        }
    }
}
