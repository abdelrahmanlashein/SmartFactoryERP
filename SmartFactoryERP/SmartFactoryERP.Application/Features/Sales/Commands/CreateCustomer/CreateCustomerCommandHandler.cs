using MediatR;
using SmartFactoryERP.Domain.Entities.Sales;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Sales.Commands.CreateCustomer
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, int>
    {
        private readonly ISalesRepository _salesRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCustomerCommandHandler(ISalesRepository salesRepository, IUnitOfWork unitOfWork)
        {
            _salesRepository = salesRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            // 1. Check for duplicate email
            if (!string.IsNullOrEmpty(request.Email))
            {
                bool isUnique = await _salesRepository.IsEmailUniqueAsync(request.Email, cancellationToken);
                if (!isUnique)
                {
                    throw new Exception($"Customer with email '{request.Email}' already exists.");
                }
            }

            // 2. Create Entity
            var customer = Customer.Create(
                request.CustomerName,
                request.Email,
                request.PhoneNumber,
                request.Address,
                request.TaxNumber,
                request.CreditLimit
            );

            // 3. Add to Repository
            await _salesRepository.AddCustomerAsync(customer, cancellationToken);

            // 4. Save Changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return customer.Id;
        }
    }
}
