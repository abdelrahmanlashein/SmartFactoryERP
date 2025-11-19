using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Purchasing.Commands.UpdateSupplier
{
    public class UpdateSupplierValidator : AbstractValidator<UpdateSupplierCommand>
    {
        public UpdateSupplierValidator()
        {
            RuleFor(p => p.Id)
                .GreaterThan(0).WithMessage("ID must be valid.");

            RuleFor(p => p.SupplierName)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.Email)
                .EmailAddress().When(p => !string.IsNullOrEmpty(p.Email));

            RuleFor(p => p.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.");
        }
    }
}
