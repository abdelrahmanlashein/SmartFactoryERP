using MediatR;
using SmartFactoryERP.Domain.Interfaces.AI;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using SmartFactoryERP.Domain.Models.AI; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.AI.Queries.GetProductForecast
{
    public class GetProductForecastQueryHandler : IRequestHandler<GetProductForecastQuery, ProductForecastDto>
    {
        private readonly ISalesRepository _salesRepository;
        private readonly IForecastingService _aiService; // 👈 التعامل مع Interface

        public GetProductForecastQueryHandler(ISalesRepository salesRepo, IForecastingService aiService)
        {
            _salesRepository = salesRepo;
            _aiService = aiService;
        }

        public async Task<ProductForecastDto> Handle(GetProductForecastQuery request, CancellationToken token)
        {
            // 1. هات التاريخ من الداتابيز
            var history = await _salesRepository.GetSalesHistoryAsync(request.ProductId, token);

            // 2. اطلب التوقع من الـ AI
            var result = _aiService.PredictNextMonth(history);

            // 3. جهز الرد
            return new ProductForecastDto
            {
                ProductId = request.ProductId,
                PredictedSalesQuantity = (int)Math.Ceiling(result.PredictedSales), // تقريب لأقرب رقم صحيح
                Advice = result.PredictedSales > 20 ? "🔥 High demand expected! Stock up." : "📉 Demand is stable."
            };
        }
    }

}
