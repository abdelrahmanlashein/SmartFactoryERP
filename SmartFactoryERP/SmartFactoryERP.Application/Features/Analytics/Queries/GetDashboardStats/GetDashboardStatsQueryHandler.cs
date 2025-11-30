using MediatR;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Analytics.Queries.GetDashboardStats
{
    public class GetDashboardStatsQueryHandler : IRequestHandler<GetDashboardStatsQuery, DashboardStatsDto>
    {
        private readonly IAnalyticsRepository _analyticsRepository;

        public GetDashboardStatsQueryHandler(IAnalyticsRepository analyticsRepository)
        {
            _analyticsRepository = analyticsRepository;
        }

        public async Task<DashboardStatsDto> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
        {
            // ✅ 1. التنفيذ المتسلسل (Sequential Execution)
            // لحل مشكلة "InvalidOperationException" مع EF Core

            var totalMaterials = await _analyticsRepository.GetTotalMaterialsAsync(cancellationToken);
            var lowStock = await _analyticsRepository.GetLowStockCountAsync(cancellationToken);
            var activeProduction = await _analyticsRepository.GetActiveProductionCountAsync(cancellationToken);

            // --- البيانات المالية ---
            var pendingSales = await _analyticsRepository.GetPendingSalesCountAsync(cancellationToken);
            var totalRevenue = await _analyticsRepository.GetPendingSalesRevenueAsync(cancellationToken);

            // 👇 الجديد: جلب المصروفات
            var totalExpenses = await _analyticsRepository.GetTotalExpensesAsync(cancellationToken);

            // 👇 الجديد: حساب صافي الربح
            var netProfit = totalRevenue - totalExpenses;

            // ✅ 2. إرجاع الـ DTO بالبيانات الكاملة
            return new DashboardStatsDto
            {
                TotalMaterialsCount = totalMaterials,
                LowStockItemsCount = lowStock,
                ActiveProductionOrders = activeProduction,

                PendingSalesOrders = pendingSales,
                TotalRevenue = totalRevenue,

                // الحقول الجديدة
                TotalExpenses = totalExpenses,
                NetProfit = netProfit
            };
        }
    }
}