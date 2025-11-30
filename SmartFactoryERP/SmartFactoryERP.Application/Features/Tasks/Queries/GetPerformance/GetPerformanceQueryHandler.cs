using MediatR;
using SmartFactoryERP.Domain.Enums;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Tasks.Queries.GetPerformance
{
    public class GetPerformanceQueryHandler : IRequestHandler<GetPerformanceQuery, List<EmployeePerformanceDto>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IHRRepository _hrRepository;

        public GetPerformanceQueryHandler(ITaskRepository taskRepository, IHRRepository hrRepository)
        {
            _taskRepository = taskRepository;
            _hrRepository = hrRepository;
        }

        public async Task<List<EmployeePerformanceDto>> Handle(GetPerformanceQuery request, CancellationToken cancellationToken)
        {
            // 1. جلب الموظفين (إما واحد أو الكل)
            // تعريف القائمة فارغة في البداية
            List<SmartFactoryERP.Domain.Entities.HR.Employee> employees;

            if (request.EmployeeId.HasValue)
            {
                // حالة: طلب موظف محدد
                var employee = await _hrRepository.GetEmployeeByIdAsync(request.EmployeeId.Value, cancellationToken);

                // تأكد أن الموظف موجود لتجنب Error
                if (employee != null)
                {
                    employees = new List<SmartFactoryERP.Domain.Entities.HR.Employee> { employee };
                }
                else
                {
                    employees = new List<SmartFactoryERP.Domain.Entities.HR.Employee>(); // قائمة فارغة
                }
            }
            else
            {
                // حالة: طلب كل الموظفين
                employees = await _hrRepository.GetAllEmployeesAsync(cancellationToken);
            }

            var report = new List<EmployeePerformanceDto>();

            foreach (var emp in employees)
            {
                if (emp == null) continue;

                // 2. جلب مهام الموظف
                var tasks = await _taskRepository.GetTasksByEmployeeIdAsync(emp.Id, cancellationToken);

                var total = tasks.Count;
                var completed = tasks.Count(t => t.Status == WorkTaskStatus.Completed);
                var pending = tasks.Count(t => t.Status == WorkTaskStatus.Pending || t.Status == WorkTaskStatus.InProgress);

                // 3. حساب النسبة المئوية
                double rate = total == 0 ? 0 : Math.Round(((double)completed / total) * 100, 1);

                // 4. تحديد التقييم اللفظي
                string label = "N/A";
                if (total > 0)
                {
                    if (rate >= 90) label = "🌟 Excellent";
                    else if (rate >= 75) label = "✅ Good";
                    else if (rate >= 50) label = "⚠️ Average";
                    else label = "❌ Needs Improvement";
                }

                report.Add(new EmployeePerformanceDto
                {
                    EmployeeId = emp.Id,
                    EmployeeName = emp.FullName,
                    TotalTasks = total,
                    CompletedTasks = completed,
                    PendingTasks = pending,
                    CompletionRate = rate,
                    PerformanceLabel = label
                });
            }

            return report.OrderByDescending(x => x.CompletionRate).ToList();
        }
    }
}
