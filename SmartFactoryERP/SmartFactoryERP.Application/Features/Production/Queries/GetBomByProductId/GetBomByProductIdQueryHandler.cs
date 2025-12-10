using MediatR;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Production.Queries.GetBomByProductId
{
    public class GetBomByProductIdQueryHandler : IRequestHandler<GetBomByProductIdQuery, BomDto>
    {
        private readonly IProductionRepository _repository;

        public GetBomByProductIdQueryHandler(IProductionRepository repository)
        {
            _repository = repository;
        }

        public async Task<BomDto> Handle(GetBomByProductIdQuery request, CancellationToken cancellationToken)
        {
            var bomLines = await _repository.GetBomLinesByProductIdAsync(request.ProductId);

            if (bomLines == null || !bomLines.Any())
            {
                return null;
            }

            return new BomDto
            {
                ProductId = request.ProductId,
                // ✅✅ التعديل هنا: استخدام BomLineDto بدلاً من BomComponentDto ✅✅
                Components = bomLines.Select(line => new BomLineDto
                {
                    ComponentId = line.ComponentId,
                    ComponentName = line.Component?.MaterialName ?? "Unknown Material",
                    Quantity = line.Quantity
                }).ToList()
            };
        }
    }
}

