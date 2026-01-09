using System;
using System.Collections.Generic; // required for list of materials
using MediatR;

namespace SmartFactoryERP.Application.Features.Production.Commands.CreateProductionOrder
{
    public class CreateProductionOrderCommand : IRequest<int>
    {
        public int ProductId { get; set; }      // The Finished Good ID
        public int Quantity { get; set; }       // How many to produce?
        public DateTime StartDate { get; set; } // Planned start date
        public string Notes { get; set; }
        public string Priority { get; set; }

        // New addition: list of materials entered by the user
        public List<OrderItemInputDto> Items { get; set; }

    }

    // DTO for a single material sent in the command
    public class OrderItemInputDto
    {
        public int MaterialId { get; set; }
        // Final required quantity (quantity of raw material needed)
        public decimal Quantity { get; set; }
    }
}