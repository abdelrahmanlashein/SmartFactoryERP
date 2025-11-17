using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Inventory.Commands.UpdateMaterial
{
    public class UpdateMaterialValidator : AbstractValidator<UpdateMaterialCommand>
    {
        public UpdateMaterialValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty();

            RuleFor(p => p.MaterialName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(200);

            RuleFor(p => p.UnitPrice)
                .GreaterThan(0);

            RuleFor(p => p.MinimumStockLevel)
                .GreaterThanOrEqualTo(0);
        }
    }
}
