using System;

namespace SmartFactoryERP.Application.Features.Production.Queries.GetProductionOrders
{
    public class ProductionOrderDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}