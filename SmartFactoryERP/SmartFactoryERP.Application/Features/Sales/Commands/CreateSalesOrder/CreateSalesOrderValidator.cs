using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Sales.Commands.CreateSalesOrder
{
    public class CreateSalesOrderValidator : AbstractValidator<CreateSalesOrderCommand>
    {
        public CreateSalesOrderValidator()
        {
            RuleFor(p => p.CustomerId)
                .GreaterThan(0).WithMessage("Valid Customer ID is required.");

            RuleFor(p => p.Items)
                .NotEmpty().WithMessage("Sales Order must contain at least one item.");

            RuleForEach(p => p.Items).SetValidator(new CreateSalesOrderItemValidator());
        }
    }

    public class CreateSalesOrderItemValidator : AbstractValidator<CreateSalesOrderItemDto>
    {
        public CreateSalesOrderItemValidator()
        {
            RuleFor(i => i.MaterialId).GreaterThan(0);
            RuleFor(i => i.Quantity).GreaterThan(0);
            RuleFor(i => i.UnitPrice).GreaterThan(0);
        }
    }
}
