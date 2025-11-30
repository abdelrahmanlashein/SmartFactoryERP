using SmartFactoryERP.Domain.Entities.HR;
using SmartFactoryERP.Domain.Entities.HR___Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.Production
{
    public class ProductionExecution
    {
        public int ExecutionID { get; set; } // (PK)
        public int ProductionOrderID { get; set; } // (FK)
        public int MachineID { get; set; } // (FK)
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; } // Nullable, as it might be running
        public int ProducedQuantity { get; set; }
        public int DefectiveQuantity { get; set; }
        public int EmployeeID { get; set; } // (FK)

        // Navigation Properties
        public virtual ProductionOrder ProductionOrder { get; set; }
        public virtual Machine Machine { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
