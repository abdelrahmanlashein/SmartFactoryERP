using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartFactoryERP.Application.Features.AI.Queries.GetProductForecast;

namespace SmartFactoryERP.WebAPI.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AIController : BaseApiController
    {
        // GET api/v1/ai/forecast/{productId}
        [HttpGet("forecast/{productId}")]
        public async Task<IActionResult> GetForecast(int productId)
        {
            var query = new GetProductForecastQuery { ProductId = productId };
            var result = await Mediator.Send(query);

            return Ok(result);
        }
    }
}
