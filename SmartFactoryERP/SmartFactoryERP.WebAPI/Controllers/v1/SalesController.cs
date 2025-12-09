using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartFactoryERP.Application.Features.Sales.Commands.ConfirmSalesOrder;
using SmartFactoryERP.Application.Features.Sales.Commands.CreateCustomer;
using SmartFactoryERP.Application.Features.Sales.Commands.CreateSalesOrder;
using SmartFactoryERP.Application.Features.Sales.Commands.GenerateInvoice;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using SmartFactoryERP.Infrastructure.Services.Pdf;

namespace SmartFactoryERP.WebAPI.Controllers.v1
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class SalesController : BaseApiController
    {
        private readonly ISalesRepository _salesRepository;
        private readonly PdfService _pdfService;
        // POST api/v1/sales/customers
        public SalesController(ISalesRepository salesRepository, PdfService pdfService)
        {
            _salesRepository = salesRepository;
            _pdfService = pdfService;
        }

        // GET api/v1/sales/customers
        [HttpGet("customers")]
        public async Task<IActionResult> GetCustomers(CancellationToken cancellationToken)
        {
            var customers = await _salesRepository.GetAllCustomersAsync(cancellationToken);
            return Ok(customers);
        }

        [HttpPost("customers")]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerCommand command) //tested
        {
            var customerId = await Mediator.Send(command);
            return Ok(customerId);
        }
        #region Test
       

     

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
        // GET api/v1/sales/invoices/{id}/pdf
        [HttpGet("invoices/{id}/pdf")]
        public async Task<IActionResult> DownloadInvoicePdf(int id)
        {
            // 1. هات البيانات (يفضل استخدام Mediator Query)
            // InvoiceDto invoice = await Mediator.Send(new GetInvoiceByIdQuery { Id = id });
            // هنا سنفترض أننا جبنا الـ Entity مباشرة للسرعة (عدلها لتستخدم الـ Mediator)
            var invoice = await _salesRepository.GetInvoiceByIdAsync(id, CancellationToken.None);

            if (invoice == null) return NotFound("Invoice not found");

            // 2. حولها لـ PDF
            var pdfBytes = _pdfService.GenerateInvoicePdf(invoice);

            // 3. رجع الملف
            return File(pdfBytes, "application/pdf", $"Invoice-{invoice.InvoiceNumber}.pdf");
        }
    }
}
