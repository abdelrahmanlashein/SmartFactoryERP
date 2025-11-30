using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Expenses.Queries.GetExpenses
{
    public class GetExpensesQuery : IRequest<List<ExpenseDto>> { }
}
