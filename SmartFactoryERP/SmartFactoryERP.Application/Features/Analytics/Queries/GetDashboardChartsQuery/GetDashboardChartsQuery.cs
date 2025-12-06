using MediatR;
using SmartFactoryERP.Domain.Models.Analytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Analytics.Queries.GetDashboardChartsQuery
{
    public class GetDashboardChartsQuery : IRequest<DashboardChartsDto> { }
}
