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
            // 👇 1. التحقق: هل هذا المنتج له وصفة (BOM)؟
            var bom = await _productionRepository.GetBOMForProductAsync(request.ProductId, cancellationToken);

            if (bom == null || bom.Count == 0)
            {
                // نرفض الطلب فوراً
                throw new Exception($"Cannot create order. Product ID {request.ProductId} has no Bill of Materials (Recipe). Please define BOM first.");
            }

            // 2. لو الوصفة موجودة، نكمل عادي...
            var order = ProductionOrder.Create(
                request.ProductId,
                request.Quantity,
                request.StartDate,
                request.Notes
            );

            await _productionRepository.AddProductionOrderAsync(order, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return order.Id;
        }
    }
}
