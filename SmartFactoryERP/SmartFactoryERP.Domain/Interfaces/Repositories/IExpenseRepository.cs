using SmartFactoryERP.Domain.Entities.Expenses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Interfaces.Repositories
{
    public interface IExpenseRepository
    {
        Task AddExpenseAsync(Expense expense, CancellationToken token);
        Task<List<Expense>> GetAllExpensesAsync(CancellationToken token);
    }
}
