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
            // 1. Product ID validation
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Valid Product ID is required.");

            // 2. Components list must not be empty
            RuleFor(x => x.Components)
                .NotEmpty().WithMessage("At least one component is required.");

            // 3. Validate each component
            RuleForEach(x => x.Components).ChildRules(component =>
            {
                component.RuleFor(c => c.ComponentId)
                    .GreaterThan(0).WithMessage("Valid Component ID is required.");

                component.RuleFor(c => c.Quantity)
                    .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
            });

            // 4. Product cannot be a component of itself
            RuleFor(x => x)
                .Must(command => !command.Components.Any(c => c.ComponentId == command.ProductId))
                .WithMessage("A product cannot be a component of itself.");

            // 5. NEW: Warning about duplicate components (optional - we handle it in handler)
            RuleFor(x => x.Components)
                .Must(components => components.Select(c => c.ComponentId).Distinct().Count() == components.Count)
                .WithMessage("Duplicate components detected. Quantities will be aggregated automatically.")
                .WithSeverity(Severity.Warning); // ⚠️ Warning, not error
        }
    }
}
