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
            return !await _context.Suppliers.AnyAsync(s => s.SupplierCode == code, cancellationToken);
        }

        public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(email)) return true;
            return !await _context.Suppliers.AnyAsync(s => s.Email == email, cancellationToken);
        }

        // --- Purchase Order Methods ---

        public async Task AddPurchaseOrderAsync(PurchaseOrder order, CancellationToken cancellationToken)
        {
            await _context.PurchaseOrders.AddAsync(order, cancellationToken);
        }

        // ✅✅ هذا هو التعديل المهم جداً ✅✅
        public async Task<PurchaseOrder> GetPurchaseOrderWithItemsAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.PurchaseOrders
                .Include(po => po.Supplier) // عشان نجيب اسم المورد
                .Include(po => po.Items)    // عشان نجيب قائمة الأصناف
                    .ThenInclude(i => i.Material) // 👈👈 الإضافة الحاسمة: عشان نجيب اسم وكود المادة داخل كل صنف
                .FirstOrDefaultAsync(po => po.Id == id, cancellationToken);
        }

        public async Task<List<PurchaseOrder>> GetAllPurchaseOrdersAsync(CancellationToken cancellationToken)
        {
            return await _context.PurchaseOrders
                .Include(po => po.Supplier)
                .AsNoTracking()
                .OrderByDescending(po => po.OrderDate)
                .ToListAsync(cancellationToken);
        }

        // --- Goods Receipt Methods ---

        public async Task AddGoodsReceiptAsync(GoodsReceipt receipt, CancellationToken cancellationToken)
        {
            await _context.GoodsReceipts.AddAsync(receipt, cancellationToken);
        }

        public async Task<GoodsReceipt> GetGoodsReceiptWithItemsAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.GoodsReceipts
                .Include(gr => gr.Items)
                .AsNoTracking()
                .FirstOrDefaultAsync(gr => gr.Id == id, cancellationToken);
        }
    }
}