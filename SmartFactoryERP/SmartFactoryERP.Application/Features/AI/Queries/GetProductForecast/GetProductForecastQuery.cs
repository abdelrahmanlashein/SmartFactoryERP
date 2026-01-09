using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.AI.Queries.GetProductForecast
{
    // This class represents the query we send to the Mediator
    // It returns a ProductForecastDto
    public class GetProductForecastQuery : IRequest<ProductForecastDto>
    {
        public int ProductId { get; set; } // The product for which we want to forecast sales
    }
}
