using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Production.Commands.CreateBOM
{
    public class CreateBillOfMaterialValidator : AbstractValidator<CreateBillOfMaterialCommand>
    {
        public CreateBillOfMaterialValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Valid Product ID is required.");

            RuleFor(x => x.ComponentId)
                .GreaterThan(0).WithMessage("Valid Component ID is required.")
                .NotEqual(x => x.ProductId).WithMessage("A product cannot be a component of itself.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        }
    }
}
