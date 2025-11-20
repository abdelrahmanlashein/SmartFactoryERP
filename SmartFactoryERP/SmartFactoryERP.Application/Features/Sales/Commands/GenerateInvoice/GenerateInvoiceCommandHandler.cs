using MediatR;
using SmartFactoryERP.Domain.Entities.Sales;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Sales.Commands.GenerateInvoice
{
    public class GenerateInvoiceCommandHandler : IRequestHandler<GenerateInvoiceCommand, int>
    {
        private readonly ISalesRepository _salesRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GenerateInvoiceCommandHandler(ISalesRepository salesRepository, IUnitOfWork unitOfWork)
        {
            _salesRepository = salesRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(GenerateInvoiceCommand request, CancellationToken cancellationToken)
        {
            // 1. Fetch Sales Order with Items (to calculate total if needed, though TotalAmount is computed)
            var order = await _salesRepository.GetSalesOrderWithItemsAsync(request.SalesOrderId, cancellationToken);

            if (order == null)
                throw new Exception($"Sales Order {request.SalesOrderId} not found.");

            // 2. Create Invoice Entity
            var invoice = Invoice.Create(
                order.Id,
                order.TotalAmount, // Using the computed total from the order
                request.DueDate
            );

            // 3. Update Order Status
            order.MarkAsInvoiced();

            // 4. Persist
            await _salesRepository.AddInvoiceAsync(invoice, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return invoice.Id;
        }
    }
}
