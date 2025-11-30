using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Tasks.Queries.GetTasks
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }

        // بيانات الموظف (للعرض)
        public int? AssignedEmployeeId { get; set; }
        public string AssignedEmployeeName { get; set; }
    }
}
