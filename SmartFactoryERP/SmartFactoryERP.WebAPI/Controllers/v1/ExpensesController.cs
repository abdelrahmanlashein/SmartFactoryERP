using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartFactoryERP.Application.Features.Expenses.Commands.CreateExpense;
using SmartFactoryERP.Application.Features.Expenses.Queries.GetExpenses;

namespace SmartFactoryERP.WebAPI.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ExpensesController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateExpenseCommand command) //tested
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() //tested
        {
            return Ok(await Mediator.Send(new GetExpensesQuery()));
        }
    }
}
