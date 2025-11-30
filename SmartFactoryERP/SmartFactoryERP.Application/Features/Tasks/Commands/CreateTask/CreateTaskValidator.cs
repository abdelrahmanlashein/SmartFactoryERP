using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Tasks.Commands.CreateTask
{
    public class CreateTaskValidator : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskValidator()
        {
            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("Task Title is required.")
                .MaximumLength(200);

            RuleFor(p => p.DueDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("Due date must be in the future.");

            RuleFor(p => p.AssignedEmployeeId)
                .GreaterThan(0).When(p => p.AssignedEmployeeId.HasValue)
                .WithMessage("Invalid Employee ID.");
        }
    }
}
