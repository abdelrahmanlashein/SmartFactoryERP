using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.AI.Queries.GetProductForecast
{
    public class ProductForecastDto
    {
        public int ProductId { get; set; }
        public float PredictedSalesQuantity { get; set; } 
        public string Advice { get; set; }
    }
}
