using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Inventory.Commands.AdjustStock
{
    public class AdjustStockValidator : AbstractValidator<AdjustStockCommand>
    {
        public AdjustStockValidator()
        {
            RuleFor(p => p.MaterialId)
                .NotEmpty();

            RuleFor(p => p.Quantity)
                .NotEqual(0).WithMessage("Quantity cannot be zero.");

            RuleFor(p => p.Notes)
                .NotEmpty().WithMessage("Notes are required for adjustments.");
        }
    }
}
