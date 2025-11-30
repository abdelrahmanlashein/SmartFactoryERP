using SmartFactoryERP.Domain.Entities.HR;
using SmartFactoryERP.Domain.Entities.HR___Departments;
using SmartFactoryERP.Domain.Entities.Shared;
using SmartFactoryERP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.Performance___Task_Management
{
    public class WorkTask : BaseAuditableEntity, IAggregateRoot
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime DueDate { get; private set; } // الموعد النهائي
        public WorkTaskStatus Status { get; private set; }
        public TaskPriority Priority { get; private set; }

        // الموظف المسؤول (Assigned To)
        public int? AssignedEmployeeId { get; private set; }
        public virtual Employee AssignedEmployee { get; private set; }

        private WorkTask() { }

        public static WorkTask Create(string title, string desc, DateTime dueDate, TaskPriority priority, int? employeeId)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new Exception("Task Title is required");
            if (dueDate < DateTime.UtcNow) throw new Exception("Due date cannot be in the past");

            return new WorkTask
            {
                Title = title,
                Description = desc,
                DueDate = dueDate,
                Priority = priority,
                AssignedEmployeeId = employeeId,
                Status = WorkTaskStatus.Pending,
                CreatedDate = DateTime.UtcNow
            };
        }

        public void StartTask()
        {
            Status = WorkTaskStatus.InProgress;
        }

        public void CompleteTask()
        {
            Status = WorkTaskStatus.Completed;
        }
    }
}
