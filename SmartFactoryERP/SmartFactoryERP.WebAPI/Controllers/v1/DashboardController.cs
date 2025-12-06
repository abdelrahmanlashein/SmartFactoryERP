using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartFactoryERP.Application.Features.Analytics.Queries.GetDashboardChartsQuery;
using SmartFactoryERP.Application.Features.Analytics.Queries.GetDashboardStats;

namespace SmartFactoryERP.WebAPI.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DashboardController : BaseApiController
    {
        [HttpGet("stats")]
        public async Task<IActionResult> GetDashboardStats() //tested
        {
            var stats = await Mediator.Send(new GetDashboardStatsQuery());
            return Ok(stats);
        }
        [HttpGet("charts")]
        public async Task<IActionResult> GetDashboardCharts()
        {
            return Ok(await Mediator.Send(new GetDashboardChartsQuery()));
        }
    } 
}
