using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Purchasing.Commands.CreateSupplier
{
    public class CreateSupplierValidator : AbstractValidator<CreateSupplierCommand>
    {
        public CreateSupplierValidator()
        {
            RuleFor(p => p.SupplierCode)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(50);

            RuleFor(p => p.SupplierName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(200);

            RuleFor(p => p.Email)
                .EmailAddress().When(p => !string.IsNullOrEmpty(p.Email)) // لو الايميل مكتوب، اتأكد انه صح
                .WithMessage("A valid email address is required.");

            RuleFor(p => p.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.");
        }
    }
}
