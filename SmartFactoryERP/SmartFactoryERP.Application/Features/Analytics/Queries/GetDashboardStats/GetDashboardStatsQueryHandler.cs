using MediatR;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            // Execute queries in parallel for performance (Optional but good practice)
            var taskTotalMaterials = _analyticsRepository.GetTotalMaterialsAsync(cancellationToken);
            var taskLowStock = _analyticsRepository.GetLowStockCountAsync(cancellationToken);
            var taskSalesCount = _analyticsRepository.GetPendingSalesCountAsync(cancellationToken);
            var taskRevenue = _analyticsRepository.GetPendingSalesRevenueAsync(cancellationToken);
            var taskProduction = _analyticsRepository.GetActiveProductionCountAsync(cancellationToken);

            await Task.WhenAll(taskTotalMaterials, taskLowStock, taskSalesCount, taskRevenue, taskProduction);

            return new DashboardStatsDto
            {
                TotalMaterialsCount = taskTotalMaterials.Result,
                LowStockItemsCount = taskLowStock.Result,
                PendingSalesOrders = taskSalesCount.Result,
                PotentialRevenue = taskRevenue.Result,
                ActiveProductionOrders = taskProduction.Result
            };
        }
    }
}
