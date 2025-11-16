using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Inventory.Commands.CreateMaterial
{
    public class CreateMaterialValidator : AbstractValidator<CreateMaterialCommand>
    {
        public CreateMaterialValidator()
        {
            RuleFor(p => p.MaterialCode)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(p => p.MaterialName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(200).WithMessage("{PropertyName} must not exceed 200 characters.");

            RuleFor(p => p.UnitPrice)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than zero.");

            RuleFor(p => p.MinimumStockLevel)
                .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} cannot be negative.");
        }
    }
}

