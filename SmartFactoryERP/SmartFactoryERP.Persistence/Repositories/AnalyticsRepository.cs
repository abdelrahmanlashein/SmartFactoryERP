using Microsoft.EntityFrameworkCore;
using SmartFactoryERP.Domain.Enums;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using SmartFactoryERP.Domain.Models.Analytics;
using SmartFactoryERP.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Persistence.Repositories
{
    public class AnalyticsRepository : IAnalyticsRepository
    {
        private readonly ApplicationDbContext _context;

        public AnalyticsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetTotalMaterialsAsync(CancellationToken token)
        {
            return await _context.Materials.CountAsync(token);
        }

        public async Task<int> GetLowStockCountAsync(CancellationToken token)
        {
            // Count materials where Current < Min
            return await _context.Materials
                .CountAsync(m => m.CurrentStockLevel <= m.MinimumStockLevel, token);
        }

        public async Task<int> GetPendingSalesCountAsync(CancellationToken token)
        {
            return await _context.SalesOrders
                .CountAsync(s => s.Status == SalesOrderStatus.Confirmed, token);
        }

        public async Task<decimal> GetPendingSalesRevenueAsync(CancellationToken token)
        {
            // Note: TotalAmount is a computed property in Domain (not stored in DB in our basic setup), 
            // so we might need to calculate it via Sum(Items) or if we stored it, sum it directly.
            // Assuming we stored it or doing a simple join:
            // Let's sum the items directly for accuracy:
            return await _context.SalesOrderItems
                .Where(i => i.SalesOrder.Status == SalesOrderStatus.Confirmed)
                .SumAsync(i => i.Quantity * i.UnitPrice, token);
        }

        public async Task<int> GetActiveProductionCountAsync(CancellationToken token)
        {
            return await _context.ProductionOrders
                .CountAsync(p => p.Status == ProductionStatus.Started || p.Status == ProductionStatus.Planned, token);
        }
        public async Task<decimal> GetTotalExpensesAsync(CancellationToken token)
        {
            // جمع كل المبالغ في جدول المصروفات
            return await _context.Expenses.SumAsync(e => e.Amount, token);
        }
        public async Task<List<DailySalesDto>> GetSalesTrendAsync(CancellationToken token)
        {
            // نجمع المبيعات حسب اليوم لآخر 7 أيام
            var sevenDaysAgo = DateTime.UtcNow.AddDays(-7);

            return await _context.Invoices
                .Where(i => i.InvoiceDate >= sevenDaysAgo)
                .GroupBy(i => i.InvoiceDate.Date)
                .Select(g => new DailySalesDto
                {
                    Date = g.Key,
                    TotalAmount = g.Sum(x => x.TotalAmount)
                })
                .OrderBy(x => x.Date)
                .ToListAsync(token);
        }

        public async Task<List<TopProductDto>> GetTopSellingProductsAsync(CancellationToken token)
        {
            // أكثر 5 منتجات مبيعاً
            return await _context.SalesOrderItems
                .GroupBy(i => i.Material.MaterialName)
                .Select(g => new TopProductDto
                {
                    ProductName = g.Key,
                    QuantitySold = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.QuantitySold)
                .Take(5)
                .ToListAsync(token);
        }

        public async Task<List<OrderStatusDto>> GetOrdersStatusDistributionAsync(CancellationToken token)
        {
            // عدد الطلبات لكل حالة
            return await _context.SalesOrders
                .GroupBy(o => o.Status)
                .Select(g => new OrderStatusDto
                {
                    Status = g.Key.ToString(),
                    Count = g.Count()
                })
                .ToListAsync(token);
        }

        public async Task<List<LowStockMaterialDto>> GetCriticalRawMaterialsAsync(int count, CancellationToken token)
        {
            return await _context.Materials
                // 1. فلتر: نوعها خامة + الرصيد أقل من أو يساوي حد الطلب
                .Where(m => m.MaterialType == MaterialType.RawMaterial && m.CurrentStockLevel <= m.MinimumStockLevel)
                .OrderBy(m => m.CurrentStockLevel) // الأقل رصيداً يظهر أولاً
                .Take(count) // هات أهم 5 مثلاً
                .Select(m => new LowStockMaterialDto
                {
                    MaterialId = m.Id,
                    Name = m.MaterialName,
                    CurrentStock = m.CurrentStockLevel,
                    ReorderLevel = m.MinimumStockLevel,
                    Unit = m.UnitOfMeasure
                })
                .AsNoTracking()
                .ToListAsync(token);
        }
    }
}
