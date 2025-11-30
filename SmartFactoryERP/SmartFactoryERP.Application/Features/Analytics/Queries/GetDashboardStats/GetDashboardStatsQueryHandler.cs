using MediatR;
using SmartFactoryERP.Domain.Interfaces.Repositories; // ✅ تصحيح الـ Namespace
using SmartFactoryERP.Domain.Interfaces.Repositories.SmartFactoryERP.Domain.Interfaces.Repositories;
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
            // ✅ الحل: تنفيذ الاستعلامات واحداً تلو الآخر (Sequential)
            // هذا يمنع تداخل العمليات على الـ DbContext

            var totalMaterials = await _analyticsRepository.GetTotalMaterialsAsync(cancellationToken);
            var lowStock = await _analyticsRepository.GetLowStockCountAsync(cancellationToken);
            var salesCount = await _analyticsRepository.GetPendingSalesCountAsync(cancellationToken);
            var revenue = await _analyticsRepository.GetPendingSalesRevenueAsync(cancellationToken);
            var productionCount = await _analyticsRepository.GetActiveProductionCountAsync(cancellationToken);

            return new DashboardStatsDto
            {
                TotalMaterialsCount = totalMaterials,
                LowStockItemsCount = lowStock,
                PendingSalesOrders = salesCount,
                PotentialRevenue = revenue,
                ActiveProductionOrders = productionCount
            };
        }
    }
}