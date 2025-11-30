using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartFactoryERP.Application.Features.Purchasing.Commands.ConfirmPurchaseOrder;
using SmartFactoryERP.Application.Features.Purchasing.Commands.CreateGoodsReceipt;
using SmartFactoryERP.Application.Features.Purchasing.Commands.CreatePurchaseOrder;
using SmartFactoryERP.Application.Features.Purchasing.Commands.CreateSupplier;
using SmartFactoryERP.Application.Features.Purchasing.Commands.DeactivateSupplier;
using SmartFactoryERP.Application.Features.Purchasing.Commands.UpdateSupplier;
using SmartFactoryERP.Application.Features.Purchasing.Queries.GetAllPurchaseOrders;
using SmartFactoryERP.Application.Features.Purchasing.Queries.GetGoodsReceiptById;
using SmartFactoryERP.Application.Features.Purchasing.Queries.GetPurchaseOrderById;
using SmartFactoryERP.Application.Features.Purchasing.Queries.GetSupplierById;
using SmartFactoryERP.Application.Features.Purchasing.Queries.GetSuppliers;

namespace SmartFactoryERP.WebAPI.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PurchasingController : BaseApiController
    {
        // --- Suppliers ---

        [HttpPost("suppliers")]  
        public async Task<IActionResult> CreateSupplier([FromBody] CreateSupplierCommand command) // tested
        {
            var supplierId = await Mediator.Send(command);
            return Ok(supplierId);
        }
        // GET api/v1/purchasing/suppliers (Get All)
        [HttpGet("suppliers")]
        public async Task<IActionResult> GetSuppliersList()  //tested
        {
            var suppliersList = await Mediator.Send(new GetSuppliersQuery());
            return Ok(suppliersList);
        }

        // GET api/v1/purchasing/suppliers/{id} (Get By ID)
        [HttpGet("suppliers/{id}")]
        public async Task<IActionResult> GetSupplierById(int id) // tested
        {
            var query = new GetSupplierByIdQuery { Id = id };
            var supplierDto = await Mediator.Send(query);

            return Ok(supplierDto);
        }
        // PUT api/v1/purchasing/suppliers/{id} (Update) 
        [HttpPut("suppliers/{id}")]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] UpdateSupplierCommand command) // tested
        {
            // 1. Set the ID from the route for the handler
            if (id != command.Id)
            {
                return BadRequest("ID mismatch in route and body.");
            }

            // 2. Send the command
            await Mediator.Send(command);

            // 3. Return 204 No Content for successful update
            return NoContent();
        }
        // DELETE api/v1/purchasing/suppliers/{id} (Deactivate/Soft Delete)
        [HttpDelete("suppliers/{id}")]
        public async Task<IActionResult> DeactivateSupplier(int id) // tested
        {
            // 1. Send the command with the ID from the route
            await Mediator.Send(new DeactivateSupplierCommand { Id = id }); 

            // 2. Return 204 No Content
            return NoContent();
        }
        // POST api/v1/purchasing/orders (Create PO)
        [HttpPost("orders")]
        public async Task<IActionResult> CreatePurchaseOrder([FromBody] CreatePurchaseOrderCommand command) // tested
        {
            var orderId = await Mediator.Send(command);
            // Returns 201 Created with the ID of the new order
            return CreatedAtAction(nameof(GetPurchaseOrderById), new { id = orderId }, orderId);
        }

        // GET api/v1/purchasing/orders/{id}
        [HttpGet("orders/{id}")]
        public async Task<IActionResult> GetPurchaseOrderById(int id) // tested
        {
            var query = new GetPurchaseOrderByIdQuery { Id = id };
            var orderDto = await Mediator.Send(query);

            return Ok(orderDto);
        }
        [HttpGet("orders")]
        public async Task<IActionResult> GetAllPurchaseOrders() // tested
        {
            var query = new GetAllPurchaseOrdersQuery();
            var ordersList = await Mediator.Send(query);

            return Ok(ordersList);
        }
        [HttpPost("goods-receipt")]
        public async Task<IActionResult> CreateGoodsReceipt([FromBody] CreateGoodsReceiptCommand command)
        {
            var receiptId = await Mediator.Send(command);
            // Returns 201 Created
            return CreatedAtAction(null, new { id = receiptId }, receiptId);
        }
        // GET api/v1/purchasing/goods-receipts/{id}
        [HttpGet("goods-receipts/{id}")]
        public async Task<IActionResult> GetGoodsReceiptById(int id)
        {
            var query = new GetGoodsReceiptByIdQuery { Id = id };
            var receiptDto = await Mediator.Send(query);

            return Ok(receiptDto);
        }
        // --- NEW ENDPOINT (Action) ---
        // POST api/v1/purchasing/orders/{id}/confirm
        [HttpPost("orders/{id}/confirm")]
        public async Task<IActionResult> ConfirmPurchaseOrder(int id)   // tested 
        {
            // 1. Send the command
            await Mediator.Send(new ConfirmPurchaseOrderCommand { Id = id });

            // 2. Return 204 No Content (Action successful)
            return NoContent();
        }
    } 
}
