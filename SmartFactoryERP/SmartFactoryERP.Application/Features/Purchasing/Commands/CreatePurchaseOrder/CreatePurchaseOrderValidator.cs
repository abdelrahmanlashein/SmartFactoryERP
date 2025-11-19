using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Purchasing.Commands.CreatePurchaseOrder
{
    public class CreatePurchaseOrderValidator : AbstractValidator<CreatePurchaseOrderCommand>
    {
        public CreatePurchaseOrderValidator()
        {
            RuleFor(p => p.SupplierId)
                .GreaterThan(0).WithMessage("A valid Supplier ID is required.");

            RuleFor(p => p.ExpectedDeliveryDate)
                .GreaterThan(DateTime.Today).WithMessage("Delivery date must be in the future.");

            RuleFor(p => p.Items)
                .NotEmpty().WithMessage("Purchase Order must contain at least one item.");

            // Rule to validate each item in the list
            RuleForEach(p => p.Items).SetValidator(new CreatePurchaseOrderItemValidator());
        }
    }

    // Validator for the nested Item DTO
    public class CreatePurchaseOrderItemValidator : AbstractValidator<CreatePurchaseOrderItemDto>
    {
        public CreatePurchaseOrderItemValidator()
        {
            RuleFor(i => i.MaterialId)
                .GreaterThan(0).WithMessage("Item Material ID is required.");

            RuleFor(i => i.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

            RuleFor(i => i.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than zero.");
        }
    }
}
