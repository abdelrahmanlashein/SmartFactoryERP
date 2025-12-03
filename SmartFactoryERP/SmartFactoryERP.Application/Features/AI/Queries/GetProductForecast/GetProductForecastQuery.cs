using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.AI.Queries.GetProductForecast
{
    // هذا الكلاس يمثل "سؤال" نرسله للـ Mediator
    // ويرجع لنا الإجابة في شكل ProductForecastDto
    public class GetProductForecastQuery : IRequest<ProductForecastDto>
    {
        public int ProductId { get; set; } // المنتج اللي عايزين نتوقع مبيعاته
    }
}
