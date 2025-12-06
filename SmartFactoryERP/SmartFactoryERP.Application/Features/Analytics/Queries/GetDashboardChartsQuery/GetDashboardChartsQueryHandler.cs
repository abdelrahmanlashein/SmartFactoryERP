using MediatR;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using SmartFactoryERP.Domain.Models.Analytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Analytics.Queries.GetDashboardChartsQuery
{
    public class GetDashboardChartsQueryHandler : IRequestHandler<GetDashboardChartsQuery, DashboardChartsDto>
    {
        private readonly IAnalyticsRepository _repo;

        public GetDashboardChartsQueryHandler(IAnalyticsRepository repo) => _repo = repo;

        public async Task<DashboardChartsDto> Handle(GetDashboardChartsQuery request, CancellationToken token)
        {
            // تنفيذ متسلسل لتجنب مشاكل الـ DbContext
            var trend = await _repo.GetSalesTrendAsync(token);
            var topProducts = await _repo.GetTopSellingProductsAsync(token);
            var statusDist = await _repo.GetOrdersStatusDistributionAsync(token);

            return new DashboardChartsDto
            {
                SalesTrend = trend,
                TopProducts = topProducts,
                OrdersStatus = statusDist
            };
        }
    }
}
