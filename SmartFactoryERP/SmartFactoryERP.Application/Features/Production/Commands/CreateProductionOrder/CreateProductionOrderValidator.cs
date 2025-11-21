using System;
using System.Collections.Generic;
using FluentValidation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Production.Commands.CreateProductionOrder
{
    public class CreateProductionOrderValidator : AbstractValidator<CreateProductionOrderCommand>
    {
        public CreateProductionOrderValidator()
        {
            RuleFor(p => p.ProductId)
                .GreaterThan(0).WithMessage("Valid Product ID is required.");

            RuleFor(p => p.Quantity)
                .GreaterThan(0).WithMessage("Production quantity must be greater than zero.");

            RuleFor(p => p.StartDate)
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Start Date cannot be in the past.");
        }
    }
}
