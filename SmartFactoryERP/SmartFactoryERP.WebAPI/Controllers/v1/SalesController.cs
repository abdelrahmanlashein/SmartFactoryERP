using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartFactoryERP.Application.Features.Sales.Commands.ConfirmSalesOrder;
using SmartFactoryERP.Application.Features.Sales.Commands.CreateCustomer;
using SmartFactoryERP.Application.Features.Sales.Commands.GenerateInvoice;
using SmartFactoryERP.Application.Features.Sales.CreateSalesOrder;

namespace SmartFactoryERP.WebAPI.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SalesController : BaseApiController
    {
        // POST api/v1/sales/customers
        [HttpPost("customers")]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerCommand command) //tested
        {
            var customerId = await Mediator.Send(command);
            return Ok(customerId);
        }
        #region Test
        ///* 
        //             * 🧪 Test Data for Postman
        //    Once you Rebuild and Run the project, test it immediately.

        //    Method: POST

        //    URL: https://localhost:7093/api/v1/sales/customers

        //    Body (JSON):

        //    JSON

        //    {
        //     "customerName": "Global Tech Industries",
        //     "email": "procurement@globaltech.com",
        //     "phoneNumber": "+1-555-0199",
        //     "address": "456 Innovation Blvd, Silicon Valley",
        //     "taxNumber": "TX-987654321",
        //     "creditLimit": 50000.00
        //    }
        //    Expected Result: 200 OK with the new ID.
        // */ 

        #endregion
        // POST api/v1/sales/orders
        [HttpPost("orders")]
        public async Task<IActionResult> CreateSalesOrder([FromBody] CreateSalesOrderCommand command) // tested
        {
            var orderId = await Mediator.Send(command);
            // مبدئياً نرجع Ok لحد ما نعمل الـ GetById
            return Ok(orderId);
        }
        #region Test
        // https://localhost:7093/api/v1/sales/orders
        //        {
        //  "customerId": 1,
        //  "items": [
        //    {
        //      "materialId": 1,
        //      "quantity": 10,
        //      "unitPrice": 1200.50
        //    },
        //    {
        //      "materialId": 1,
        //      "quantity": 5,
        //      "unitPrice": 1200.50
        //    }
        //  ]
        //}
        #endregion

        // POST api/v1/sales/orders/{id}/confirm
        [HttpPost("orders/{id}/confirm")]
        public async Task<IActionResult> ConfirmSalesOrder(int id) //500
        {
            await Mediator.Send(new ConfirmSalesOrderCommand { Id = id });
            return NoContent();
        }

        [HttpPost("invoices")]
        public async Task<IActionResult> GenerateInvoice([FromBody] GenerateInvoiceCommand command) //500
        {
            var invoiceId = await Mediator.Send(command);
            return Ok(invoiceId);
        }
    }
}
