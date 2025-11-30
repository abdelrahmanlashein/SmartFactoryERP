using SmartFactoryERP.Domain.Entities.HR___Departments;
using SmartFactoryERP.Domain.Entities.Shared;
using SmartFactoryERP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.Expenses
{
    public class Expense : BaseAuditableEntity, IAggregateRoot
    {
        public string Description { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime ExpenseDate { get; private set; }
        public ExpenseCategory Category { get; private set; }

        // ممكن نربطه بموظف (مين اللي صرف؟)
        public int? EmployeeId { get; private set; }

        private Expense() { }

        public static Expense Create(string desc, decimal amount, DateTime date, ExpenseCategory category, int? empId)
        {
            if (amount <= 0) throw new Exception("Amount must be > 0");
            if (string.IsNullOrWhiteSpace(desc)) throw new Exception("Description is required");

            return new Expense
            {
                Description = desc,
                Amount = amount,
                ExpenseDate = date,
                Category = category,
                EmployeeId = empId
            };
        }
    }
}
