using Microsoft.EntityFrameworkCore;
using SmartFactoryERP.Domain.Entities.Expenses;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using SmartFactoryERP.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Persistence.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly ApplicationDbContext _context;

        public ExpenseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddExpenseAsync(Expense expense, CancellationToken token)
        {
            await _context.Expenses.AddAsync(expense, token);
        }

        public async Task<List<Expense>> GetAllExpensesAsync(CancellationToken token)
        {
            return await _context.Expenses
                .OrderByDescending(e => e.ExpenseDate)
                .AsNoTracking()
                .ToListAsync(token);
        }
    }
}
