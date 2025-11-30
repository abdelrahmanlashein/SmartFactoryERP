using Microsoft.EntityFrameworkCore;
using SmartFactoryERP.Domain.Enums;
using SmartFactoryERP.Domain.Interfaces.Repositories;
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
    }
}
