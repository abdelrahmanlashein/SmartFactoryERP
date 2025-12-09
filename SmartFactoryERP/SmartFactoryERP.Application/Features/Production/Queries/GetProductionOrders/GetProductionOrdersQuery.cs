using MediatR;
using System.Collections.Generic;

namespace SmartFactoryERP.Application.Features.Production.Queries.GetProductionOrders
{
    /// <summary>
    /// Query ··Õ’Ê· ⁄·Ï Ã„Ì⁄ √Ê«„— «·≈‰ «Ã
    /// </summary>
    public class GetProductionOrdersQuery : IRequest<List<ProductionOrderDto>>
    {
        // Ì„ﬂ‰ ≈÷«›… ›·« — Â‰« ·«Õﬁ«
        public string Status { get; set; } // «Œ Ì«—Ì: ›· —… Õ”» «·Õ«·…
    }
}