using MediatR;
using SmartFactoryERP.Domain.Entities.Production;
using SmartFactoryERP.Domain.Interfaces;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Production.Commands.CreateProductionOrder
{
    public class CreateProductionOrderCommandHandler : IRequestHandler<CreateProductionOrderCommand, int>
    {
        private readonly IProductionRepository _productionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductionOrderCommandHandler(IProductionRepository productionRepository, IUnitOfWork unitOfWork)
        {
            _productionRepository = productionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateProductionOrderCommand request, CancellationToken cancellationToken)
        {
            // 1. Create the Production Order Entity
            // The Domain Entity handles the creation logic (Status = Planned, OrderNumber generation)
            var order = ProductionOrder.Create(
                request.ProductId,
                request.Quantity,
                request.StartDate,
                request.Notes
            );

            // 2. Add to Repository
            await _productionRepository.AddProductionOrderAsync(order, cancellationToken);

            // 3. Save Changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return order.Id;
        }
    }
}
