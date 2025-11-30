using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.HR.Commands.CreateEmployee
{
    public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeCommand>
    {
        public CreateEmployeeValidator()
        {
            RuleFor(p => p.FullName)
                .NotEmpty().WithMessage("Full Name is required.")
                .MaximumLength(200);

            RuleFor(p => p.DepartmentId)
                .GreaterThan(0).WithMessage("Please select a valid Department.");

            RuleFor(p => p.Email)
                .EmailAddress().When(p => !string.IsNullOrEmpty(p.Email))
                .WithMessage("Invalid email format.");
        }
    }
}
