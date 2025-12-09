using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFactoryERP.Application.Features.Production.Commands.CompleteProduction;
using SmartFactoryERP.Application.Features.Production.Commands.CreateBOM;
using SmartFactoryERP.Application.Features.Production.Commands.CreateProductionOrder;
using SmartFactoryERP.Application.Features.Production.Commands.StartProduction;
using SmartFactoryERP.Application.Features.Production.Queries.GetProductionOrderById;
using SmartFactoryERP.Application.Features.Production.Queries.GetProductionOrders;
using System.Threading.Tasks;

namespace SmartFactoryERP.WebAPI.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductionController : BaseApiController
    {
        // ✅ GET: api/v1/production/orders - جديد
        [HttpGet("orders")]
        public async Task<IActionResult> GetProductionOrders([FromQuery] string status = null)
        {
            var query = new GetProductionOrdersQuery { Status = status };
            var orders = await Mediator.Send(query);
            return Ok(orders);
        }

        // ✅ GET: api/v1/production/orders/{id} - جديد
        [HttpGet("orders/{id}")]
        public async Task<IActionResult> GetProductionOrderById(int id)
        {
            var query = new GetProductionOrderByIdQuery { Id = id };
            var order = await Mediator.Send(query);
            
            if (order == null)
                return NotFound(new { message = $"Production order with ID {id} not found" });

            return Ok(order);
        }

        // POST api/v1/production/bom
        [HttpPost("bom")]
        public async Task<IActionResult> CreateBillOfMaterial([FromBody] CreateBillOfMaterialCommand command)
        {
            var componentsCount = await Mediator.Send(command);
            return Ok(new 
            { 
                message = $"{componentsCount} component(s) added to BOM successfully",
                productId = command.ProductId,
                componentsAdded = componentsCount
            });
        }

        // POST api/v1/production/orders
        [HttpPost("orders")]
        public async Task<IActionResult> CreateProductionOrder([FromBody] CreateProductionOrderCommand command)
        {
            var orderId = await Mediator.Send(command);
            return Ok(new { id = orderId, message = "Production order created successfully" });
        }

        // POST api/v1/production/orders/{id}/start
        [HttpPost("orders/{id}/start")]
        public async Task<IActionResult> StartProduction(int id)
        {
            await Mediator.Send(new StartProductionCommand { Id = id });
            return Ok(new { message = "Production started successfully" });
        }

        // POST api/v1/production/orders/{id}/complete
        [HttpPost("orders/{id}/complete")]
        public async Task<IActionResult> CompleteProduction(int id)
        {
            await Mediator.Send(new CompleteProductionCommand { Id = id });
            return Ok(new { message = "Production completed successfully" });
        }
      

        // ✅ DELETE: api/v1/production/orders/{id} - جديد (إلغاء أمر إنتاج)
        //[HttpDelete("orders/{id}")]
        //public async Task<IActionResult> CancelProductionOrder(int id)
        //{
        //    await Mediator.Send(new CancelProductionOrderCommand { Id = id });
        //    return NoContent();
        //}
    }
}