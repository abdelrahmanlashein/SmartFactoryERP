using MediatR;
using SmartFactoryERP.Domain.Entities.Expenses;
using SmartFactoryERP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Expenses.Commands.CreateExpense
{
    public class CreateExpenseCommand : IRequest<int>
    {
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public ExpenseCategory Category { get; set; }
        public int? EmployeeId { get; set; }
    }
}
