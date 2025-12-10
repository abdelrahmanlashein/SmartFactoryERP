using Microsoft.EntityFrameworkCore;
using SmartFactoryERP.Domain.Entities.Production;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using SmartFactoryERP.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Persistence.Repositories
{
    public class ProductionRepository : IProductionRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- Bill of Materials (BOM) ---

        public async Task AddBillOfMaterialAsync(BillOfMaterial bom, CancellationToken cancellationToken)
        {
            await _context.BillOfMaterials.AddAsync(bom, cancellationToken);
        }

        public async Task<List<BillOfMaterial>> GetBOMForProductAsync(int productId, CancellationToken cancellationToken)
        {
            return await _context.BillOfMaterials
                .Where(b => b.ProductId == productId)
                .Include(b => b.Component) // مهم: عشان نجيب اسم المادة الخام وتفاصيلها
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        // --- Production Orders ---

        public async Task AddProductionOrderAsync(ProductionOrder order, CancellationToken cancellationToken)
        {
            await _context.ProductionOrders.AddAsync(order, cancellationToken);
        }

        public async Task<ProductionOrder> GetProductionOrderByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.ProductionOrders
                .Include(p => p.Product) // عشان نعرف إحنا بنصنع إيه (اسم المنتج)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<List<ProductionOrder>> GetAllProductionOrdersAsync(CancellationToken cancellationToken)
        {
            return await _context.ProductionOrders
                .Include(p => p.Product)
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedDate) // الأحدث أولاً
                .ToListAsync(cancellationToken);
        }
        public async Task<List<BillOfMaterial>> GetBomLinesByProductIdAsync(int productId)
        {
            return await _context.BillOfMaterials
                .Include(b => b.Component) // عشان نجيب اسم الخامة (MaterialName)
                .Where(b => b.ProductId == productId)
                .ToListAsync();
        }
        public async Task<ProductionOrder> GetOrderWithItemsAsync(int id)
        {
            return await _context.ProductionOrders
                .Include(o => o.Product)        // يجيب تفاصيل المنتج النهائي (اسمه)
                .Include(o => o.Items)          // ✅ يجيب قائمة الخامات (Items)
                    .ThenInclude(i => i.Material) // ✅ يجيب اسم الخامة نفسها جوه كل سطر
                .FirstOrDefaultAsync(o => o.Id == id);
        }
        public async Task AddAsync(ProductionOrder order)
        {
            await _context.ProductionOrders.AddAsync(order);
            // لاحظ: SaveChanges بيتعمل في الـ UnitOfWork، فمش محتاجينه هنا
        }
    }
}
