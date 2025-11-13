using SmartFactoryERP.Domain.Entities.HR___Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.Performance___Task_Management
{
    public enum TaskPriority
    {
        Low,
        Medium,
        High
    }
    public enum TaskStatus
    {
        ToDo,
        InProgress,
        Done,
        Cancelled
    }
    public class Task
    {
        public int TaskID { get; set; } // (PK)
        public string TaskTitle { get; set; }
        public string TaskDescription { get; set; }

        public int AssignedTo { get; set; } // (FK -> Employee)
        public int AssignedBy { get; set; } // (FK -> Employee)
        public int DepartmentID { get; set; } // (FK)

        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? CompletionDate { get; set; } // Nullable

        public TaskPriority Priority { get; set; }
        public TaskStatus Status { get; set; }

        public DateTime CreatedDate { get; set; }

        // Navigation Properties
        public virtual Employee AssignedToEmployee { get; set; }
        public virtual Employee AssignedByEmployee { get; set; }
        public virtual Department Department { get; set; }
    }
}
