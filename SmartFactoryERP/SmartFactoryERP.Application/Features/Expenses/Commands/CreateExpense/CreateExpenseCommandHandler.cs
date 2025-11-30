using MediatR;
using SmartFactoryERP.Domain.Entities.Expenses;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Expenses.Commands.CreateExpense
{
    public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand, int>
    {
        private readonly IExpenseRepository _repo;
        private readonly IUnitOfWork _uow;

        public CreateExpenseCommandHandler(IExpenseRepository repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<int> Handle(CreateExpenseCommand req, CancellationToken token)
        {
            var expense = Expense.Create(req.Description, req.Amount, req.ExpenseDate, req.Category, req.EmployeeId);
            await _repo.AddExpenseAsync(expense, token);
            await _uow.SaveChangesAsync(token);
            return expense.Id;
        }
    }
}
