using MediatR;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Expenses.Queries.GetExpenses
{
    public class GetExpensesQueryHandler : IRequestHandler<GetExpensesQuery, List<ExpenseDto>>
    {
        private readonly IExpenseRepository _repo;
        public GetExpensesQueryHandler(IExpenseRepository repo) => _repo = repo;

        public async Task<List<ExpenseDto>> Handle(GetExpensesQuery req, CancellationToken token)
        {
            var list = await _repo.GetAllExpensesAsync(token);
            return list.Select(e => new ExpenseDto
            {
                Id = e.Id,
                Description = e.Description,
                Amount = e.Amount,
                Date = e.ExpenseDate,
                Category = e.Category.ToString()
            }).ToList();
        }
    }
}
